using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HousingManagementSystem.Models
{
    // Сервис для управления сотрудниками, запросами и автоматической демонстрацией
    public class HousingService : IDisposable
    {
        // Списки сотрудников и запросов
        private readonly List<Employee> _employees = new List<Employee>();
        private readonly List<Request> _requests = new List<Request>();

        // Объект для синхронизации потоков
        private readonly object _lock = new object();

        // Счетчики для генерации уникальных ID
        private int _nextEmployeeId = 1;
        private int _nextRequestId = 1;

        // Токен для отмены авто-демонстрации
        private CancellationTokenSource _cts;

        // Задача для выполнения авто-демонстрации
        private Task _autoDemoTask;

        // Генератор случайных чисел
        private readonly Random _random = new Random();

        // Список адресов для случайных запросов
        private readonly List<string> _addresses = new List<string>
        {
            "101 Main St", "202 Oak Ave", "303 Pine Rd", "404 Elm Blvd", "505 Maple Ln"
        };
        // Флаг состояния авто-демонстрации
        private bool _isAutoDemoRunning;

        // События для уведомления об изменениях сотрудников и запросов
        public event EventHandler EmployeesChanged;
        public event EventHandler RequestsChanged;

        // Доступ к спискам сотрудников и запросов (только для чтения)
        public IReadOnlyList<Employee> Employees => _employees.AsReadOnly();
        public IReadOnlyList<Request> Requests => _requests.AsReadOnly();

        // Добавление нового сотрудника
        public void AddEmployee(Employee employee)
        {
            lock (_lock)
            {
                // Проверка на существование сотрудника с таким ID
                if (_employees.Any(e => e.Id == employee.Id))
                    throw new HousingServiceException("Сотрудник с таким ID уже существует");

                // Добавление сотрудника и подписка на его изменения
                _employees.Add(employee);
                employee.Changed += OnEmployeeChanged;
                // Уведомление об изменении списка сотрудников
                EmployeesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Удаление сотрудника по ID
        public void RemoveEmployee(int employeeId)
        {
            lock (_lock)
            {
                // Поиск сотрудника
                var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
                if (employee == null)
                    throw new HousingServiceException("Сотрудник не найден");

                // Проверка, что у сотрудника нет активных запросов
                if (_requests.Any(r => r.AssignedEmployee?.Id == employeeId &&
                                    (r.Status == RequestStatus.Assigned || r.Status == RequestStatus.InProgress)))
                    throw new HousingServiceException("У сотрудника есть активные запросы");

                // Удаление сотрудника и отписка от его изменений
                _employees.Remove(employee);
                employee.Changed -= OnEmployeeChanged;
                // Уведомление об изменении списка сотрудников
                EmployeesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Создание нового запроса
        public Request CreateRequest(RequestType type, string description, string address)
        {
            lock (_lock)
            {
                // Проверка входных данных
                if (string.IsNullOrWhiteSpace(description))
                    throw new HousingServiceException("Описание не может быть пустым");
                if (string.IsNullOrWhiteSpace(address))
                    throw new HousingServiceException("Адрес не может быть пустым");

                // Создание запроса с уникальным ID
                var request = new Request(_nextRequestId++, type, description, address);
                // Добавление запроса и подписка на его изменения
                _requests.Add(request);
                request.Changed += OnRequestChanged;
                // Уведомление об изменении списка запросов
                RequestsChanged?.Invoke(this, EventArgs.Empty);
                return request;
            }
        }

        // Назначение запроса сотруднику
        public void AssignRequest(int requestId, int employeeId)
        {
            lock (_lock)
            {
                // Поиск запроса и сотрудника
                var request = _requests.FirstOrDefault(r => r.Id == requestId);
                var employee = _employees.FirstOrDefault(e => e.Id == employeeId);

                if (request == null)
                    throw new HousingServiceException("Запрос не найден");
                if (employee == null)
                    throw new HousingServiceException("Сотрудник не найден");

                // Назначение запроса сотруднику
                request.AssignTo(employee);
            }
        }

        // Запуск автоматической демонстрации
        public void StartAutoDemo()
        {
            lock (_lock)
            {
                // Проверка, что демонстрация не запущена
                if (_isAutoDemoRunning)
                    return;

                _isAutoDemoRunning = true;
                // Создание токена для отмены
                _cts = new CancellationTokenSource();
                // Создание начальных сотрудников
                CreateInitialEmployees();
                // Запуск задачи авто-демонстрации
                _autoDemoTask = Task.Run(() => AutoDemoProcess(_cts.Token));
            }
        }

        // Остановка автоматической демонстрации
        public void StopAutoDemo()
        {
            lock (_lock)
            {
                // Проверка, что демонстрация запущена
                if (!_isAutoDemoRunning)
                    return;

                _isAutoDemoRunning = false;
                // Отмена задачи и ожидание завершения
                _cts?.Cancel();
                _autoDemoTask?.Wait();
                // Освобождение токена
                _cts?.Dispose();
                _cts = null;
            }
        }

        // Создание начальных сотрудников
        private void CreateInitialEmployees()
        {
            lock (_lock)
            {
                // Список сотрудников для создания
                var employees = new List<(string type, string name)>
                {
                    ("plumber", "John Plumbing"),
                    ("plumber", "Mike Pipes"),
                    ("electrician", "Lisa Watts"),
                    ("electrician", "Dave Current"),
                    ("janitor", "Sam Cleaner"),
                    ("janitor", "Anna Sweep"),
                    ("dispatcher", "Dispatch Master")
                };

                // Создание сотрудников через фабрику
                foreach (var emp in employees)
                {
                    try
                    {
                        var employee = EmployeeFactory.CreateEmployee(emp.type, _nextEmployeeId++, emp.name);
                        AddEmployee(employee);
                    }
                    catch (HousingServiceException ex)
                    {
                        // Логирование ошибки создания сотрудника
                        Console.WriteLine($"Ошибка создания сотрудника: {ex.Message}");
                    }
                }
            }
        }

        // Процесс автоматической демонстрации
        private void AutoDemoProcess(CancellationToken token)
        {
            // Цикл выполняется до отмены
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Случайное создание нового запроса (80% вероятность)
                    if (_random.Next(0, 100) < 80)
                    {
                        CreateRandomRequest();
                    }

                    // Обработка запросов в процессе выполнения
                    ProcessAssignedRequests();

                    // Назначение неназначенных запросов
                    AssignUnassignedRequests();

                    // Пауза 2 секунды
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    // Логирование ошибок авто-демонстрации
                    Console.WriteLine($"Ошибка авто-демонстрации: {ex.Message}");
                }
            }
        }

        // Создание случайного запроса
        private void CreateRandomRequest()
        {
            lock (_lock)
            {
                // Выбор случайного типа запроса
                var requestTypes = Enum.GetValues(typeof(RequestType));
                var type = (RequestType)requestTypes.GetValue(_random.Next(requestTypes.Length));
                // Выбор случайного адреса
                var address = _addresses[_random.Next(_addresses.Count)];
                // Описания для типов запросов
                var descriptions = new Dictionary<RequestType, string>
                {
                    { RequestType.Plumbing, "Протечка трубы в ванной" },
                    { RequestType.Electrical, "Не работает светильник" },
                    { RequestType.Cleaning, "Требуется уборка общей зоны" },
                    { RequestType.YardWork, "Требуется уход за двором" },
                    { RequestType.Other, "Общий запрос на обслуживание" }
                };

                // Создание запроса
                CreateRequest(type, descriptions[type], address);
            }
        }

        // Обработка запросов в процессе выполнения
        private void ProcessAssignedRequests()
        {
            lock (_lock)
            {
                // Выбор запросов в статусе "В процессе"
                var inProgressRequests = _requests
                    .Where(r => r.Status == RequestStatus.InProgress)
                    .ToList();

                // Случайное завершение запросов (30% вероятность)
                foreach (var request in inProgressRequests)
                {
                    if (_random.Next(0, 100) < 30)
                    {
                        request.Complete();
                    }
                }
            }
        }

        // Назначение неназначенных запросов
        private void AssignUnassignedRequests()
        {
            lock (_lock)
            {
                // Выбор неназначенных запросов
                var unassignedRequests = _requests
                    .Where(r => r.Status == RequestStatus.Created)
                    .ToList();

                foreach (var request in unassignedRequests)
                {
                    // Поиск доступных сотрудников, способных обработать запрос
                    var availableEmployees = _employees
                        .Where(e => e.IsAvailable && e.CanHandleRequest(request))
                        .ToList();

                    if (availableEmployees.Any())
                    {
                        // Случайный выбор сотрудника и назначение запроса
                        var employee = availableEmployees[_random.Next(availableEmployees.Count)];
                        request.AssignTo(employee);
                        request.StartWork();
                    }
                }
            }
        }

        // Обработчик изменения сотрудника
        private void OnEmployeeChanged(object sender, EventArgs e)
        {
            // Уведомление об изменении списка сотрудников
            EmployeesChanged?.Invoke(this, EventArgs.Empty);
        }

        // Обработчик изменения запроса
        private void OnRequestChanged(object sender, EventArgs e)
        {
            // Уведомление об изменении списка запросов
            RequestsChanged?.Invoke(this, EventArgs.Empty);
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            // Остановка авто-демонстрации
            StopAutoDemo();
            lock (_lock)
            {
                // Отписка от событий сотрудников
                foreach (var employee in _employees)
                    employee.Changed -= OnEmployeeChanged;

                // Отписка от событий запросов
                foreach (var request in _requests)
                    request.Changed -= OnRequestChanged;
            }
        }
    }
}