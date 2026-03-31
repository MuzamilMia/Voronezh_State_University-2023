using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FifoScheduler.Presenter;
using FifoScheduler.Shared;

namespace FifoScheduler.View
{
    public partial class Form1 : Form, IMainView
    {
        // Элементы управления формы
        private ListBox lbReadyQueue;
        private ListBox lbBlockedQueue;
        private Label lblRunningProcess;
        private Label lblStats;
        private Button btnStart;
        private Button btnStop;
        private readonly MainPresenter _presenter;

        // События для взаимодействия с презентером
        public event EventHandler StartClicked;
        public event EventHandler StopClicked;

        public Form1()
        {
            InitializeComponent();  // Наша ручная версия инициализации
            _presenter = new MainPresenter(this); // Создаем презентер
        }
        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "FIFO Process Scheduler Simulator — Планирование FIFO";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок для очереди готовых процессов
            var lblReady = new Label
            {
                Text = "Очередь готовых (FIFO)",
                Location = new Point(20, 20),
                Size = new Size(180, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Blue
            };
            this.Controls.Add(lblReady);

            // Список для отображения готовых процессов
            lbReadyQueue = new ListBox
            {
                Location = new Point(20, 50),
                Size = new Size(200, 400),
                Font = new Font("Consolas", 9)
            };
            this.Controls.Add(lbReadyQueue);

            // Заголовок для очереди заблокированных процессов  
            var lblBlocked = new Label
            {
                Text = "Очередь заблокированных",
                Location = new Point(240, 20),
                Size = new Size(180, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblBlocked);

            // Список для отображения заблокированных процессов
            lbBlockedQueue = new ListBox
            {
                Location = new Point(240, 50),
                Size = new Size(200, 400),
                Font = new Font("Consolas", 9)
            };
            this.Controls.Add(lbBlockedQueue);

            // Заголовок для выполняющегося процесса
            var lblRunningTitle = new Label
            {
                Text = "Выполняющийся процесс:",
                Location = new Point(460, 20),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };
            this.Controls.Add(lblRunningTitle);

            // Метка для отображения текущего выполняющегося процесса
            lblRunningProcess = new Label
            {
                Location = new Point(460, 50),
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(lblRunningProcess);

            // Метка для статистики
            lblStats = new Label
            {
                Location = new Point(460, 140),
                Size = new Size(300, 25),
                Font = new Font("Arial", 9),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblStats);

            // Кнопка "Старт"
            btnStart = new Button
            {
                Location = new Point(460, 180),
                Size = new Size(100, 35),
                Text = "Старт",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnStart.Click += (s, e) =>
            {
                // Генерируем событие и меняем состояние кнопок
                StartClicked?.Invoke(this, EventArgs.Empty);
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            };
            this.Controls.Add(btnStart);

            // Кнопка "Стоп"
            btnStop = new Button
            {
                Location = new Point(570, 180),
                Size = new Size(100, 35),
                Text = "Стоп",
                Enabled = false,
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnStop.Click += (s, e) =>
            {
                // Генерируем событие и меняем состояние кнопок
                StopClicked?.Invoke(this, EventArgs.Empty);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            };
            this.Controls.Add(btnStop);

            // Обработчик закрытия формы - останавливаем симуляцию
            this.FormClosing += (s, e) => _presenter?.Stop();
        }

        /// Обновление списка готовых процессов
        /// <param name="processes">Список строк с описанием процессов</param>
        public void UpdateReadyQueue(List<string> processes)
        {
            // Проверяем, нужно ли вызывать через Invoke (работаем из другого потока)
            if (lbReadyQueue.InvokeRequired)
            {
                lbReadyQueue.Invoke(new Action<List<string>>(UpdateReadyQueue), processes);
                return;
            }
            // Обновляем источник данных ListBox
            lbReadyQueue.DataSource = processes.Count > 0 ? processes : new List<string> { "Пусто" };
        }

        /// <param name="processes">Список строк с описанием процессов</param>
        public void UpdateBlockedQueue(List<string> processes)
        {
            if (lbBlockedQueue.InvokeRequired)
            {
                lbBlockedQueue.Invoke(new Action<List<string>>(UpdateBlockedQueue), processes);
                return;
            }
            lbBlockedQueue.DataSource = processes.Count > 0 ? processes : new List<string> { "Пусто" };
        }

        /// Обновление информации о выполняющемся процессе
        /// 
        /// <param name="name">Имя процесса</param>
        /// <param name="remaining">Оставшееся время выполнения</param>
        public void UpdateRunningProcess(string name, int remaining)
        {
            if (lblRunningProcess.InvokeRequired)
            {
                lblRunningProcess.Invoke(new Action<string, int>(UpdateRunningProcess), name, remaining);
                return;
            }
            // Если процесса нет или он пустой
            if (string.IsNullOrEmpty(name) || name == "None" || name == "Пусто")
            {
                lblRunningProcess.Text = "НЕТ ВЫПОЛНЯЮЩЕГОСЯ ПРОЦЕССА";
                lblRunningProcess.BackColor = Color.LightGray;
            }
            else
            {
                // Отображаем процесс с эмодзи и информацией
                lblRunningProcess.Text = $"🔥 {name}\n⏱️ Осталось: {remaining}";
                // Меняем цвет в зависимости от оставшегося времени
                lblRunningProcess.BackColor = remaining > 5 ? Color.LightGreen : Color.Orange;
            }
        }

        /// <summary>
        /// Обновление статистики
        /// </summary>
        /// <param name="stats">Строка статистики</param>
        public void UpdateStats(string stats)
        {
            if (lblStats.InvokeRequired)
            {
                lblStats.Invoke(new Action<string>(UpdateStats), stats);
                return;
            }
            lblStats.Text = $"Состояние: {stats}";
        }
    }
}