namespace HousingManagementSystem.Views
{
    partial class Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlRequests = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStopDemo = new System.Windows.Forms.Button();
            this.btnStartDemo = new System.Windows.Forms.Button();
            this.pnlEmployees = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRequests
            // 
            this.pnlRequests.AutoScroll = true;
            this.pnlRequests.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pnlRequests.Location = new System.Drawing.Point(297, 12);
            this.pnlRequests.Name = "pnlRequests";
            this.pnlRequests.Size = new System.Drawing.Size(1056, 586);
            this.pnlRequests.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnStopDemo);
            this.panel1.Controls.Add(this.btnStartDemo);
            this.panel1.Location = new System.Drawing.Point(12, 617);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1341, 56);
            this.panel1.TabIndex = 5;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(1193, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(145, 50);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStopDemo
            // 
            this.btnStopDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStopDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopDemo.Location = new System.Drawing.Point(639, 3);
            this.btnStopDemo.Name = "btnStopDemo";
            this.btnStopDemo.Size = new System.Drawing.Size(170, 50);
            this.btnStopDemo.TabIndex = 1;
            this.btnStopDemo.Text = "Stop Auto Demo";
            this.btnStopDemo.UseVisualStyleBackColor = false;
            // 
            // btnStartDemo
            // 
            this.btnStartDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStartDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartDemo.Location = new System.Drawing.Point(3, 3);
            this.btnStartDemo.Name = "btnStartDemo";
            this.btnStartDemo.Size = new System.Drawing.Size(193, 50);
            this.btnStartDemo.TabIndex = 0;
            this.btnStartDemo.Text = "Start Auto Demo";
            this.btnStartDemo.UseVisualStyleBackColor = false;
            // 
            // pnlEmployees
            // 
            this.pnlEmployees.AutoScroll = true;
            this.pnlEmployees.BackColor = System.Drawing.Color.PeachPuff;
            this.pnlEmployees.Location = new System.Drawing.Point(12, 12);
            this.pnlEmployees.Name = "pnlEmployees";
            this.pnlEmployees.Size = new System.Drawing.Size(279, 585);
            this.pnlEmployees.TabIndex = 6;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1365, 684);
            this.Controls.Add(this.pnlEmployees);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlRequests);
            this.Name = "Test";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlRequests;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStopDemo;
        private System.Windows.Forms.Button btnStartDemo;
        private System.Windows.Forms.FlowLayoutPanel pnlEmployees;
    }
}