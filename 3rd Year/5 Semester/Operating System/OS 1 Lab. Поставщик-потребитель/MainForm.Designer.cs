partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView dgvBuffers;
    private System.Windows.Forms.ListView lvWorkers;
    private System.Windows.Forms.RichTextBox rtbLog;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnResume;
    private System.Windows.Forms.NumericUpDown nudDataLimit;
    private System.Windows.Forms.Label lblDataLimit;
    private System.Windows.Forms.Timer readerCreationTimer;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.dgvBuffers = new System.Windows.Forms.DataGridView();
            this.lvWorkers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.nudDataLimit = new System.Windows.Forms.NumericUpDown();
            this.lblDataLimit = new System.Windows.Forms.Label();
            this.readerCreationTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuffers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBuffers
            // 
            this.dgvBuffers.AllowUserToAddRows = false;
            this.dgvBuffers.AllowUserToDeleteRows = false;
            this.dgvBuffers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvBuffers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuffers.Location = new System.Drawing.Point(10, 10);
            this.dgvBuffers.Name = "dgvBuffers";
            this.dgvBuffers.ReadOnly = true;
            this.dgvBuffers.RowHeadersVisible = false;
            this.dgvBuffers.Size = new System.Drawing.Size(531, 217);
            this.dgvBuffers.TabIndex = 0;
            // 
            // lvWorkers
            // 
            this.lvWorkers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lvWorkers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvWorkers.FullRowSelect = true;
            this.lvWorkers.GridLines = true;
            this.lvWorkers.HideSelection = false;
            this.lvWorkers.Location = new System.Drawing.Point(547, 10);
            this.lvWorkers.Name = "lvWorkers";
            this.lvWorkers.Size = new System.Drawing.Size(315, 217);
            this.lvWorkers.TabIndex = 1;
            this.lvWorkers.UseCompatibleStateImageBehavior = false;
            this.lvWorkers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Role";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Status";
            this.columnHeader3.Width = 150;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.rtbLog.Location = new System.Drawing.Point(13, 233);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(849, 261);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(10, 497);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(159, 40);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnPause.Enabled = false;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Location = new System.Drawing.Point(175, 498);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(158, 39);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = false;
            // 
            // btnResume
            // 
            this.btnResume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnResume.Enabled = false;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResume.Location = new System.Drawing.Point(339, 496);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(194, 40);
            this.btnResume.TabIndex = 5;
            this.btnResume.Text = "Возобновить/Resume";
            this.btnResume.UseVisualStyleBackColor = false;
            // 
            // nudDataLimit
            // 
            this.nudDataLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDataLimit.Location = new System.Drawing.Point(237, 549);
            this.nudDataLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDataLimit.Name = "nudDataLimit";
            this.nudDataLimit.Size = new System.Drawing.Size(51, 26);
            this.nudDataLimit.TabIndex = 6;
            this.nudDataLimit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblDataLimit
            // 
            this.lblDataLimit.AutoSize = true;
            this.lblDataLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataLimit.Location = new System.Drawing.Point(9, 551);
            this.lblDataLimit.Name = "lblDataLimit";
            this.lblDataLimit.Size = new System.Drawing.Size(222, 20);
            this.lblDataLimit.TabIndex = 7;
            this.lblDataLimit.Text = "Лимит данных на читателя:";
            // 
            // readerCreationTimer
            // 
            this.readerCreationTimer.Interval = 3000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 580);
            this.Controls.Add(this.lblDataLimit);
            this.Controls.Add(this.nudDataLimit);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.lvWorkers);
            this.Controls.Add(this.dgvBuffers);
            this.Name = "MainForm";
            this.Text = "Producer-Consumer (Queue with Semaphores/Mutex)";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuffers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}