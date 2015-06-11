using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliClient
{
    public partial class CreateOrModifyComponentForm : Form
    {
        private Control[] numericControlsGroup;
        private Control[] booleanControlGroup;
        public Sensor Sensor { get; set; }

        private CreateOrModifyComponentForm()
        {
            InitializeComponent();

            this.numericControlsGroup = new Control[] { label_threshold, comboBox_threshold_type, numericUpDown_threshold, checkBox_active };
            this.booleanControlGroup = new Control[] { label_boolThreshold, comboBox_boolThreshold, checkBox_active_bool };

            comboBox_type.ValueMember = "Key";
            comboBox_type.DisplayMember = "Value";
            comboBox_type.DataSource = Communication.sensorTypeToString.ToList();

            comboBox_threshold_type.ValueMember = "Key";
            comboBox_threshold_type.DisplayMember = "Value";
            comboBox_threshold_type.DataSource = Communication.thresholdTypeString.ToList();

            comboBox_threshold_type.SelectedIndex = 0;
            comboBox_boolThreshold.SelectedIndex = 0;
        }

        public CreateOrModifyComponentForm(List<Sensor> orphanSensors) : this()
        {
            comboBox_addr.DisplayMember = "Address";
            comboBox_addr.DataSource = orphanSensors;
            comboBox_addr.Enabled = true;
            comboBox_type.Enabled = true;
            comboBox_addr.SelectedIndex = Math.Min(0, orphanSensors.Count - 1);

            comboBox_addr_TextChanged(null, null);
            
            this.Sensor = new Sensor();
        }

        public CreateOrModifyComponentForm(Sensor sensor) : this()
        {
            this.Sensor = sensor;
            comboBox_addr.Enabled = false;
            comboBox_type.Enabled = false;

            initFromSensor();
        }

        private void initFromSensor()
        {
            comboBox_type.SelectedValue = Sensor.Type;
            
            // TODO: don't clear this if we select another address after putting a name in the textbox
            textBox_name.Text = Sensor.Name;

            comboBox_addr.Text = Sensor.Address;

            if (!Sensor.IsActuator)
            {
                if (Sensor.DataType == Communication.sensor_datatype.BOOL)
                {
                    comboBox_boolThreshold.SelectedIndex = ((bool)Sensor.Threshold) ? 0 : 1;
                    checkBox_active_bool.Checked = Sensor.ThresholdType != Communication.threshold_type.NONE;
                }
                else
                {
                    comboBox_threshold_type.SelectedValue = Sensor.ThresholdType == Communication.threshold_type.NONE ? Communication.threshold_type.EQ : Sensor.ThresholdType;
                    checkBox_active.Checked = Sensor.ThresholdType != Communication.threshold_type.NONE;
                    numericUpDown_threshold.Value = Decimal.Parse(Sensor.Threshold.ToString());
                }
            }

            comboBox_type.Enabled = false;
            updateType();
        }

        private void setEditMode(bool boolEdit, bool intEdit)
        {
            foreach (Control c in numericControlsGroup) {
                c.Visible = intEdit;
            }

            foreach (Control c in booleanControlGroup) {
                c.Visible = boolEdit;
            }

            tableLayoutPanel1.RowStyles[3].SizeType = intEdit ? SizeType.AutoSize : SizeType.Absolute;
            tableLayoutPanel1.RowStyles[4].SizeType = boolEdit ? SizeType.AutoSize : SizeType.Absolute;
            tableLayoutPanel1.RowStyles[3].Height = 0;
            tableLayoutPanel1.RowStyles[4].Height = 0;
        }

        private void updateType()
        {
            var type = (Communication.sensor_type)comboBox_type.SelectedValue;
            if (comboBox_type.SelectedValue != null && !Communication.IsActuatorType(type))
            {
                var datatype = Communication.GetDataType(type);
                bool isBool = datatype == Communication.sensor_datatype.BOOL;
                setEditMode(isBool, !isBool);
            }
            else
            {
                setEditMode(false, false);
            }
        }

        private void textBox_address_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(comboBox_addr.Text, "^[a-f0-9]{16}$", RegexOptions.IgnoreCase))
            {
                errorProvider.SetError(comboBox_addr, "Format d'adresse invalide. L'adresse doit être constituée de 16 caractères hexadécimaux (0-9 A-F).");
            }
            else
            {
                errorProvider.SetError(comboBox_addr, null);
            }
        }

        private void textBox_name_Validating(object sender, CancelEventArgs e)
        {
            if (textBox_name.Text.Trim().Length == 0)
            {
                errorProvider.SetError(textBox_name, "Veuiller entrer un nom.");
            }
            else
            {
                errorProvider.SetError(textBox_name, null);
            }
        }

        private void comboBox_type_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox_type.SelectedIndex == -1)
            {
                errorProvider.SetError(comboBox_type, "Veuillez sélectionner un type.");
            }
            else
            {
                errorProvider.SetError(comboBox_type, null);
                updateType();
            }
        }

        private void CreateSensorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
                return;

            ValidateChildren();

            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (!String.IsNullOrEmpty(errorProvider.GetError(c)))
                {
                    e.Cancel = true;
                    return;
                }
            }

            Sensor.Name = textBox_name.Text;
            Sensor.Address = comboBox_addr.Text;
            Sensor.Type = (Communication.sensor_type)comboBox_type.SelectedValue;
            Sensor.DataType = Communication.GetDataType(Sensor.Type).Value;

            Communication.threshold_type thresholdType = (Communication.threshold_type)comboBox_threshold_type.SelectedValue;

            if (!Sensor.IsActuator)
            {
                switch (Sensor.DataType)
                {
                    case Communication.sensor_datatype.BOOL:
                        Sensor.ThresholdType = checkBox_active_bool.Checked ? Communication.threshold_type.EQ : Communication.threshold_type.NONE;
                        Sensor.Threshold = comboBox_boolThreshold.SelectedIndex == 0;
                        break;
                    case Communication.sensor_datatype.INT:
                        Sensor.ThresholdType = checkBox_active.Checked ? thresholdType : Communication.threshold_type.NONE;
                        Sensor.Threshold = (int)numericUpDown_threshold.Value;
                        break;
                    case Communication.sensor_datatype.FLOAT:
                        Sensor.ThresholdType = checkBox_active.Checked ? thresholdType : Communication.threshold_type.NONE;
                        Sensor.Threshold = (float)numericUpDown_threshold.Value;
                        break;
                }
            }
            else
            {
                Sensor.ThresholdType = Communication.threshold_type.NONE;
                Sensor.Threshold = null;
            }
        }

        private void comboBox_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateType();
        }

        void comboBox_addr_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_addr.Enabled)
            {
                int matchingIndex = comboBox_addr.FindStringExact(comboBox_addr.Text.ToUpper());

                if (comboBox_addr.SelectedIndex == -1 && matchingIndex != -1)
                {
                    comboBox_addr.SelectedIndex = matchingIndex;
                }

                if (comboBox_addr.SelectedIndex != -1)
                {
                    this.Sensor = (Sensor)comboBox_addr.SelectedValue;
                    initFromSensor();
                }
                else
                {
                    comboBox_type.Enabled = true;
                }
            }
        }
    }
}
