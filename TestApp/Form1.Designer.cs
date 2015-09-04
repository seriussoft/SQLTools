namespace TestApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.colNamesBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabNamesBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.whereClauseBox = new System.Windows.Forms.TextBox();
            this.resultSetBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgview1 = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.msSqlClass1 = new nTools.SqlTools.MsSqlClass();
            this.rECIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lOCIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uSRIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aYEARDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sEMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLSTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgview1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.msSqlClass1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(140, 327);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "GO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // colNamesBox
            // 
            this.colNamesBox.Location = new System.Drawing.Point(12, 49);
            this.colNamesBox.Multiline = true;
            this.colNamesBox.Name = "colNamesBox";
            this.colNamesBox.Size = new System.Drawing.Size(203, 72);
            this.colNamesBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SELECT ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "FROM ";
            // 
            // tabNamesBox
            // 
            this.tabNamesBox.Location = new System.Drawing.Point(12, 144);
            this.tabNamesBox.Multiline = true;
            this.tabNamesBox.Name = "tabNamesBox";
            this.tabNamesBox.Size = new System.Drawing.Size(203, 72);
            this.tabNamesBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "WHERE ";
            // 
            // whereClauseBox
            // 
            this.whereClauseBox.Location = new System.Drawing.Point(12, 239);
            this.whereClauseBox.Multiline = true;
            this.whereClauseBox.Name = "whereClauseBox";
            this.whereClauseBox.Size = new System.Drawing.Size(203, 72);
            this.whereClauseBox.TabIndex = 6;
            // 
            // resultSetBox
            // 
            this.resultSetBox.Location = new System.Drawing.Point(312, 49);
            this.resultSetBox.Multiline = true;
            this.resultSetBox.Name = "resultSetBox";
            this.resultSetBox.Size = new System.Drawing.Size(235, 262);
            this.resultSetBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Result Set ";
            // 
            // dgview1
            // 
            this.dgview1.AutoGenerateColumns = false;
            this.dgview1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgview1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rECIDDataGridViewTextBoxColumn,
            this.cIDDataGridViewTextBoxColumn,
            this.lOCIDDataGridViewTextBoxColumn,
            this.uSRIDDataGridViewTextBoxColumn,
            this.aYEARDataGridViewTextBoxColumn,
            this.sEMDataGridViewTextBoxColumn,
            this.cLSTIMEDataGridViewTextBoxColumn,
            this.sECDataGridViewTextBoxColumn});
            this.dgview1.DataSource = this.msSqlClass1;
            this.dgview1.Location = new System.Drawing.Point(312, 49);
            this.dgview1.Name = "dgview1";
            this.dgview1.Size = new System.Drawing.Size(385, 262);
            this.dgview1.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Table Schema",
            "Result Set"});
            this.comboBox1.Location = new System.Drawing.Point(12, 327);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 10;
            // 
            // msSqlClass1
            // 
            this.msSqlClass1.ConnString = new string[] {
        "sql01",
        "itdedbusr",
        "santac1aus3",
        "itdedev",
        ""};
            this.msSqlClass1.Position = 0;
            this.msSqlClass1.QueryString = "SELECT * FROM BA_ASSIGNMENT";
            // 
            // rECIDDataGridViewTextBoxColumn
            // 
            this.rECIDDataGridViewTextBoxColumn.DataPropertyName = "RECID";
            this.rECIDDataGridViewTextBoxColumn.HeaderText = "RECID";
            this.rECIDDataGridViewTextBoxColumn.Name = "rECIDDataGridViewTextBoxColumn";
            // 
            // cIDDataGridViewTextBoxColumn
            // 
            this.cIDDataGridViewTextBoxColumn.DataPropertyName = "CID";
            this.cIDDataGridViewTextBoxColumn.HeaderText = "CID";
            this.cIDDataGridViewTextBoxColumn.Name = "cIDDataGridViewTextBoxColumn";
            // 
            // lOCIDDataGridViewTextBoxColumn
            // 
            this.lOCIDDataGridViewTextBoxColumn.DataPropertyName = "LOCID";
            this.lOCIDDataGridViewTextBoxColumn.HeaderText = "LOCID";
            this.lOCIDDataGridViewTextBoxColumn.Name = "lOCIDDataGridViewTextBoxColumn";
            // 
            // uSRIDDataGridViewTextBoxColumn
            // 
            this.uSRIDDataGridViewTextBoxColumn.DataPropertyName = "USRID";
            this.uSRIDDataGridViewTextBoxColumn.HeaderText = "USRID";
            this.uSRIDDataGridViewTextBoxColumn.Name = "uSRIDDataGridViewTextBoxColumn";
            // 
            // aYEARDataGridViewTextBoxColumn
            // 
            this.aYEARDataGridViewTextBoxColumn.DataPropertyName = "AYEAR";
            this.aYEARDataGridViewTextBoxColumn.HeaderText = "AYEAR";
            this.aYEARDataGridViewTextBoxColumn.Name = "aYEARDataGridViewTextBoxColumn";
            // 
            // sEMDataGridViewTextBoxColumn
            // 
            this.sEMDataGridViewTextBoxColumn.DataPropertyName = "SEM";
            this.sEMDataGridViewTextBoxColumn.HeaderText = "SEM";
            this.sEMDataGridViewTextBoxColumn.Name = "sEMDataGridViewTextBoxColumn";
            // 
            // cLSTIMEDataGridViewTextBoxColumn
            // 
            this.cLSTIMEDataGridViewTextBoxColumn.DataPropertyName = "CLSTIME";
            this.cLSTIMEDataGridViewTextBoxColumn.HeaderText = "CLSTIME";
            this.cLSTIMEDataGridViewTextBoxColumn.Name = "cLSTIMEDataGridViewTextBoxColumn";
            // 
            // sECDataGridViewTextBoxColumn
            // 
            this.sECDataGridViewTextBoxColumn.DataPropertyName = "SEC";
            this.sECDataGridViewTextBoxColumn.HeaderText = "SEC";
            this.sECDataGridViewTextBoxColumn.Name = "sECDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 374);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dgview1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resultSetBox);
            this.Controls.Add(this.whereClauseBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabNamesBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colNamesBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgview1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.msSqlClass1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox colNamesBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tabNamesBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox whereClauseBox;
        private System.Windows.Forms.TextBox resultSetBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgview1;
        private System.Windows.Forms.ComboBox comboBox1;
        private nTools.SqlTools.MsSqlClass msSqlClass1;
        private System.Windows.Forms.DataGridViewTextBoxColumn rECIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOCIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uSRIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aYEARDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sEMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLSTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sECDataGridViewTextBoxColumn;
    }
}

