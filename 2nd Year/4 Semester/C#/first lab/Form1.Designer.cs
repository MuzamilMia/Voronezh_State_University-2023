namespace first_lab
{

    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            btnAdd = new Button();
            btnRemove = new Button();
            lstMapItems = new ListBox();
            button1 = new Button();
            button2 = new Button();
            lblCount = new Label();
            lblIsEmpty = new Label();
            lblContainsValue = new Label();
            lblContainsKey = new Label();
            btnCheckValue = new Button();
            btnCheckKey = new Button();
            lstKeys = new ListBox();
            lstValues = new ListBox();
            lblvalue = new Label();
            lblkeys = new Label();
            txtcheckKey = new TextBox();
            txtcheckvalue = new TextBox();
            btnfilter = new Button();
            btnForAll = new Button();
            btnforeach = new Button();
            txtforall = new TextBox();
            cmbKeyType = new ComboBox();
            cmbValueType = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            btnCreateMap = new Button();
            txtValue = new TextBox();
            txtKey = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            cmbMapType = new ComboBox();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            button3 = new Button();
            button4 = new Button();
            chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart2).BeginInit();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 12F);
            btnAdd.Location = new Point(12, 220);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(93, 40);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Font = new Font("Segoe UI", 12F);
            btnRemove.Location = new Point(111, 220);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(92, 40);
            btnRemove.TabIndex = 9;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // lstMapItems
            // 
            lstMapItems.BackColor = SystemColors.ScrollBar;
            lstMapItems.Font = new Font("Segoe UI", 14.25F);
            lstMapItems.FormattingEnabled = true;
            lstMapItems.ItemHeight = 25;
            lstMapItems.Location = new Point(378, 29);
            lstMapItems.Name = "lstMapItems";
            lstMapItems.Size = new Size(248, 329);
            lstMapItems.TabIndex = 10;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F);
            button1.Location = new Point(190, 266);
            button1.Name = "button1";
            button1.Size = new Size(172, 40);
            button1.TabIndex = 11;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F);
            button2.Location = new Point(209, 220);
            button2.Name = "button2";
            button2.Size = new Size(153, 40);
            button2.TabIndex = 12;
            button2.Text = "Immutable Map";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Font = new Font("Segoe UI", 14.25F);
            lblCount.Location = new Point(642, 25);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(21, 25);
            lblCount.TabIndex = 13;
            lblCount.Text = ". ";
            // 
            // lblIsEmpty
            // 
            lblIsEmpty.AutoSize = true;
            lblIsEmpty.Font = new Font("Segoe UI", 14.25F);
            lblIsEmpty.Location = new Point(642, 75);
            lblIsEmpty.Name = "lblIsEmpty";
            lblIsEmpty.Size = new Size(21, 25);
            lblIsEmpty.TabIndex = 14;
            lblIsEmpty.Text = ". ";
            // 
            // lblContainsValue
            // 
            lblContainsValue.AutoSize = true;
            lblContainsValue.Font = new Font("Segoe UI", 14.25F);
            lblContainsValue.Location = new Point(659, 244);
            lblContainsValue.Name = "lblContainsValue";
            lblContainsValue.Size = new Size(0, 25);
            lblContainsValue.TabIndex = 15;
            // 
            // lblContainsKey
            // 
            lblContainsKey.AutoSize = true;
            lblContainsKey.Font = new Font("Segoe UI", 14.25F);
            lblContainsKey.Location = new Point(669, 163);
            lblContainsKey.Name = "lblContainsKey";
            lblContainsKey.Size = new Size(0, 25);
            lblContainsKey.TabIndex = 16;
            // 
            // btnCheckValue
            // 
            btnCheckValue.Font = new Font("Segoe UI", 12F);
            btnCheckValue.Location = new Point(632, 202);
            btnCheckValue.Name = "btnCheckValue";
            btnCheckValue.Size = new Size(108, 29);
            btnCheckValue.TabIndex = 17;
            btnCheckValue.Text = "Check Value";
            btnCheckValue.UseVisualStyleBackColor = true;
            btnCheckValue.Click += btnCheckValue_Click;
            // 
            // btnCheckKey
            // 
            btnCheckKey.Font = new Font("Segoe UI", 12F);
            btnCheckKey.Location = new Point(632, 124);
            btnCheckKey.Name = "btnCheckKey";
            btnCheckKey.Size = new Size(108, 29);
            btnCheckKey.TabIndex = 18;
            btnCheckKey.Text = "Check Key";
            btnCheckKey.UseVisualStyleBackColor = true;
            btnCheckKey.Click += btnCheckKey_Click;
            // 
            // lstKeys
            // 
            lstKeys.BackColor = SystemColors.ScrollBar;
            lstKeys.Font = new Font("Segoe UI", 14.25F);
            lstKeys.FormattingEnabled = true;
            lstKeys.ItemHeight = 25;
            lstKeys.Location = new Point(1021, 59);
            lstKeys.Name = "lstKeys";
            lstKeys.Size = new Size(188, 304);
            lstKeys.TabIndex = 19;
            // 
            // lstValues
            // 
            lstValues.BackColor = SystemColors.ScrollBar;
            lstValues.Font = new Font("Segoe UI", 14.25F);
            lstValues.FormattingEnabled = true;
            lstValues.ItemHeight = 25;
            lstValues.Location = new Point(902, 59);
            lstValues.Name = "lstValues";
            lstValues.Size = new Size(109, 304);
            lstValues.TabIndex = 20;
            // 
            // lblvalue
            // 
            lblvalue.AutoSize = true;
            lblvalue.Font = new Font("Segoe UI", 14.25F);
            lblvalue.Location = new Point(915, 21);
            lblvalue.Name = "lblvalue";
            lblvalue.Size = new Size(67, 25);
            lblvalue.TabIndex = 21;
            lblvalue.Text = "Values";
            // 
            // lblkeys
            // 
            lblkeys.AutoSize = true;
            lblkeys.Font = new Font("Segoe UI", 14.25F);
            lblkeys.Location = new Point(1069, 21);
            lblkeys.Name = "lblkeys";
            lblkeys.Size = new Size(50, 25);
            lblkeys.TabIndex = 22;
            lblkeys.Text = "Keys";
            // 
            // txtcheckKey
            // 
            txtcheckKey.Font = new Font("Times New Roman", 14.25F);
            txtcheckKey.Location = new Point(746, 124);
            txtcheckKey.Name = "txtcheckKey";
            txtcheckKey.Size = new Size(150, 29);
            txtcheckKey.TabIndex = 23;
            // 
            // txtcheckvalue
            // 
            txtcheckvalue.Font = new Font("Times New Roman", 14.25F);
            txtcheckvalue.Location = new Point(746, 202);
            txtcheckvalue.Name = "txtcheckvalue";
            txtcheckvalue.Size = new Size(150, 29);
            txtcheckvalue.TabIndex = 24;
            // 
            // btnfilter
            // 
            btnfilter.Font = new Font("Segoe UI", 12F);
            btnfilter.Location = new Point(12, 266);
            btnfilter.Name = "btnfilter";
            btnfilter.Size = new Size(176, 40);
            btnfilter.TabIndex = 25;
            btnfilter.Text = "Check For All";
            btnfilter.UseVisualStyleBackColor = true;
            btnfilter.Click += btnfilter_Click;
            // 
            // btnForAll
            // 
            btnForAll.Font = new Font("Segoe UI", 12F);
            btnForAll.Location = new Point(632, 324);
            btnForAll.Name = "btnForAll";
            btnForAll.Size = new Size(93, 29);
            btnForAll.TabIndex = 26;
            btnForAll.Text = "ForAll";
            btnForAll.UseVisualStyleBackColor = true;
            btnForAll.Click += btnForAll_Click;
            // 
            // btnforeach
            // 
            btnforeach.Font = new Font("Segoe UI", 12F);
            btnforeach.Location = new Point(632, 270);
            btnforeach.Name = "btnforeach";
            btnforeach.Size = new Size(264, 40);
            btnforeach.TabIndex = 27;
            btnforeach.Text = "For Each";
            btnforeach.UseVisualStyleBackColor = true;
            btnforeach.Click += btnforeach_Click;
            // 
            // txtforall
            // 
            txtforall.Font = new Font("Segoe UI", 12F);
            txtforall.Location = new Point(731, 324);
            txtforall.Name = "txtforall";
            txtforall.Size = new Size(165, 29);
            txtforall.TabIndex = 28;
            // 
            // cmbKeyType
            // 
            cmbKeyType.Font = new Font("Segoe UI", 12F);
            cmbKeyType.FormattingEnabled = true;
            cmbKeyType.Location = new Point(7, 34);
            cmbKeyType.Name = "cmbKeyType";
            cmbKeyType.Size = new Size(167, 29);
            cmbKeyType.TabIndex = 29;
            // 
            // cmbValueType
            // 
            cmbValueType.Font = new Font("Segoe UI", 12F);
            cmbValueType.FormattingEnabled = true;
            cmbValueType.Location = new Point(180, 34);
            cmbValueType.Name = "cmbValueType";
            cmbValueType.Size = new Size(188, 29);
            cmbValueType.TabIndex = 30;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F);
            label4.Location = new Point(194, 6);
            label4.Name = "label4";
            label4.Size = new Size(160, 25);
            label4.TabIndex = 31;
            label4.Text = "Values Data  Type";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F);
            label5.Location = new Point(20, 6);
            label5.Name = "label5";
            label5.Size = new Size(140, 25);
            label5.TabIndex = 32;
            label5.Text = "Keys Data type ";
            // 
            // btnCreateMap
            // 
            btnCreateMap.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateMap.Location = new Point(128, 105);
            btnCreateMap.Name = "btnCreateMap";
            btnCreateMap.Size = new Size(233, 40);
            btnCreateMap.TabIndex = 39;
            btnCreateMap.Text = "Create Map ";
            btnCreateMap.UseVisualStyleBackColor = true;
            btnCreateMap.Click += btnCreateMap_Click;
            // 
            // txtValue
            // 
            txtValue.Font = new Font("Segoe UI", 12F);
            txtValue.Location = new Point(128, 185);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(233, 29);
            txtValue.TabIndex = 38;
            // 
            // txtKey
            // 
            txtKey.Font = new Font("Segoe UI", 12F);
            txtKey.Location = new Point(128, 151);
            txtKey.Name = "txtKey";
            txtKey.Size = new Size(233, 29);
            txtKey.TabIndex = 37;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(29, 188);
            label3.Name = "label3";
            label3.Size = new Size(48, 21);
            label3.TabIndex = 36;
            label3.Text = "Value";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(29, 158);
            label2.Name = "label2";
            label2.Size = new Size(39, 21);
            label2.TabIndex = 35;
            label2.Text = "Key ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(18, 73);
            label1.Name = "label1";
            label1.Size = new Size(86, 21);
            label1.TabIndex = 34;
            label1.Text = "Select Map";
            // 
            // cmbMapType
            // 
            cmbMapType.Font = new Font("Segoe UI", 12F);
            cmbMapType.FormattingEnabled = true;
            cmbMapType.Items.AddRange(new object[] { "ArrayMap", "HashMap", "LinkedMap" });
            cmbMapType.Location = new Point(128, 70);
            cmbMapType.Name = "cmbMapType";
            cmbMapType.Size = new Size(233, 29);
            cmbMapType.TabIndex = 33;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(12, 373);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(549, 300);
            chart1.TabIndex = 40;
            chart1.Text = "chart1";
            // 
            // button3
            // 
            button3.BackColor = SystemColors.AppWorkspace;
            button3.Font = new Font("Segoe UI", 12F);
            button3.Location = new Point(12, 310);
            button3.Name = "button3";
            button3.Size = new Size(176, 44);
            button3.TabIndex = 41;
            button3.Text = "Values Graph";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ButtonShadow;
            button4.Font = new Font("Segoe UI", 12F);
            button4.Location = new Point(190, 310);
            button4.Name = "button4";
            button4.Size = new Size(171, 44);
            button4.TabIndex = 42;
            button4.Text = "Keys Graph";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            chart2.Legends.Add(legend2);
            chart2.Location = new Point(593, 372);
            chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chart2.Series.Add(series2);
            chart2.Size = new Size(616, 301);
            chart2.TabIndex = 43;
            chart2.Text = "chart2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1231, 719);
            Controls.Add(chart2);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(chart1);
            Controls.Add(btnCreateMap);
            Controls.Add(txtValue);
            Controls.Add(txtKey);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmbMapType);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(cmbValueType);
            Controls.Add(cmbKeyType);
            Controls.Add(txtforall);
            Controls.Add(btnforeach);
            Controls.Add(btnForAll);
            Controls.Add(btnfilter);
            Controls.Add(txtcheckvalue);
            Controls.Add(txtcheckKey);
            Controls.Add(lblkeys);
            Controls.Add(lblvalue);
            Controls.Add(lstValues);
            Controls.Add(lstKeys);
            Controls.Add(btnCheckKey);
            Controls.Add(btnCheckValue);
            Controls.Add(lblContainsKey);
            Controls.Add(lblContainsValue);
            Controls.Add(lblIsEmpty);
            Controls.Add(lblCount);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(lstMapItems);
            Controls.Add(btnRemove);
            Controls.Add(btnAdd);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnAdd;
        private Button btnRemove;
        private ListBox lstMapItems;
        private Button button1;
        private Button button2;
        private Label lblCount;
        private Label lblIsEmpty;
        private Label lblContainsValue;
        private Label lblContainsKey;
        private Button btnCheckValue;
        private Button btnCheckKey;
        private ListBox lstKeys;
        private ListBox lstValues;
        private Label lblvalue;
        private Label lblkeys;
        private TextBox txtcheckKey;
        private TextBox txtcheckvalue;
        private Button btnfilter;
        private Button btnForAll;
        private Button btnforeach;
        private TextBox txtforall;
        private ComboBox cmbKeyType;
        private ComboBox cmbValueType;
        private Label label4;
        private Label label5;
        private Button btnCreateMap;
        private TextBox txtValue;
        private TextBox txtKey;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox cmbMapType;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Button button3;
        private Button button4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
    }
}