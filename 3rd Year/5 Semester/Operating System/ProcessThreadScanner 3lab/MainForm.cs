using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProcessThreadScanner
{
    public partial class MainForm : Form
    {
        // UI Controls
        private DataGridView dgvProcesses;
        private DataGridView dgvThreads;
        private RichTextBox rtbLog;
        private Button btnScan;
        private Button btnRefresh;
        private NumericUpDown nudThreadCount;
        private Label lblThreadCount;
        private ProgressBar progressBar;

        public MainForm()
        {
            InitializeComponent1();
            SetupForm();
        }

        private void InitializeComponent1()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Text = "Process Thread Scanner - OS Lab 3";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Thread count selection
            lblThreadCount = new Label
            {
                Text = "Target Thread Count:",
                Location = new Point(10, 15),
                Size = new Size(120, 20)
            };

            nudThreadCount = new NumericUpDown
            {
                Location = new Point(135, 12),
                Size = new Size(80, 20),
                Minimum = 1,
                Maximum = 1000,
                Value = 5
            };

            // Control buttons
            btnScan = new Button
            {
                Text = "Scan Processes",
                Location = new Point(230, 10),
                Size = new Size(120, 25)
            };

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(360, 10),
                Size = new Size(80, 25)
            };

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(450, 12),
                Size = new Size(200, 25),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            // Data Grid View for processes
            dgvProcesses = new DataGridView
            {
                Location = new Point(10, 50),
                Size = new Size(980, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Data Grid View for threads
            dgvThreads = new DataGridView
            {
                Location = new Point(10, 270),
                Size = new Size(980, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };

            // Log text box
            rtbLog = new RichTextBox
            {
                Location = new Point(10, 490),
                Size = new Size(980, 200),
                ReadOnly = true,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 9)
            };

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                lblThreadCount, nudThreadCount,
                btnScan, btnRefresh, progressBar,
                dgvProcesses, dgvThreads, rtbLog
            });

            // Initialize components
            InitializeDataGridViews();
            SetupEventHandlers();
        }

        private void InitializeDataGridViews()
        {
            // Processes DataGridView
            dgvProcesses.Columns.Clear();
            dgvProcesses.Columns.Add("ProcessID", "Process ID");
            dgvProcesses.Columns.Add("ProcessName", "Process Name");
            dgvProcesses.Columns.Add("ThreadCount", "Thread Count");
            dgvProcesses.Columns.Add("ParentPID", "Parent PID");
            dgvProcesses.Columns.Add("Priority", "Base Priority");

            dgvProcesses.Columns["ProcessID"].Width = 80;
            dgvProcesses.Columns["ProcessName"].Width = 200;
            dgvProcesses.Columns["ThreadCount"].Width = 80;
            dgvProcesses.Columns["ParentPID"].Width = 80;
            dgvProcesses.Columns["Priority"].Width = 80;

            // Threads DataGridView
            dgvThreads.Columns.Clear();
            dgvThreads.Columns.Add("ThreadID", "Thread ID");
            dgvThreads.Columns.Add("ProcessID", "Process ID");
            dgvThreads.Columns.Add("ProcessName", "Process Name");
            dgvThreads.Columns.Add("BasePriority", "Base Priority");
            dgvThreads.Columns.Add("DeltaPriority", "Delta Priority");

            dgvThreads.Columns["ThreadID"].Width = 100;
            dgvThreads.Columns["ProcessID"].Width = 80;
            dgvThreads.Columns["ProcessName"].Width = 200;
            dgvThreads.Columns["BasePriority"].Width = 80;
            dgvThreads.Columns["DeltaPriority"].Width = 80;

            // Handle process selection change - FIXED VERSION
            dgvProcesses.SelectionChanged += (s, e) =>
            {
                if (dgvProcesses.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvProcesses.SelectedRows[0];

                    // NULL CHECKING ADDED HERE
                    if (selectedRow.Cells["ProcessID"].Value != null &&
                        selectedRow.Cells["ProcessName"].Value != null)
                    {
                        uint processId = Convert.ToUInt32(selectedRow.Cells["ProcessID"].Value);
                        string processName = selectedRow.Cells["ProcessName"].Value.ToString();
                        ShowThreadsForProcess(processId, processName);
                    }
                }
            };
        }

        private void SetupEventHandlers()
        {
            btnScan.Click += BtnScan_Click;
            btnRefresh.Click += BtnRefresh_Click;
            this.FormClosing += MainForm_FormClosing;
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            uint targetThreadCount = (uint)nudThreadCount.Value;
            ScanProcessesWithThreadCount(targetThreadCount);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            uint targetThreadCount = (uint)nudThreadCount.Value;
            ScanProcessesWithThreadCount(targetThreadCount);
        }

        private void ScanProcessesWithThreadCount(uint targetThreadCount)
        {
            Log($"=== Scanning for processes with {targetThreadCount} threads ===");

            // Clear previous results
            dgvProcesses.Rows.Clear();
            dgvThreads.Rows.Clear();

            // Show progress
            progressBar.Visible = true;
            btnScan.Enabled = false;
            btnRefresh.Enabled = false;

            // Use BackgroundWorker or Thread to avoid UI freezing
            var scanThread = new System.Threading.Thread(() =>
            {
                try
                {
                    var processes = ProcessThreadScanner.GetProcessesWithThreadCount(targetThreadCount);

                    InvokeIfRequired(() =>
                    {
                        foreach (var process in processes)
                        {
                            int rowIndex = dgvProcesses.Rows.Add();
                            dgvProcesses.Rows[rowIndex].Cells["ProcessID"].Value = process.ProcessID;
                            dgvProcesses.Rows[rowIndex].Cells["ProcessName"].Value = process.ProcessName;
                            dgvProcesses.Rows[rowIndex].Cells["ThreadCount"].Value = process.ThreadCount;
                            dgvProcesses.Rows[rowIndex].Cells["ParentPID"].Value = process.ParentPID;
                            dgvProcesses.Rows[rowIndex].Cells["Priority"].Value = process.Priority;
                        }

                        Log($"Found {processes.Count} processes with {targetThreadCount} threads");

                        // Select first process if any - WITH NULL CHECK
                        if (dgvProcesses.Rows.Count > 0 && dgvProcesses.Rows[0].Cells["ProcessID"].Value != null)
                        {
                            dgvProcesses.Rows[0].Selected = true;
                        }
                        else
                        {
                            Log("No processes found with the specified thread count");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log($"Error during scanning: {ex.Message}");
                }
                finally
                {
                    InvokeIfRequired(() =>
                    {
                        progressBar.Visible = false;
                        btnScan.Enabled = true;
                        btnRefresh.Enabled = true;
                        Log("=== Scan completed ===");
                    });
                }
            });

            scanThread.IsBackground = true;
            scanThread.Start();
        }

        private void ShowThreadsForProcess(uint processId, string processName)
        {
            Log($"Loading threads for process: {processName} (PID: {processId})");

            // Clear previous threads
            dgvThreads.Rows.Clear();

            var threadScanner = new System.Threading.Thread(() =>
            {
                try
                {
                    var threads = ProcessThreadScanner.GetThreadsForProcess(processId);

                    InvokeIfRequired(() =>
                    {
                        foreach (var thread in threads)
                        {
                            int rowIndex = dgvThreads.Rows.Add();
                            dgvThreads.Rows[rowIndex].Cells["ThreadID"].Value = thread.ThreadID;
                            dgvThreads.Rows[rowIndex].Cells["ProcessID"].Value = thread.ProcessID;
                            dgvThreads.Rows[rowIndex].Cells["ProcessName"].Value = processName;
                            dgvThreads.Rows[rowIndex].Cells["BasePriority"].Value = thread.BasePriority;
                            dgvThreads.Rows[rowIndex].Cells["DeltaPriority"].Value = thread.DeltaPriority;
                        }

                        Log($"Loaded {threads.Count} threads for process {processName}");
                    });
                }
                catch (Exception ex)
                {
                    Log($"Error loading threads: {ex.Message}");
                }
            });

            threadScanner.IsBackground = true;
            threadScanner.Start();
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup if needed
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