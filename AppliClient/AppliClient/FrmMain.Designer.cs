namespace AppliClient
{
    partial class FrmMain
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
            this.panel_Controllers = new System.Windows.Forms.Panel();
            this.dataGridView_rules = new System.Windows.Forms.DataGridView();
            this.Column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_actuator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_sensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label_type = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_value = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.label_address = new System.Windows.Forms.Label();
            this.label_threshold = new System.Windows.Forms.Label();
            this.label_thresholdText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_lastActivityState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_wirelessSignal = new System.Windows.Forms.Label();
            this.dataGridView_sensors = new System.Windows.Forms.DataGridView();
            this.Column_sensorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_sensor_address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_add = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_eventAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_eventDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_load = new System.Windows.Forms.ToolStripButton();
            this.panel_image = new System.Windows.Forms.Panel();
            this.pictureBox_Map = new System.Windows.Forms.PictureBox();
            this.panel_Controllers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rules)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sensors)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel_image.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Controllers
            // 
            this.panel_Controllers.BackColor = System.Drawing.Color.Transparent;
            this.panel_Controllers.Controls.Add(this.dataGridView_rules);
            this.panel_Controllers.Controls.Add(this.tableLayoutPanel1);
            this.panel_Controllers.Controls.Add(this.dataGridView_sensors);
            this.panel_Controllers.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Controllers.Location = new System.Drawing.Point(611, 0);
            this.panel_Controllers.Name = "panel_Controllers";
            this.panel_Controllers.Size = new System.Drawing.Size(352, 555);
            this.panel_Controllers.TabIndex = 2;
            // 
            // dataGridView_rules
            // 
            this.dataGridView_rules.AllowUserToAddRows = false;
            this.dataGridView_rules.AllowUserToDeleteRows = false;
            this.dataGridView_rules.AllowUserToResizeRows = false;
            this.dataGridView_rules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_name,
            this.Column_actuator,
            this.Column_sensor});
            this.dataGridView_rules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_rules.Location = new System.Drawing.Point(0, 204);
            this.dataGridView_rules.MultiSelect = false;
            this.dataGridView_rules.Name = "dataGridView_rules";
            this.dataGridView_rules.ReadOnly = true;
            this.dataGridView_rules.RowHeadersVisible = false;
            this.dataGridView_rules.RowHeadersWidth = 50;
            this.dataGridView_rules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_rules.Size = new System.Drawing.Size(352, 189);
            this.dataGridView_rules.TabIndex = 12;
            // 
            // Column_name
            // 
            this.Column_name.DataPropertyName = "Name";
            this.Column_name.HeaderText = "Nom";
            this.Column_name.Name = "Column_name";
            this.Column_name.ReadOnly = true;
            // 
            // Column_actuator
            // 
            this.Column_actuator.DataPropertyName = "Actuator";
            this.Column_actuator.HeaderText = "Actionneur";
            this.Column_actuator.Name = "Column_actuator";
            this.Column_actuator.ReadOnly = true;
            // 
            // Column_sensor
            // 
            this.Column_sensor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_sensor.DataPropertyName = "SensorsString";
            this.Column_sensor.HeaderText = "Capteurs";
            this.Column_sensor.Name = "Column_sensor";
            this.Column_sensor.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_type, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_value, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_name, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_address, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_threshold, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label_thresholdText, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label_lastActivityState, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label_wirelessSignal, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 393);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(352, 162);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 18);
            this.label7.TabIndex = 20;
            this.label7.Text = "Adresse";
            // 
            // label_type
            // 
            this.label_type.AutoSize = true;
            this.label_type.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_type.Location = new System.Drawing.Point(134, 39);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(0, 18);
            this.label_type.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Valeur";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Nom";
            // 
            // label_value
            // 
            this.label_value.AutoSize = true;
            this.label_value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_value.Location = new System.Drawing.Point(134, 20);
            this.label_value.Name = "label_value";
            this.label_value.Size = new System.Drawing.Size(0, 18);
            this.label_value.TabIndex = 1;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(134, 1);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(0, 18);
            this.label_name.TabIndex = 11;
            // 
            // label_address
            // 
            this.label_address.AutoSize = true;
            this.label_address.Location = new System.Drawing.Point(134, 58);
            this.label_address.Name = "label_address";
            this.label_address.Size = new System.Drawing.Size(0, 18);
            this.label_address.TabIndex = 21;
            // 
            // label_threshold
            // 
            this.label_threshold.AutoSize = true;
            this.label_threshold.Location = new System.Drawing.Point(134, 77);
            this.label_threshold.Name = "label_threshold";
            this.label_threshold.Size = new System.Drawing.Size(0, 18);
            this.label_threshold.TabIndex = 22;
            // 
            // label_thresholdText
            // 
            this.label_thresholdText.AutoSize = true;
            this.label_thresholdText.Location = new System.Drawing.Point(4, 77);
            this.label_thresholdText.Name = "label_thresholdText";
            this.label_thresholdText.Size = new System.Drawing.Size(116, 18);
            this.label_thresholdText.TabIndex = 23;
            this.label_thresholdText.Text = "Déclenchement";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "Dernière activité";
            // 
            // label_lastActivityState
            // 
            this.label_lastActivityState.AutoSize = true;
            this.label_lastActivityState.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_lastActivityState.Location = new System.Drawing.Point(134, 117);
            this.label_lastActivityState.Name = "label_lastActivityState";
            this.label_lastActivityState.Size = new System.Drawing.Size(0, 18);
            this.label_lastActivityState.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Qualité du signal";
            // 
            // label_wirelessSignal
            // 
            this.label_wirelessSignal.AutoSize = true;
            this.label_wirelessSignal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_wirelessSignal.Location = new System.Drawing.Point(134, 96);
            this.label_wirelessSignal.Name = "label_wirelessSignal";
            this.label_wirelessSignal.Size = new System.Drawing.Size(0, 18);
            this.label_wirelessSignal.TabIndex = 7;
            // 
            // dataGridView_sensors
            // 
            this.dataGridView_sensors.AllowUserToAddRows = false;
            this.dataGridView_sensors.AllowUserToDeleteRows = false;
            this.dataGridView_sensors.AllowUserToResizeRows = false;
            this.dataGridView_sensors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_sensors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_sensorName,
            this.Column_type,
            this.Column_sensor_address});
            this.dataGridView_sensors.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView_sensors.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_sensors.MultiSelect = false;
            this.dataGridView_sensors.Name = "dataGridView_sensors";
            this.dataGridView_sensors.ReadOnly = true;
            this.dataGridView_sensors.RowHeadersVisible = false;
            this.dataGridView_sensors.RowHeadersWidth = 50;
            this.dataGridView_sensors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_sensors.Size = new System.Drawing.Size(352, 204);
            this.dataGridView_sensors.TabIndex = 0;
            this.dataGridView_sensors.SelectionChanged += new System.EventHandler(this.dataGridView_sensors_SelectionChanged);
            // 
            // Column_sensorName
            // 
            this.Column_sensorName.DataPropertyName = "SensorName";
            this.Column_sensorName.HeaderText = "Nom du composant";
            this.Column_sensorName.MinimumWidth = 30;
            this.Column_sensorName.Name = "Column_sensorName";
            this.Column_sensorName.ReadOnly = true;
            this.Column_sensorName.Width = 114;
            // 
            // Column_type
            // 
            this.Column_type.DataPropertyName = "SensorType";
            this.Column_type.HeaderText = "Type";
            this.Column_type.Name = "Column_type";
            this.Column_type.ReadOnly = true;
            this.Column_type.Width = 56;
            // 
            // Column_sensor_address
            // 
            this.Column_sensor_address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_sensor_address.DataPropertyName = "Address";
            this.Column_sensor_address.HeaderText = "Adresse";
            this.Column_sensor_address.Name = "Column_sensor_address";
            this.Column_sensor_address.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_add,
            this.toolStripButton_delete,
            this.toolStripButton_edit,
            this.toolStripSeparator1,
            this.toolStripButton_eventAdd,
            this.toolStripButton_eventDelete,
            this.toolStripSeparator2,
            this.toolStripButton_load});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(611, 38);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_add
            // 
            this.toolStripButton_add.Image = global::AppliClient.Properties.Resources.Add;
            this.toolStripButton_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_add.Name = "toolStripButton_add";
            this.toolStripButton_add.Size = new System.Drawing.Size(113, 35);
            this.toolStripButton_add.Text = "Ajouter composant";
            this.toolStripButton_add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_add.Click += new System.EventHandler(this.toolStripButton_add_Click);
            // 
            // toolStripButton_delete
            // 
            this.toolStripButton_delete.Image = global::AppliClient.Properties.Resources.Remove;
            this.toolStripButton_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete.Name = "toolStripButton_delete";
            this.toolStripButton_delete.Size = new System.Drawing.Size(129, 35);
            this.toolStripButton_delete.Text = "Supprimer composant";
            this.toolStripButton_delete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_delete.Click += new System.EventHandler(this.toolStripButton_delete_Click);
            // 
            // toolStripButton_edit
            // 
            this.toolStripButton_edit.Image = global::AppliClient.Properties.Resources.Edit;
            this.toolStripButton_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_edit.Name = "toolStripButton_edit";
            this.toolStripButton_edit.Size = new System.Drawing.Size(119, 35);
            this.toolStripButton_edit.Text = "Modifier composant";
            this.toolStripButton_edit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_edit.Click += new System.EventHandler(this.toolStripButton_edit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButton_eventAdd
            // 
            this.toolStripButton_eventAdd.Image = global::AppliClient.Properties.Resources.Add;
            this.toolStripButton_eventAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_eventAdd.Name = "toolStripButton_eventAdd";
            this.toolStripButton_eventAdd.Size = new System.Drawing.Size(79, 35);
            this.toolStripButton_eventAdd.Text = "Ajouter règle";
            this.toolStripButton_eventAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_eventAdd.Click += new System.EventHandler(this.toolStripButton_eventAdd_Click);
            // 
            // toolStripButton_eventDelete
            // 
            this.toolStripButton_eventDelete.Image = global::AppliClient.Properties.Resources.Remove;
            this.toolStripButton_eventDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_eventDelete.Name = "toolStripButton_eventDelete";
            this.toolStripButton_eventDelete.Size = new System.Drawing.Size(95, 35);
            this.toolStripButton_eventDelete.Text = "Supprimer règle";
            this.toolStripButton_eventDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_eventDelete.Click += new System.EventHandler(this.toolStripButton_eventDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButton_load
            // 
            this.toolStripButton_load.Image = global::AppliClient.Properties.Resources.Image;
            this.toolStripButton_load.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_load.Name = "toolStripButton_load";
            this.toolStripButton_load.Size = new System.Drawing.Size(96, 35);
            this.toolStripButton_load.Text = "Charger un plan";
            this.toolStripButton_load.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_load.Click += new System.EventHandler(this.toolStripButton_load_Click);
            // 
            // panel_image
            // 
            this.panel_image.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_image.AutoScroll = true;
            this.panel_image.Controls.Add(this.pictureBox_Map);
            this.panel_image.Location = new System.Drawing.Point(12, 41);
            this.panel_image.Name = "panel_image";
            this.panel_image.Size = new System.Drawing.Size(593, 502);
            this.panel_image.TabIndex = 5;
            // 
            // pictureBox_Map
            // 
            this.pictureBox_Map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Map.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Map.Name = "pictureBox_Map";
            this.pictureBox_Map.Size = new System.Drawing.Size(593, 502);
            this.pictureBox_Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_Map.TabIndex = 3;
            this.pictureBox_Map.TabStop = false;
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 555);
            this.Controls.Add(this.panel_image);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_Controllers);
            this.Name = "FrmMain";
            this.Text = "Gestion du réseau";
            this.panel_Controllers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rules)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sensors)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_image.ResumeLayout(false);
            this.panel_image.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_Controllers;
        private System.Windows.Forms.DataGridView dataGridView_sensors;
        private System.Windows.Forms.PictureBox pictureBox_Map;
        private System.Windows.Forms.Label label_value;
        private System.Windows.Forms.Label label_lastActivityState;
        private System.Windows.Forms.Label label_wirelessSignal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_type;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_address;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_threshold;
        private System.Windows.Forms.Label label_thresholdText;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_add;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete;
        private System.Windows.Forms.ToolStripButton toolStripButton_edit;
        private System.Windows.Forms.ToolStripButton toolStripButton_load;
        private System.Windows.Forms.ToolStripButton toolStripButton_eventAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView dataGridView_rules;
        private System.Windows.Forms.ToolStripButton toolStripButton_eventDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_actuator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sensorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sensor_address;
        private System.Windows.Forms.Panel panel_image;
    }
}

