using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HousingManagementSystem.Controllers;
using HousingManagementSystem.Models;

namespace HousingManagementSystem.Views
{
    // Форма Test отображает сотрудников и запросы в двух панелях: pnlEmployees и pnlRequests
    public partial class Test : Form
    {
        // Сервис и контроллер для управления данными и логикой
        private readonly HousingService _service;
        private readonly HousingServiceController _controller;
        // Словари для хранения PictureBox и Label для сотрудников и запросов
        private readonly Dictionary<int, PictureBox> _employeePictureBoxes = new Dictionary<int, PictureBox>();
        private readonly Dictionary<int, PictureBox> _requestPictureBoxes = new Dictionary<int, PictureBox>();
        private readonly Dictionary<int, Label> _employeeLabels = new Dictionary<int, Label>();
        private readonly Dictionary<int, Label> _requestLabels = new Dictionary<int, Label>();
        // Флаг для предотвращения одновременных обновлений дисплея сотрудников
        private bool _isUpdatingEmployees = false;

        // Конструктор формы, инициализирует сервис и контроллер
        public Test(HousingService service, HousingServiceController controller)
        {
            InitializeComponent();

            // Проверка на null для сервиса и контроллера
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            // Настройка обработчиков событий
            SetupEventHandlers();
            // Начальное обновление дисплея сотрудников
            UpdateEmployeesDisplay();
            // Запуск автоматической демонстрации
            StartAutoDemo();
        }

        // Настройка обработчиков событий для сервиса и элементов управления формы
        private void SetupEventHandlers()
        {
            // Обновление дисплея сотрудников при изменении данных
            _service.EmployeesChanged += (s, e) =>
            {
                UpdateEmployeesDisplay();
                UpdateStatus($"Событие EmployeesChanged вызвано. Количество сотрудников: {_service.Employees.Count}");
            };
            // Обновление дисплея запросов при изменении данных
            _service.RequestsChanged += (s, e) => UpdateRequestsDisplay();
            // Обработчики кнопок
            btnStartDemo.Click += (s, e) => _controller.StartAutoDemo();
            btnStopDemo.Click += (s, e) => _controller.StopAutoDemo();
            btnExit.Click += (s, e) => Close();
            // Статус при загрузке формы
            Load += (s, e) => UpdateStatus("Авто-демонстрация запущена");
            // Остановка демонстрации при закрытии формы
            FormClosing += (s, e) => _controller.StopAutoDemo();
        }

        // Запуск автоматической демонстрации
        public void StartAutoDemo()
        {
            _controller.StartAutoDemo();
        }

        // Обновление строки статуса (lblStatus) с учетом потокобезопасности
        public void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }
        }

        // Отображение сообщения об ошибке в диалоговом окне
        public void ShowError(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowError), message);
                return;
            }
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Включение/отключение кнопки StartDemo
        public void EnableStartDemoButton(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(EnableStartDemoButton), enabled);
                return;
            }
            btnStartDemo.Enabled = enabled;
        }

        // Включение/отключение кнопки StopDemo
        public void EnableStopDemoButton(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(EnableStopDemoButton), enabled);
                return;
            }
            btnStopDemo.Enabled = enabled;
        }

        // Обновление дисплея сотрудников в pnlEmployees
        private void UpdateEmployeesDisplay()
        {
            // Потокобезопасность: вызов в основном потоке
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateEmployeesDisplay));
                return;
            }

            // Предотвращение одновременных обновлений
            if (_isUpdatingEmployees)
            {
                UpdateStatus("Пропуск UpdateEmployeesDisplay: обновление уже выполняется.");
                return;
            }

            _isUpdatingEmployees = true;
            try
            {
                // Логирование количества сотрудников и их должностей для отладки
                UpdateStatus($"Обновление сотрудников. Количество: {_service.Employees.Count}. Должности: {string.Join(", ", _service.Employees.Select(e => e.Position))}");

                // Очистка существующих элементов управления для предотвращения дублирования
                foreach (var id in _employeePictureBoxes.Keys.ToList())
                {
                    var pb = _employeePictureBoxes[id];
                    var label = _employeeLabels[id];

                    // Безопасное удаление PictureBox
                    if (pb != null && !pb.IsDisposed)
                    {
                        if (pb.Parent != null)
                        {
                            pb.Parent.Controls.Remove(pb);
                        }
                        pb.Dispose();
                    }
                    else
                    {
                        UpdateStatus($"Предупреждение: Пропуск PictureBox для сотрудника ID {id} (null или удален).");
                    }

                    // Безопасное удаление Label
                    if (label != null && !label.IsDisposed)
                    {
                        if (label.Parent != null)
                        {
                            label.Parent.Controls.Remove(label);
                        }
                        label.Dispose();
                    }
                    else
                    {
                        UpdateStatus($"Предупреждение: Пропуск Label для сотрудника ID {id} (null или удален).");
                    }

                    _employeePictureBoxes.Remove(id);
                    _employeeLabels.Remove(id);
                }

                // Добавление PictureBox и Label для каждого сотрудника
                foreach (var employee in _service.Employees)
                {
                    // Получение имени иконки на основе должности
                    string iconName = GetEmployeeIconName(employee.Position);
                    Image employeeIcon = null;
                    try
                    {
                        // Динамическая загрузка иконки из ресурсов
                        var resourceProperty = typeof(Properties.Resources).GetProperty(iconName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                        employeeIcon = resourceProperty?.GetValue(null) as Image;
                        if (employeeIcon == null)
                        {
                            UpdateStatus($"Предупреждение: Иконка {iconName} не найдена для сотрудника {employee.Name} (Должность: {employee.Position}). Используется запасная.");
                            employeeIcon = Properties.Resources.other;
                        }
                    }
                    catch
                    {
                        UpdateStatus($"Ошибка: Не удалось загрузить иконку {iconName} для сотрудника {employee.Name} (Должность: {employee.Position}). Используется запасная.");
                        employeeIcon = Properties.Resources.other;
                    }

                    // Создание PictureBox для отображения иконки сотрудника
                    var pb = new PictureBox
                    {
                        Size = new Size(100, 100),
                        Image = employeeIcon,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Tag = employee.Id,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = employee.IsAvailable ? Color.LightGreen : Color.LightCoral
                    };

                    // Создание Label для имени и должности
                    var label = new Label
                    {
                        Text = $"{employee.Name}\n{employee.Position}",
                        AutoSize = false,
                        Size = new Size(120, 40),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.LightGray,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    // Добавление элементов в pnlEmployees
                    pnlEmployees.Controls.Add(pb);
                    pnlEmployees.Controls.Add(label);

                    // Сохранение элементов в словари
                    _employeePictureBoxes[employee.Id] = pb;
                    _employeeLabels[employee.Id] = label;
                }

                // Логирование успешного добавления элементов
                UpdateStatus($"Сотрудники добавлены в pnlEmployees. Элементов: {pnlEmployees.Controls.Count}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка в UpdateEmployeesDisplay: {ex.Message}");
            }
            finally
            {
                // Сброс флага обновления
                _isUpdatingEmployees = false;
            }
        }

        // Получение имени иконки на основе должности сотрудника
        private string GetEmployeeIconName(string position)
        {
            // Проверка на пустую должность
            if (string.IsNullOrWhiteSpace(position))
                return "other";

            // Сопоставление должностей (русских и английских) с именами иконок
            var positionLower = position.Trim().ToLower();
            switch (positionLower)
            {
                case "сантехник":
                case "plumber":
                    return "plumber";
                case "электрик":
                case "electrician":
                    return "electric";
                case "уборщик":
                case "cleaner":
                    return "cleaner";
                case "садовник":
                case "yardworker":
                    return "yardworker";
                case "разнорабочий":
                case "multiworker":
                    return "multiworker";
                case "механик":
                case "mechanic":
                    return "mechanic";
                default:
                    UpdateStatus($"Предупреждение: Неизвестная должность '{position}'. Используется запасная иконка.");
                    return "other";
            }
        }

        // Обновление дисплея запросов в pnlRequests
        private void UpdateRequestsDisplay()
        {
            // Потокобезопасность: вызов в основном потоке
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateRequestsDisplay));
                return;
            }

            try
            {
                // Удаление PictureBox и Label для запросов, которых больше нет
                var currentRequestIds = _service.Requests.Select(r => r.Id).ToHashSet();
                var toRemove = _requestPictureBoxes.Keys.Where(id => !currentRequestIds.Contains(id)).ToList();
                foreach (var id in toRemove)
                {
                    var pb = _requestPictureBoxes[id];
                    var label = _requestLabels[id];
                    pb.Parent.Controls.Remove(pb);
                    label.Parent.Controls.Remove(label);
                    pb.Dispose();
                    label.Dispose();
                    _requestPictureBoxes.Remove(id);
                    _requestLabels.Remove(id);
                }

                // Добавление или обновление PictureBox и Label для всех запросов
                foreach (var request in _service.Requests)
                {
                    if (!_requestPictureBoxes.ContainsKey(request.Id))
                    {
                        // Создание PictureBox для иконки запроса
                        var pb = new PictureBox
                        {
                            Size = new Size(100, 100),
                            Image = GetRequestIcon(request.Type, request.Status),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Tag = request.Id,
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        // Создание Label с информацией о запросе
                        var label = new Label
                        {
                            Text = GetRequestLabelText(request),
                            AutoSize = false,
                            Size = new Size(120, 120),
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = Color.LightGray,
                            TextAlign = ContentAlignment.TopLeft
                        };

                        // Добавление элементов в pnlRequests
                        pnlRequests.Controls.Add(pb);
                        pnlRequests.Controls.Add(label);
                        _requestPictureBoxes[request.Id] = pb;
                        _requestLabels[request.Id] = label;
                    }

                    // Обновление PictureBox и Label запроса
                    var requestPb = _requestPictureBoxes[request.Id];
                    var requestLabel = _requestLabels[request.Id];
                    requestPb.Image = GetRequestIcon(request.Type, request.Status);

                    // Установка цвета фона в зависимости от статуса запроса
                    switch (request.Status)
                    {
                        case RequestStatus.Created:
                            requestPb.BackColor = Color.White;
                            break;
                        case RequestStatus.Assigned:
                            requestPb.BackColor = Color.LightYellow;
                            break;
                        case RequestStatus.InProgress:
                            requestPb.BackColor = Color.LightBlue;
                            break;
                        case RequestStatus.Completed:
                            requestPb.BackColor = Color.LightGreen;
                            break;
                        case RequestStatus.Cancelled:
                            requestPb.BackColor = Color.LightPink;
                            break;
                    }

                    requestLabel.Text = GetRequestLabelText(request);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка в UpdateRequestsDisplay: {ex.Message}");
            }
        }

        // Получение иконки для запроса на основе типа и статуса
        private Image GetRequestIcon(RequestType type, RequestStatus status)
        {
            // Для завершенных запросов всегда используется checkmark
            if (status == RequestStatus.Completed)
            {
                return Properties.Resources.checkmark;
            }

            // Выбор иконки по типу запроса
            switch (type)
            {
                case RequestType.Plumbing:
                    return Properties.Resources.plumbing;
                case RequestType.Electrical:
                    return Properties.Resources.electrical;
                case RequestType.Cleaning:
                    return Properties.Resources.cleaning;
                case RequestType.YardWork:
                    return Properties.Resources.yardwork;
                case RequestType.Other:
                    return Properties.Resources.other;
                default:
                    return Properties.Resources.other;
            }
        }

        // Формирование текста для Label запроса
        private string GetRequestLabelText(Request request)
        {
            return $"ID: {request.Id}\n" +
                   $"Тип: {request.Type}\n" +
                   $"Адрес: {request.Address}\n" +
                   $"Назначен: {(request.AssignedEmployee?.Name ?? "Не назначен")}\n" +
                   $"Статус: {request.Status}\n" +
                   $"Описание: {request.Description}";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}