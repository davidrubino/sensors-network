using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliClient
{
    public partial class AddSensorDialog : Form
    {
        public AddSensorDialog()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (comboBox_type.SelectedItem == "Luminosité" || comboBox_type.SelectedItem == "Température"
                || comboBox_type.SelectedItem == "Contacteur")
            {

            }
        }

    }
}
