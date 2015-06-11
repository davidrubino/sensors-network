namespace AppliClient
{
    partial class CreateRuleForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_validate = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox_actuator = new System.Windows.Forms.ComboBox();
            this.label_sensors = new System.Windows.Forms.Label();
            this.label_actuator = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.checkedListBox_sensors = new System.Windows.Forms.CheckedListBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(180, 260);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Annuler";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_validate
            // 
            this.button_validate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_validate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_validate.Location = new System.Drawing.Point(96, 260);
            this.button_validate.Name = "button_validate";
            this.button_validate.Size = new System.Drawing.Size(78, 23);
            this.button_validate.TabIndex = 1;
            this.button_validate.Text = "Valider";
            this.button_validate.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.comboBox_actuator, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_sensors, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_actuator, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_name, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox_sensors, 1, 2);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(326, 242);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox_actuator
            // 
            this.comboBox_actuator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_actuator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_actuator.FormattingEnabled = true;
            this.comboBox_actuator.Location = new System.Drawing.Point(109, 29);
            this.comboBox_actuator.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.comboBox_actuator.Name = "comboBox_actuator";
            this.comboBox_actuator.Size = new System.Drawing.Size(187, 21);
            this.comboBox_actuator.TabIndex = 3;
            this.comboBox_actuator.Validating += new System.ComponentModel.CancelEventHandler(this.comboBox_actuator_Validating);
            // 
            // label_sensors
            // 
            this.label_sensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_sensors.Location = new System.Drawing.Point(3, 53);
            this.label_sensors.Name = "label_sensors";
            this.label_sensors.Size = new System.Drawing.Size(100, 189);
            this.label_sensors.TabIndex = 4;
            this.label_sensors.Text = "Capteurs déclencheurs";
            // 
            // label_actuator
            // 
            this.label_actuator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_actuator.Location = new System.Drawing.Point(3, 26);
            this.label_actuator.Name = "label_actuator";
            this.label_actuator.Size = new System.Drawing.Size(100, 27);
            this.label_actuator.TabIndex = 2;
            this.label_actuator.Text = "Actionneur";
            this.label_actuator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_name
            // 
            this.textBox_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_name.Location = new System.Drawing.Point(109, 3);
            this.textBox_name.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(187, 20);
            this.textBox_name.TabIndex = 1;
            this.textBox_name.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_name_Validating);
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_name.Location = new System.Drawing.Point(3, 0);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(100, 26);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "Nom";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkedListBox_sensors
            // 
            this.checkedListBox_sensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_sensors.FormattingEnabled = true;
            this.checkedListBox_sensors.Location = new System.Drawing.Point(109, 56);
            this.checkedListBox_sensors.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.checkedListBox_sensors.Name = "checkedListBox_sensors";
            this.checkedListBox_sensors.Size = new System.Drawing.Size(187, 183);
            this.checkedListBox_sensors.TabIndex = 5;
            this.checkedListBox_sensors.Validating += new System.ComponentModel.CancelEventHandler(this.checkedListBox_sensors_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CreateRuleForm
            // 
            this.AcceptButton = this.button_validate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(350, 295);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_validate);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "CreateRuleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Créer une règle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateAlertForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_validate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_actuator;
        private System.Windows.Forms.Label label_sensors;
        private System.Windows.Forms.ComboBox comboBox_actuator;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.CheckedListBox checkedListBox_sensors;
    }
}