namespace LogAudit
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.msgDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgPriorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgHostnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.msgDateDataGridViewTextBoxColumn,
            this.msgTimeDataGridViewTextBoxColumn,
            this.msgPriorityDataGridViewTextBoxColumn,
            this.msgHostnameDataGridViewTextBoxColumn,
            this.msgTextDataGridViewTextBoxColumn});
            this.dataGridView1.Location = new System.Drawing.Point(401, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(817, 254);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // msgDateDataGridViewTextBoxColumn
            // 
            this.msgDateDataGridViewTextBoxColumn.DataPropertyName = "MsgDate";
            this.msgDateDataGridViewTextBoxColumn.HeaderText = "MsgDate";
            this.msgDateDataGridViewTextBoxColumn.Name = "msgDateDataGridViewTextBoxColumn";
            this.msgDateDataGridViewTextBoxColumn.Width = 79;
            // 
            // msgTimeDataGridViewTextBoxColumn
            // 
            this.msgTimeDataGridViewTextBoxColumn.DataPropertyName = "MsgTime";
            this.msgTimeDataGridViewTextBoxColumn.HeaderText = "MsgTime";
            this.msgTimeDataGridViewTextBoxColumn.Name = "msgTimeDataGridViewTextBoxColumn";
            this.msgTimeDataGridViewTextBoxColumn.Width = 81;
            // 
            // msgPriorityDataGridViewTextBoxColumn
            // 
            this.msgPriorityDataGridViewTextBoxColumn.DataPropertyName = "MsgPriority";
            this.msgPriorityDataGridViewTextBoxColumn.HeaderText = "MsgPriority";
            this.msgPriorityDataGridViewTextBoxColumn.Name = "msgPriorityDataGridViewTextBoxColumn";
            this.msgPriorityDataGridViewTextBoxColumn.Width = 93;
            // 
            // msgHostnameDataGridViewTextBoxColumn
            // 
            this.msgHostnameDataGridViewTextBoxColumn.DataPropertyName = "MsgHostname";
            this.msgHostnameDataGridViewTextBoxColumn.HeaderText = "MsgHostname";
            this.msgHostnameDataGridViewTextBoxColumn.Name = "msgHostnameDataGridViewTextBoxColumn";
            this.msgHostnameDataGridViewTextBoxColumn.Width = 110;
            // 
            // msgTextDataGridViewTextBoxColumn
            // 
            this.msgTextDataGridViewTextBoxColumn.DataPropertyName = "MsgText";
            this.msgTextDataGridViewTextBoxColumn.HeaderText = "MsgText";
            this.msgTextDataGridViewTextBoxColumn.Name = "msgTextDataGridViewTextBoxColumn";
            this.msgTextDataGridViewTextBoxColumn.Width = 76;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(213, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "Buscar Logs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 38);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(213, 23);
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.Value = new System.DateTime(2022, 3, 4, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fecha actual o semana a auditar:";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "3 Intentos fallidos de Ingreso",
            "Auditoria \"Usuario Bloqueado\"",
            "Ingreso Fuera de Horario por un usuario",
            "Auditorias Completas"});
            this.checkedListBox1.Location = new System.Drawing.Point(12, 155);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(213, 76);
            this.checkedListBox1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(213, 38);
            this.button2.TabIndex = 6;
            this.button2.Text = "Ejecutar Auditorias";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar1.Location = new System.Drawing.Point(401, 283);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(817, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1230, 351);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private DateTimePicker dateTimePicker1;
        private Label label1;
        private DataGridViewTextBoxColumn msgDateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn msgTimeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn msgPriorityDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn msgHostnameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn msgTextDataGridViewTextBoxColumn;
        private CheckedListBox checkedListBox1;
        private Button button2;
        private ProgressBar progressBar1;
    }
}







