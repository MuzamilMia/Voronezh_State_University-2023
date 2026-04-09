using System;

namespace HousingManagementSystem
{
    // Класс Request представляет запрос на обслуживание в системе
    public class Request : ObservableEntity
    {
        // Уникальный идентификатор запроса
        public int Id { get; }
        // Тип запроса (например, Plumbing, Electrical)
        public RequestType Type { get; }
        // Описание запроса
        public string Description { get; }
        // Адрес, связанный с запросом
        public string Address { get; }
        // Дата создания запроса
        public DateTime CreatedDate { get; }
        // Текущий статус запроса (Created, Assigned, InProgress, Completed, Cancelled)
        public RequestStatus Status { get; private set; }
        // Сотрудник, назначенный на запрос
        public Employee AssignedEmployee { get; private set; }
        // Дата завершения запроса (если завершен)
        public DateTime? CompletionDate { get; private set; }

        // Конструктор для создания нового запроса
        public Request(int id, RequestType type, string description, string address)
        {
            // Инициализация свойств запроса
            Id = id;
            Type = type;
            Description = description;
            Address = address;
            CreatedDate = DateTime.Now;
            Status = RequestStatus.Created;
        }

        // Назначение запроса сотруднику
        public void AssignTo(Employee employee)
        {
            // Проверка, что запрос находится в статусе Created
            if (Status != RequestStatus.Created)
                throw new HousingServiceException("Запрос уже назначен или завершен");

            // Проверка, что сотрудник может обработать запрос
            if (!employee.CanHandleRequest(this))
                throw new HousingServiceException("Сотрудник не может обработать этот тип запроса");

            // Назначение сотрудника и обновление статуса
            AssignedEmployee = employee;
            Status = RequestStatus.Assigned;
            // Установка статуса сотрудника как занятого
            employee.IsAvailable = false;
            // Уведомление об изменении запроса
            OnChanged();
        }

        // Начало работы над запросом
        public void StartWork()
        {
            // Проверка, что запрос находится в статусе Assigned
            if (Status != RequestStatus.Assigned)
                throw new HousingServiceException("Запрос не назначен сотруднику");

            // Обновление статуса на InProgress
            Status = RequestStatus.InProgress;
            // Уведомление об изменении запроса
            OnChanged();
        }

        // Завершение запроса
        public void Complete()
        {
            // Проверка, что запрос находится в статусе InProgress
            if (Status != RequestStatus.InProgress)
                throw new HousingServiceException("Запрос не находится в процессе выполнения");

            // Обновление статуса и установка даты завершения
            Status = RequestStatus.Completed;
            CompletionDate = DateTime.Now;
            // Освобождение сотрудника
            AssignedEmployee.IsAvailable = true;
            // Уведомление об изменении запроса
            OnChanged();
        }

        // Отмена запроса
        public void Cancel()
        {
            // Проверка, что запрос не завершен и не отменен
            if (Status == RequestStatus.Completed || Status == RequestStatus.Cancelled)
                throw new HousingServiceException("Нельзя отменить завершенный запрос");

            // Обновление статуса на Cancelled
            Status = RequestStatus.Cancelled;
            // Освобождение сотрудника, если он был назначен
            if (AssignedEmployee != null)
                AssignedEmployee.IsAvailable = true;
            // Уведомление об изменении запроса
            OnChanged();
        }
    }
}