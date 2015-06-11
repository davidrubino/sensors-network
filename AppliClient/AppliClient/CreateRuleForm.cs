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
    public partial class CreateRuleForm : Form
    {
        public TriggerRule Rule { get; private set; }

        public CreateRuleForm(List<Sensor> sensorList, Sensor selected)
        {
            InitializeComponent();
            this.Rule = new TriggerRule();
            this.comboBox_actuator.DataSource = sensorList.Where(e => e.IsActuator).ToList();
            this.checkedListBox_sensors.DataSource = sensorList.Where(e => !e.IsActuator).ToList();

            if (selected != null)
            {
                if (selected.IsActuator)
                {
                    comboBox_actuator.SelectedItem = selected;
                }
                else
                {
                    int index = checkedListBox_sensors.Items.IndexOf(selected);
                    if (index != -1)
                    {
                        checkedListBox_sensors.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void textBox_name_Validating(object sender, CancelEventArgs e)
        {
            if (textBox_name.Text.Trim().Length == 0)
            {
                errorProvider.SetError(textBox_name, "Veuiller entrer un nom!");
            }
            else
            {
                errorProvider.SetError(textBox_name, null);
            }
        }

        private void comboBox_actuator_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox_actuator.SelectedIndex == -1)
            {
                errorProvider.SetError(comboBox_actuator, "Sélectionner un actionneur");
            }
            else
            {
                errorProvider.SetError(comboBox_actuator, null);
                
            }
        }

        private void checkedListBox_sensors_Validating(object sender, CancelEventArgs e)
        {
            if (checkedListBox_sensors.CheckedItems.Count != 0)
            {
                errorProvider.SetError(checkedListBox_sensors, null);
            }
            else
            {
                errorProvider.SetError(checkedListBox_sensors, "Sélectionner au moins un capteur");
            }
        }

        private void CreateAlertForm_FormClosing(object sender, FormClosingEventArgs e)
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

            Rule.Name = textBox_name.Text;
            Rule.Actuator = ((Sensor)comboBox_actuator.SelectedItem).Address;
            Rule.Sensors.Clear();
            Rule.Sensors.AddRange(checkedListBox_sensors.CheckedItems.Cast<Sensor>().Select(s => s.Address));
        }
    }
}
