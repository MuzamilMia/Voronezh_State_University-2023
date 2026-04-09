using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HousingManagementSystem.Controllers;
using HousingManagementSystem.Models;

namespace HousingManagementSystem.Views
{
    // Главная форма приложения для управления жилищными запросами
    public partial class MainForm : Form
    {
        // Сервис и контроллер для управления данными и логикой
        private readonly HousingService _service;
        private readonly HousingServiceController _controller;

        // Конструктор формы, инициализирует компоненты и сервисы
        public MainForm()
        {
            InitializeComponent();

            // Создание экземпляров сервиса и контроллера
            _service = new HousingService();
            _controller = new HousingServiceController(_service, this);
            // Настройка таблиц сотрудников и запросов
            InitializeGrids();
            // Настройка обработчиков событий
            SetupEventHandlers();
            // Запуск автоматической демонстрации
            StartAutoDemo();
        }

        // Инициализация таблиц dgvEmployees и dgvRequests
        private void InitializeGrids()
        {
            // Очистка существующих столбцов
            dgvEmployees.Columns.Clear();
            dgvRequests.Columns.Clear();

            // Настройка столбцов для таблицы сотрудников
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                Width = 50
            });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Имя",
                Width = 120
            });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPosition",
                HeaderText = "Должность",
                Width = 100
            });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Статус",
                Width = 80
            });

            // Настройка столбцов для таблицы запросов
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqId",
                HeaderText = "ID",
                Width = 50
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqType",
                HeaderText = "Тип",
                Width = 80
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqDesc",
                HeaderText = "Описание",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqAddress",
                HeaderText = "Адрес",
                Width = 120
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqStatus",
                HeaderText = "Статус",
                Width = 90
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqEmployee",
                HeaderText = "Назначен",
                Width = 120
            });
            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colReqCreated",
                HeaderText = "Создан",
                Width = 120
            });

            // Настройка таблиц: только для чтения, без заголовков строк и добавления новых строк
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.AllowUserToAddRows = false;

            dgvRequests.ReadOnly = true;
            dgvRequests.RowHeadersVisible = false;
            dgvRequests.AllowUserToAddRows = false;
        }

        // Настройка обработчиков событий для сервиса и элементов управления
        private void SetupEventHandlers()
        {
            // Обновление таблицы сотрудников при изменении данных
            _service.EmployeesChanged += (s, e) => UpdateEmployeesGrid();
            // Обновление таблицы запросов при изменении данных
            _service.RequestsChanged += (s, e) => UpdateRequestsGrid();
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
            lblStatus.Text = message;
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
            if (btnStartDemo.InvokeRequired)
            {
                btnStartDemo.Invoke(new Action<bool>(EnableStartDemoButton), enabled);
                return;
            }
            btnStartDemo.Enabled = enabled;
        }

        // Включение/отключение кнопки StopDemo
        public void EnableStopDemoButton(bool enabled)
        {
            if (btnStopDemo.InvokeRequired)
            {
                btnStopDemo.Invoke(new Action<bool>(EnableStopDemoButton), enabled);
                return;
            }
            btnStopDemo.Enabled = enabled;
        }

        // Обновление таблицы сотрудников (dgvEmployees)
        private void UpdateEmployeesGrid()
        {
            // Потокобезопасность: вызов в основном потоке
            if (dgvEmployees.InvokeRequired)
            {
                dgvEmployees.Invoke(new Action(UpdateEmployeesGrid));
                return;
            }

            // Приостановка отрисовки для оптимизации
            dgvEmployees.SuspendLayout();
            // Очистка существующих строк
            dgvEmployees.Rows.Clear();

            // Добавление строк для каждого сотрудника
            foreach (var employee in _service.Employees)
            {
                int rowIndex = dgvEmployees.Rows.Add(
                    employee.Id,
                    employee.Name,
                    employee.Position,
                    employee.IsAvailable ? "Доступен" : "Занят"
                );

                // Цвет статуса: зеленый для доступных, красный для занятых
                var statusCell = dgvEmployees.Rows[rowIndex].Cells["colStatus"];
                statusCell.Style.ForeColor = employee.IsAvailable ? Color.Green : Color.Red;
            }

            // Возобновление отрисовки
            dgvEmployees.ResumeLayout();
        }

        // Обновление таблицы запросов (dgvRequests)
        private void UpdateRequestsGrid()
        {
            // Потокобезопасность: вызов в основном потоке
            if (dgvRequests.InvokeRequired)
            {
                dgvRequests.Invoke(new Action(UpdateRequestsGrid));
                return;
            }

            // Приостановка отрисовки для оптимизации
            dgvRequests.SuspendLayout();
            // Очистка существующих строк
            dgvRequests.Rows.Clear();

            // Добавление строк для каждого запроса, сортировка по дате создания
            foreach (var request in _service.Requests.OrderByDescending(r => r.CreatedDate))
            {
                int rowIndex = dgvRequests.Rows.Add(
                    request.Id,
                    request.Type,
                    request.Description,
                    request.Address,
                    request.Status,
                    request.AssignedEmployee?.Name ?? "Не назначен",
                    request.CreatedDate.ToString("g")
                );

                // Цвет статуса запроса в зависимости от его состояния
                var statusCell = dgvRequests.Rows[rowIndex].Cells["colReqStatus"];
                switch (request.Status)
                {
                    case RequestStatus.Completed:
                        statusCell.Style.ForeColor = Color.Green;
                        break;
                    case RequestStatus.InProgress:
                        statusCell.Style.ForeColor = Color.Blue;
                        break;
                    case RequestStatus.Assigned:
                        statusCell.Style.ForeColor = Color.Orange;
                        break;
                    case RequestStatus.Cancelled:
                        statusCell.Style.ForeColor = Color.Gray;
                        break;
                    default:
                        statusCell.Style.ForeColor = Color.Black;
                        break;
                }
            }

            // Возобновление отрисовки
            dgvRequests.ResumeLayout();
        }

        // Освобождение ресурсов при закрытии формы
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Освобождение контроллера и сервиса
                _controller.Dispose();
                _service.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Открытие формы Test при нажатии на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            // Создание и отображение формы Test с тем же сервисом и контроллером
            var testForm = new Test(_service, _controller);
            testForm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}