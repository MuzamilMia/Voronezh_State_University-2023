/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace _2.Lab_OS_No12
{
    public partial class MainForm : Form
    {
        // UI Controls
        private DataGridView dgvBuffers;
        private ListView lvWorkers;
        private RichTextBox rtbLog;
        private Button btnStart;
        private Button btnPause;
        private Button btnResume;
        private Button btnSelectDir1;
        private Button btnSelectDir2;
        private TextBox txtDir1;
        private TextBox txtDir2;
        private System.Windows.Forms.Timer uiUpdateTimer;

        // Application data
        private readonly List<DirectoryScanner> scanners = new List<DirectoryScanner>();
        private readonly List<FileScanner> fileScanners = new List<FileScanner>();
        private readonly object collectionsLock = new object();
        private bool isRunning = false;

        public MainForm()
        {
            Setup();
        }

        private void Setup()
        {
            // Basic form setup
            this.Text = "Directory File Scanner - Largest File Finder";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Directory selection - Directory 1
            var lblDir1 = new Label
            {
                Text = "Directory 1:",
                Location = new Point(10, 15),
                Size = new Size(70, 20)
            };

            txtDir1 = new TextBox
            {
                Location = new Point(85, 12),
                Size = new Size(300, 20),
                ReadOnly = true
            };

            btnSelectDir1 = new Button
            {
                Text = "Browse",
                Location = new Point(395, 10),
                Size = new Size(80, 25)
            };

            // Directory selection - Directory 2
            var lblDir2 = new Label
            {
                Text = "Directory 2:",
                Location = new Point(10, 45),
                Size = new Size(70, 20)
            };

            txtDir2 = new TextBox
            {
                Location = new Point(85, 42),
                Size = new Size(300, 20),
                ReadOnly = true
            };

            btnSelectDir2 = new Button
            {
                Text = "Browse",
                Location = new Point(395, 40),
                Size = new Size(80, 25)
            };

            // Control buttons
            btnStart = new Button
            {
                Text = "Start Scanning",
                Location = new Point(500, 10),
                Size = new Size(100, 30)
            };

            btnPause = new Button
            {
                Text = "Pause",
                Location = new Point(610, 10),
                Size = new Size(80, 30),
                Enabled = false
            };

            btnResume = new Button
            {
                Text = "Resume",
                Location = new Point(700, 10),
                Size = new Size(80, 30),
                Enabled = false
            };

            // Data Grid View for directories
            dgvBuffers = new DataGridView
            {
                Location = new Point(10, 80),
                Size = new Size(620, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };

            // List View for workers
            lvWorkers = new ListView
            {
                Location = new Point(640, 80),
                Size = new Size(340, 200),
                View = View.Details,
                FullRowSelect = true
            };

            // Log text box
            rtbLog = new RichTextBox
            {
                Location = new Point(10, 290),
                Size = new Size(970, 350),
                ReadOnly = true,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 9)
            };

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                lblDir1, txtDir1, btnSelectDir1,
                lblDir2, txtDir2, btnSelectDir2,
                btnStart, btnPause, btnResume,
                dgvBuffers, lvWorkers, rtbLog
            });

            // Initialize components
            InitializeDataGridView();
            InitializeListView();
            SetupEventHandlers();

            // UI Update Timer
            uiUpdateTimer = new System.Windows.Forms.Timer();
            uiUpdateTimer.Interval = 500;
            uiUpdateTimer.Tick += UiUpdateTimer_Tick;
        }

        private void InitializeDataGridView()
        {
            dgvBuffers.Columns.Clear();
            dgvBuffers.Columns.Add("Id", "Scanner ID");
            dgvBuffers.Columns.Add("Directory", "Directory Path");
            dgvBuffers.Columns.Add("TotalFiles", "Total Files");
            dgvBuffers.Columns.Add("Processed", "Processed");
            dgvBuffers.Columns.Add("Progress", "Progress");
            dgvBuffers.Columns.Add("LargestFile", "Largest File");
            dgvBuffers.Columns.Add("Size", "Size");
            dgvBuffers.Columns.Add("Status", "Status");

            // Set column widths
            dgvBuffers.Columns["Id"].Width = 70;
            dgvBuffers.Columns["Directory"].Width = 200;
            dgvBuffers.Columns["TotalFiles"].Width = 80;
            dgvBuffers.Columns["Processed"].Width = 80;
            dgvBuffers.Columns["Progress"].Width = 80;
            dgvBuffers.Columns["LargestFile"].Width = 150;
            dgvBuffers.Columns["Size"].Width = 80;
            dgvBuffers.Columns["Status"].Width = 100;
        }

        private void InitializeListView()
        {
            lvWorkers.Columns.Clear();
            lvWorkers.Columns.Add("ID", 60);
            lvWorkers.Columns.Add("Type", 80);
            lvWorkers.Columns.Add("Directory", 150);
            lvWorkers.Columns.Add("Status", 100);
        }

        private void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnPause.Click += BtnPause_Click;
            btnResume.Click += BtnResume_Click;
            btnSelectDir1.Click += (s, e) => SelectDirectory(txtDir1);
            btnSelectDir2.Click += (s, e) => SelectDirectory(txtDir2);
            this.FormClosing += MainForm_FormClosing;
        }

        private void SelectDirectory(TextBox textBox)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select directory to scan";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;

            if (string.IsNullOrEmpty(txtDir1.Text) || string.IsNullOrEmpty(txtDir2.Text))
            {
                MessageBox.Show("Please select both directories before starting.", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(txtDir1.Text) || !Directory.Exists(txtDir2.Text))
            {
                MessageBox.Show("One or both directories do not exist.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            isRunning = true;
            Log("=== Starting Directory Scanning System ===");

            // Clear previous results
            dgvBuffers.Rows.Clear();
            lvWorkers.Items.Clear();
            fileScanners.Clear();
            scanners.Clear();

            // Create and start scanners for both directories
            CreateAndStartScanner(txtDir1.Text);
            CreateAndStartScanner(txtDir2.Text);

            // Start UI update timer
            uiUpdateTimer.Start();

            btnStart.Enabled = false;
            btnPause.Enabled = true;
            btnResume.Enabled = false;

            Log("System started successfully. Scanning directories...");
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (!isRunning) return;

            Log("=== Pausing all scanners ===");

            lock (collectionsLock)
            {
                foreach (var scanner in scanners)
                {
                    scanner.Pause();
                }
            }

            btnPause.Enabled = false;
            btnResume.Enabled = true;
        }

        private void BtnResume_Click(object sender, EventArgs e)
        {
            if (!isRunning) return;

            Log("=== Resuming all scanners ===");

            lock (collectionsLock)
            {
                foreach (var scanner in scanners)
                {
                    scanner.Resume();
                }
            }

            btnPause.Enabled = true;
            btnResume.Enabled = false;
        }

        private void CreateAndStartScanner(string directoryPath)
        {
            try
            {
                var fileScanner = new FileScanner(directoryPath);
                var directoryScanner = new DirectoryScanner(
                    fileScanner,
                    Log,
                    OnScannerCompleted,
                    UpdateWorkerStatus
                );

                lock (collectionsLock)
                {
                    fileScanners.Add(fileScanner);
                    scanners.Add(directoryScanner);
                }

                // Add to UI
                InvokeIfRequired(() =>
                {
                    // Add to DataGridView
                    int rowIndex = dgvBuffers.Rows.Add();
                    var row = dgvBuffers.Rows[rowIndex];
                    row.Cells["Id"].Value = fileScanner.Id;
                    row.Cells["Directory"].Value = directoryPath;
                    row.Cells["TotalFiles"].Value = fileScanner.TotalFiles;
                    row.Cells["Processed"].Value = fileScanner.ProcessedFiles;
                    row.Cells["Progress"].Value = "0%";
                    row.Cells["LargestFile"].Value = "Scanning...";
                    row.Cells["Size"].Value = "";
                    row.Cells["Status"].Value = "Running";
                    row.Tag = fileScanner.Id; // Store scanner ID for updates

                    // Add to ListView
                    ListViewItem item = new ListViewItem(new[] {
                        directoryScanner.Id.ToString(),
                        "Scanner",
                        Path.GetFileName(directoryPath),
                        "Running"
                    });
                    item.Name = $"S{directoryScanner.Id}";
                    item.Tag = fileScanner.Id;
                    lvWorkers.Items.Add(item);
                });

                Log($"Created scanner #{directoryScanner.Id} for directory: {directoryPath}");
                Log($"  Total files to process: {fileScanner.TotalFiles}");
                directoryScanner.Start();
            }
            catch (Exception ex)
            {
                Log($"ERROR creating scanner for {directoryPath}: {ex.Message}");
            }
        }

        private void OnScannerCompleted(DirectoryScanner scanner, FileScanner fileScanner, string reason)
        {
            Log($"Scanner #{scanner.Id} completed. Reason: {reason}");

            lock (collectionsLock)
            {
                scanners.Remove(scanner);
            }

            // Final UI update
            UpdateScannerUI(fileScanner, true);

            InvokeIfRequired(() =>
            {
                // Update worker status
                if (lvWorkers.Items.ContainsKey($"S{scanner.Id}"))
                {
                    var item = lvWorkers.Items[$"S{scanner.Id}"];
                    item.SubItems[3].Text = "Completed";
                }
            });

            // Check if all scanners are done
            lock (collectionsLock)
            {
                if (scanners.Count == 0)
                {
                    Log("=== All scanning completed! ===");
                    uiUpdateTimer.Stop();

                    // Show final results
                    ShowFinalResults();

                    btnStart.Enabled = true;
                    btnPause.Enabled = false;
                    btnResume.Enabled = false;
                    isRunning = false;
                }
            }
        }

        private void UpdateWorkerStatus(string id, string role, string status)
        {
            InvokeIfRequired(() =>
            {
                string key = "S" + id;
                if (lvWorkers.Items.ContainsKey(key))
                {
                    var item = lvWorkers.Items[key];
                    item.SubItems[3].Text = status;
                }
            });
        }

        private void UiUpdateTimer_Tick(object sender, EventArgs e)
        {
            lock (collectionsLock)
            {
                foreach (var fileScanner in fileScanners)
                {
                    UpdateScannerUI(fileScanner, false);
                }
            }
        }

        private void UpdateScannerUI(FileScanner fileScanner, bool isFinal)
        {
            InvokeIfRequired(() =>
            {
                for (int i = 0; i < dgvBuffers.Rows.Count; i++)
                {
                    var row = dgvBuffers.Rows[i];
                    if (row.Tag?.ToString() == fileScanner.Id.ToString())
                    {
                        // Update progress
                        row.Cells["Processed"].Value = fileScanner.ProcessedFiles;

                        // Calculate progress percentage
                        double progress = fileScanner.TotalFiles > 0 ?
                            (double)fileScanner.ProcessedFiles / fileScanner.TotalFiles * 100 : 0;
                        row.Cells["Progress"].Value = $"{progress:0}%";

                        // Update largest file info
                        if (fileScanner.LargestFile != null)
                        {
                            row.Cells["LargestFile"].Value = Path.GetFileName(fileScanner.LargestFile.Name);
                            row.Cells["Size"].Value = FormatFileSize(fileScanner.LargestFile.Length);
                        }

                        // Update status
                        bool isCompleted = fileScanner.ProcessedFiles >= fileScanner.TotalFiles;
                        if (isFinal || isCompleted)
                        {
                            row.Cells["Status"].Value = "Completed";
                        }
                        else
                        {
                            row.Cells["Status"].Value = "Running";
                        }

                        break;
                    }
                }
            });
        }

        private void ShowFinalResults()
        {
            Log("=== FINAL RESULTS ===");

            lock (collectionsLock)
            {
                foreach (var fileScanner in fileScanners)
                {
                    if (fileScanner.LargestFile != null)
                    {
                        Log($"Directory: {fileScanner.DirectoryPath}");
                        Log($"  Largest file: {fileScanner.LargestFile.Name}");
                        Log($"  Size: {FormatFileSize(fileScanner.LargestFile.Length)}");
                        Log($"  Files processed: {fileScanner.ProcessedFiles}/{fileScanner.TotalFiles}");
                        Log("");
                    }
                    else
                    {
                        Log($"Directory: {fileScanner.DirectoryPath}");
                        Log($"  No files found or accessible");
                        Log($"  Files processed: {fileScanner.ProcessedFiles}/{fileScanner.TotalFiles}");
                        Log("");
                    }
                }
            }

            Log("=== END RESULTS ===");
        }

        private void Log(string text)
        {
            string line = $"[{DateTime.Now:HH:mm:ss.fff}] {text}";
            InvokeIfRequired(() =>
            {
                rtbLog.AppendText(line + Environment.NewLine);
                rtbLog.ScrollToCaret();
            });
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            uiUpdateTimer.Stop();
            isRunning = false;

            Log("=== Shutting down system ===");

            lock (collectionsLock)
            {
                foreach (var scanner in scanners)
                {
                    scanner.Stop();
                }
            }

            // Give threads time to finish
            Thread.Sleep(300);
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else
                action();
        }
    }
}*/


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _2.Lab_OS_No12
{
    public partial class MainForm : Form
    {
        // UI Controls
        private DataGridView dgvResults;
        private ListView lvWorkers;
        private RichTextBox rtbLog;
        private Button btnStart;
        private Button btnSelectDir1;
        private Button btnSelectDir2;
        private TextBox txtDir1;
        private TextBox txtDir2;

        // Для синхронизации потоков
        private ManualResetEvent[] completionEvents;
        private int completedScanners = 0;
        private readonly object lockObject = new object();

        public MainForm()
        {
            SetupForm();
        }

        private void SetupForm()
        {
            // Basic form setup
            this.Text = "Directory File Scanner - Largest File Finder";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Directory selection - Directory 1
            var lblDir1 = new Label
            {
                Text = "Directory 1:",
                Location = new Point(10, 15),
                Size = new Size(70, 20)
            };

            txtDir1 = new TextBox
            {
                Location = new Point(85, 12),
                Size = new Size(300, 20),
                ReadOnly = true
            };

            btnSelectDir1 = new Button
            {
                Text = "Browse",
                Location = new Point(395, 10),
                Size = new Size(80, 25)
            };

            // Directory selection - Directory 2
            var lblDir2 = new Label
            {
                Text = "Directory 2:",
                Location = new Point(10, 45),
                Size = new Size(70, 20)
            };

            txtDir2 = new TextBox
            {
                Location = new Point(85, 42),
                Size = new Size(300, 20),
                ReadOnly = true
            };

            btnSelectDir2 = new Button
            {
                Text = "Browse",
                Location = new Point(395, 40),
                Size = new Size(80, 25)
            };

            // Control buttons
            btnStart = new Button
            {
                Text = "Start Scanning",
                Location = new Point(500, 10),
                Size = new Size(100, 30)
            };

            // Data Grid View for directories
            dgvResults = new DataGridView
            {
                Location = new Point(10, 80),
                Size = new Size(620, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };

            // List View for workers
            lvWorkers = new ListView
            {
                Location = new Point(640, 80),
                Size = new Size(340, 200),
                View = View.Details,
                FullRowSelect = true
            };

            // Log text box
            rtbLog = new RichTextBox
            {
                Location = new Point(10, 290),
                Size = new Size(970, 350),
                ReadOnly = true,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 9)
            };

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                lblDir1, txtDir1, btnSelectDir1,
                lblDir2, txtDir2, btnSelectDir2,
                btnStart, dgvResults, lvWorkers, rtbLog
            });

            // Initialize components
            InitializeDataGridView();
            InitializeListView();
            SetupEventHandlers();
        }

        private void InitializeDataGridView()
        {
            dgvResults.Columns.Clear();
            dgvResults.Columns.Add("Directory", "Directory Path");
            dgvResults.Columns.Add("LargestFile", "Largest File");
            dgvResults.Columns.Add("Size", "Size");
            dgvResults.Columns.Add("Status", "Status");

            dgvResults.Columns["Directory"].Width = 300;
            dgvResults.Columns["LargestFile"].Width = 250;
            dgvResults.Columns["Size"].Width = 100;
            dgvResults.Columns["Status"].Width = 100;
        }

        private void InitializeListView()
        {
            lvWorkers.Columns.Clear();
            lvWorkers.Columns.Add("Directory", 150);
            lvWorkers.Columns.Add("Status", 100);
        }

        private void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnSelectDir1.Click += (s, e) => SelectDirectory(txtDir1);
            btnSelectDir2.Click += (s, e) => SelectDirectory(txtDir2);
            this.FormClosing += MainForm_FormClosing;
        }

        private void SelectDirectory(TextBox textBox)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select directory to scan";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        //private void BtnStart_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtDir1.Text) || string.IsNullOrEmpty(txtDir2.Text))
        //    {
        //        MessageBox.Show("Please select both directories before starting.", "Warning",
        //                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    Log("=== Starting Directory Scanning System ===");

        //    // Очищаем предыдущие результаты
        //    dgvResults.Rows.Clear();
        //    lvWorkers.Items.Clear();
        //    completedScanners = 0;

        //    // Создаем события синхронизации для двух директорий
        //    completionEvents = new ManualResetEvent[2];
        //    completionEvents[0] = new ManualResetEvent(false);
        //    completionEvents[1] = new ManualResetEvent(false);

        //    // Добавляем строки в таблицу для обеих директорий
        //    int row1 = dgvResults.Rows.Add();
        //    dgvResults.Rows[row1].Cells["Directory"].Value = txtDir1.Text;
        //    dgvResults.Rows[row1].Cells["Status"].Value = "Scanning...";
        //    dgvResults.Rows[row1].Tag = 0;

        //    int row2 = dgvResults.Rows.Add();
        //    dgvResults.Rows[row2].Cells["Directory"].Value = txtDir2.Text;
        //    dgvResults.Rows[row2].Cells["Status"].Value = "Scanning...";
        //    dgvResults.Rows[row2].Tag = 1;

        //    // Добавляем в список рабочих потоков
        //    lvWorkers.Items.Add(new ListViewItem(new[] { txtDir1.Text, "Scanning..." }));
        //    lvWorkers.Items.Add(new ListViewItem(new[] { txtDir2.Text, "Scanning..." }));

        //    btnStart.Enabled = false;

        //    // ЗАПУСКАЕМ ОБЕ ДИРЕКТОРИИ В ОТДЕЛЬНЫХ ПОТОКАХ
        //    ThreadPool.QueueUserWorkItem(_ => ScanDirectory(txtDir1.Text, 0));
        //    ThreadPool.QueueUserWorkItem(_ => ScanDirectory(txtDir2.Text, 1));

        //    // Запускаем поток, который будет ждать завершения обоих сканирований
        //    ThreadPool.QueueUserWorkItem(_ => WaitForAllCompletions());
        //}
        /// Сканирование одной директории в отдельном потоке
        /// 
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDir1.Text) || string.IsNullOrEmpty(txtDir2.Text))
            {
                MessageBox.Show("Please select both directories before starting.", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log("=== Starting Directory Scanning System ===");

            // Очищаем предыдущие результаты
            dgvResults.Rows.Clear();
            lvWorkers.Items.Clear();
            completedScanners = 0;

            // Создаем события синхронизации для двух директорий
            completionEvents = new ManualResetEvent[2];
            completionEvents[0] = new ManualResetEvent(false);
            completionEvents[1] = new ManualResetEvent(false);

            // Добавляем строки в таблицу
            int row1 = dgvResults.Rows.Add();
            dgvResults.Rows[row1].Cells["Directory"].Value = txtDir1.Text;
            dgvResults.Rows[row1].Cells["Status"].Value = "Scanning...";
            dgvResults.Rows[row1].Tag = 0;

            int row2 = dgvResults.Rows.Add();
            dgvResults.Rows[row2].Cells["Directory"].Value = txtDir2.Text;
            dgvResults.Rows[row2].Cells["Status"].Value = "Scanning...";
            dgvResults.Rows[row2].Tag = 1;

            // Добавляем в список рабочих потоков
            lvWorkers.Items.Add(new ListViewItem(new[] { txtDir1.Text, "Scanning..." }));
            lvWorkers.Items.Add(new ListViewItem(new[] { txtDir2.Text, "Scanning..." }));

            btnStart.Enabled = false;

            // СОЗДАЕМ  ПОТОКИ 
            Thread thread1 = new Thread(() => ScanDirectory(txtDir1.Text, 0));
            Thread thread2 = new Thread(() => ScanDirectory(txtDir2.Text, 1));
            Thread waitThread = new Thread(WaitForAllCompletions);

            //  await Task.WhenAll(task1, task2);
            // Запускаем потоки
            thread1.Start();
            thread2.Start();
            waitThread.Start();
        }
        private void ScanDirectory(string directoryPath, int scannerIndex)
        {
            try
            {
                Log($"Starting scanner #{scannerIndex} for directory: {directoryPath}");

                // Используем WinAPIFileFinder для поиска самого большого файла
                var fileFinder = new WinAPIFileFinder(directoryPath);
                var largestFile = fileFinder.FindLargestFile(); // Теперь этот метод существует!

                // Обновляем UI с результатами
                InvokeIfRequired(() =>
                {
                    if (largestFile != null)
                    {
                        // Находим строку для этой директории
                        for (int i = 0; i < dgvResults.Rows.Count; i++)
                        {
                            if (dgvResults.Rows[i].Tag?.ToString() == scannerIndex.ToString())
                            {
                                dgvResults.Rows[i].Cells["LargestFile"].Value = largestFile.Name;
                                dgvResults.Rows[i].Cells["Size"].Value = FormatFileSize(largestFile.Size);
                                dgvResults.Rows[i].Cells["Status"].Value = "Completed";
                                break;
                            }
                        }

                        // Обновляем список рабочих
                        if (scannerIndex < lvWorkers.Items.Count)
                        {
                            lvWorkers.Items[scannerIndex].SubItems[1].Text = "Completed";
                        }

                        Log($"Scanner #{scannerIndex} completed: {largestFile.Name} ({FormatFileSize(largestFile.Size)})");
                    }
                    else
                    {
                        // Обновляем статус если файлов не найдено
                        for (int i = 0; i < dgvResults.Rows.Count; i++)
                        {
                            if (dgvResults.Rows[i].Tag?.ToString() == scannerIndex.ToString())
                            {
                                dgvResults.Rows[i].Cells["LargestFile"].Value = "No files found";
                                dgvResults.Rows[i].Cells["Size"].Value = "";
                                dgvResults.Rows[i].Cells["Status"].Value = "Completed";
                                break;
                            }
                        }

                        if (scannerIndex < lvWorkers.Items.Count)
                        {
                            lvWorkers.Items[scannerIndex].SubItems[1].Text = "Completed";
                        }

                        Log($"Scanner #{scannerIndex} completed: No files found in directory");
                    }
                });
            }
            catch (Exception ex)
            {
                Log($"Scanner #{scannerIndex} error: {ex.Message}");

                InvokeIfRequired(() =>
                {
                    for (int i = 0; i < dgvResults.Rows.Count; i++)
                    {
                        if (dgvResults.Rows[i].Tag?.ToString() == scannerIndex.ToString())
                        {
                            dgvResults.Rows[i].Cells["LargestFile"].Value = "Error";
                            dgvResults.Rows[i].Cells["Size"].Value = "";
                            dgvResults.Rows[i].Cells["Status"].Value = "Error";
                            break;
                        }
                    }

                    if (scannerIndex < lvWorkers.Items.Count)
                    {
                        lvWorkers.Items[scannerIndex].SubItems[1].Text = "Error";
                    }
                });
            }
            finally
            {
                // Сигнализируем о завершении этого сканера
                completionEvents[scannerIndex].Set();
            }
        }

        /// Ожидание завершения всех сканирований
        private void WaitForAllCompletions()
        {
            // Ждем завершения обоих потоков
            WaitHandle.WaitAll(completionEvents);

            // Все сканирования завершены
            InvokeIfRequired(() =>
            {
                Log("=== All scanning completed! ===");
                btnStart.Enabled = true;

                // Показываем финальные результаты
                ShowFinalResults();
            });
        }

        private void ShowFinalResults()
        {
            Log("=== FINAL RESULTS ===");
            for (int i = 0; i < dgvResults.Rows.Count; i++)
            {
                var row = dgvResults.Rows[i];
                Log($"Directory: {row.Cells["Directory"].Value}");
                Log($"  Largest file: {row.Cells["LargestFile"].Value}");
                Log($"  Size: {row.Cells["Size"].Value}");
                Log("");
            }
            Log("=== END RESULTS ===");
        }

        private void Log(string text)
        {
            string line = $"[{DateTime.Now:HH:mm:ss.fff}] {text}";
            InvokeIfRequired(() =>
            {
                rtbLog.AppendText(line + Environment.NewLine);
                rtbLog.ScrollToCaret();
            });
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Очистка ресурсов
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else
                action();
        }
    }
}