using System;
using HousingManagementSystem.Models;
using HousingManagementSystem.Views;

namespace HousingManagementSystem.Controllers
{
    // Контроллер для координации между HousingService и пользовательским интерфейсом
    public class HousingServiceController : IDisposable
    {
        // Сервис для управления данными и логикой авто-демонстрации
        private readonly HousingService _service;
        // Главная форма для обновления UI
        private readonly MainForm _view;

        // Конструктор, инициализирует сервис и форму
        public HousingServiceController(HousingService service, MainForm view)
        {
            // Проверка на null для сервиса и формы
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        // Запуск автоматической демонстрации
        public void StartAutoDemo()
        {
            try
            {
                // Запуск авто-демонстрации через сервис
                _service.StartAutoDemo();
                // Обновление статуса на форме
                _view.UpdateStatus("Авто-демонстрация запущена...");
                // Отключение кнопки "Старт" и включение кнопки "Стоп"
                _view.EnableStartDemoButton(false);
                _view.EnableStopDemoButton(true);
            }
            catch (HousingServiceException ex)
            {
                // Отображение ошибки, если запуск не удался
                _view.ShowError($"Не удалось запустить авто-демонстрацию: {ex.Message}");
            }
        }

        // Остановка автоматической демонстрации
        public void StopAutoDemo()
        {
            try
            {
                // Остановка авто-демонстрации через сервис
                _service.StopAutoDemo();
                // Обновление статуса на форме
                _view.UpdateStatus("Демонстрация остановлена");
                // Включение кнопки "Старт" и отключение кнопки "Стоп"
                _view.EnableStartDemoButton(true);
                _view.EnableStopDemoButton(false);
            }
            catch (HousingServiceException ex)
            {
                // Отображение ошибки, если остановка не удалась
                _view.ShowError($"Не удалось остановить авто-демонстрацию: {ex.Message}");
            }
        }

        // Передача сообщения об ошибке на форму
        public void ShowError(string message)
        {
            // Вызов метода формы для отображения ошибки
            _view.ShowError(message);
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            // В данном случае нет ресурсов для освобождения
        }
    }
}