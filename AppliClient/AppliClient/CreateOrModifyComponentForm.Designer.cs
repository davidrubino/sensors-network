namespace AppliClient
{
    partial class CreateOrModifyComponentForm
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDown_threshold = new System.Windows.Forms.NumericUpDown();
            this.comboBox_threshold_type = new System.Windows.Forms.ComboBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.comboBox_addr = new System.Windows.Forms.ComboBox();
            this.label_address = new System.Windows.Forms.Label();
            this.label_threshold = new System.Windows.Forms.Label();
            this.label_type = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label_boolThreshold = new System.Windows.Forms.Label();
            this.checkBox_active_bool = new System.Windows.Forms.CheckBox();
            this.checkBox_active = new System.Windows.Forms.CheckBox();
            this.comboBox_boolThreshold = new System.Windows.Forms.ComboBox();
            this.button_validate = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_threshold, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_threshold_type, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_name, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_addr, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_address, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_threshold, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_type, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_type, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_boolThreshold, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_active_bool, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_active, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_boolThreshold, 2, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(342, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDown_threshold
            // 
            this.numericUpDown_threshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_threshold.Location = new System.Drawing.Point(239, 83);
            this.numericUpDown_threshold.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.numericUpDown_threshold.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDown_threshold.Name = "numericUpDown_threshold";
            this.numericUpDown_threshold.Size = new System.Drawing.Size(74, 20);
            this.numericUpDown_threshold.TabIndex = 17;
            // 
            // comboBox_threshold_type
            // 
            this.comboBox_threshold_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_threshold_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_threshold_type.FormattingEnabled = true;
            this.comboBox_threshold_type.IntegralHeight = false;
            this.comboBox_threshold_type.Location = new System.Drawing.Point(163, 83);
            this.comboBox_threshold_type.Name = "comboBox_threshold_type";
            this.comboBox_threshold_type.Size = new System.Drawing.Size(70, 21);
            this.comboBox_threshold_type.TabIndex = 16;
            // 
            // textBox_name
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox_name, 3);
            this.textBox_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_name.Location = new System.Drawing.Point(110, 3);
            this.textBox_name.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(203, 20);
            this.textBox_name.TabIndex = 1;
            this.textBox_name.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_name_Validating);
            // 
            // label_name
            // 
            this.label_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_name.Location = new System.Drawing.Point(3, 0);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(101, 26);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "Nom";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox_addr
            // 
            this.comboBox_addr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBox_addr, 3);
            this.comboBox_addr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_addr.FormattingEnabled = true;
            this.comboBox_addr.IntegralHeight = false;
            this.comboBox_addr.Location = new System.Drawing.Point(110, 29);
            this.comboBox_addr.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.comboBox_addr.Name = "comboBox_addr";
            this.comboBox_addr.Size = new System.Drawing.Size(203, 21);
            this.comboBox_addr.TabIndex = 3;
            this.comboBox_addr.TextChanged += new System.EventHandler(this.comboBox_addr_TextChanged);
            this.comboBox_addr.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_address_Validating);
            // 
            // label_address
            // 
            this.label_address.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_address.Location = new System.Drawing.Point(3, 26);
            this.label_address.Name = "label_address";
            this.label_address.Size = new System.Drawing.Size(101, 27);
            this.label_address.TabIndex = 2;
            this.label_address.Text = "Adresse";
            this.label_address.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_threshold
            // 
            this.label_threshold.AutoSize = true;
            this.label_threshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_threshold.Location = new System.Drawing.Point(3, 80);
            this.label_threshold.Name = "label_threshold";
            this.label_threshold.Size = new System.Drawing.Size(101, 27);
            this.label_threshold.TabIndex = 6;
            this.label_threshold.Text = "Valeur seuil";
            this.label_threshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_type
            // 
            this.label_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_type.Location = new System.Drawing.Point(3, 53);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(101, 27);
            this.label_type.TabIndex = 4;
            this.label_type.Text = "Type";
            this.label_type.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox_type
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBox_type, 3);
            this.comboBox_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.IntegralHeight = false;
            this.comboBox_type.Items.AddRange(new object[] {
            "Capteur : Interrupteur",
            "Capteur : Luminosité",
            "Actionneur : LED"});
            this.comboBox_type.Location = new System.Drawing.Point(110, 56);
            this.comboBox_type.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(203, 21);
            this.comboBox_type.TabIndex = 5;
            this.comboBox_type.SelectedIndexChanged += new System.EventHandler(this.comboBox_type_SelectedIndexChanged);
            this.comboBox_type.Validating += new System.ComponentModel.CancelEventHandler(this.comboBox_type_Validating);
            // 
            // label_boolThreshold
            // 
            this.label_boolThreshold.AutoSize = true;
            this.label_boolThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_boolThreshold.Location = new System.Drawing.Point(3, 107);
            this.label_boolThreshold.Name = "label_boolThreshold";
            this.label_boolThreshold.Size = new System.Drawing.Size(101, 27);
            this.label_boolThreshold.TabIndex = 10;
            this.label_boolThreshold.Text = "Etat d\'alerte";
            this.label_boolThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_active_bool
            // 
            this.checkBox_active_bool.Checked = true;
            this.checkBox_active_bool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_active_bool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_active_bool.Location = new System.Drawing.Point(110, 110);
            this.checkBox_active_bool.Name = "checkBox_active_bool";
            this.checkBox_active_bool.Size = new System.Drawing.Size(47, 21);
            this.checkBox_active_bool.TabIndex = 11;
            this.checkBox_active_bool.Text = "Actif";
            this.checkBox_active_bool.UseVisualStyleBackColor = true;
            // 
            // checkBox_active
            // 
            this.checkBox_active.Checked = true;
            this.checkBox_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_active.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_active.Location = new System.Drawing.Point(110, 83);
            this.checkBox_active.Name = "checkBox_active";
            this.checkBox_active.Size = new System.Drawing.Size(47, 21);
            this.checkBox_active.TabIndex = 7;
            this.checkBox_active.Text = "Actif";
            this.checkBox_active.UseVisualStyleBackColor = true;
            // 
            // comboBox_boolThreshold
            // 
            this.comboBox_boolThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_boolThreshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_boolThreshold.FormattingEnabled = true;
            this.comboBox_boolThreshold.Items.AddRange(new object[] {
            "Actif",
            "Inactif"});
            this.comboBox_boolThreshold.Location = new System.Drawing.Point(163, 110);
            this.comboBox_boolThreshold.Name = "comboBox_boolThreshold";
            this.comboBox_boolThreshold.Size = new System.Drawing.Size(70, 21);
            this.comboBox_boolThreshold.TabIndex = 15;
            // 
            // button_validate
            // 
            this.button_validate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_validate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_validate.Location = new System.Drawing.Point(104, 191);
            this.button_validate.Name = "button_validate";
            this.button_validate.Size = new System.Drawing.Size(75, 23);
            this.button_validate.TabIndex = 1;
            this.button_validate.Text = "Valider";
            this.button_validate.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(187, 191);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Annuler";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CreateOrModifyComponentForm
            // 
            this.AcceptButton = this.button_validate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(366, 226);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_validate);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "CreateOrModifyComponentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edition d\'un composant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateSensorForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_validate;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_address;
        private System.Windows.Forms.Label label_type;
        private System.Windows.Forms.Label label_threshold;
        private System.Windows.Forms.Label label_boolThreshold;
        private System.Windows.Forms.ComboBox comboBox_addr;
        private System.Windows.Forms.CheckBox checkBox_active_bool;
        private System.Windows.Forms.CheckBox checkBox_active;
        private System.Windows.Forms.NumericUpDown numericUpDown_threshold;
        private System.Windows.Forms.ComboBox comboBox_threshold_type;
        private System.Windows.Forms.ComboBox comboBox_boolThreshold;
    }
}