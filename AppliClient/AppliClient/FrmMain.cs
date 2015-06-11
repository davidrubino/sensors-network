using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliClient
{
    public partial class FrmMain : Form
    {
        private Communication com;
        private bool isDragging = false;
        private int currentX, currentY;
        private BindingList<TriggerRule> ruleList;
        private BindingList<SensorView> sensorViewList;

        private List<Sensor> sensorList
        {
            get { return sensorViewList.Select(v => v.Sensor).ToList(); }
        }

        /** List of orphan sensors (sensors which are known to the coordinator but have
         *  not been added to the user interface
         */
        public List<Sensor> OrphanSensors
        {
            get
            {
                return com.sensors.Select(
                    e =>
                    {
                        Sensor sensor = new Sensor();
                        sensor.updateFromItem(e);
                        return sensor;
                    }
                ).Where(e1 =>
                    !sensorList.Select(e2 => e2.Address).Contains(e1.Address)
                ).ToList();
            }
        }

        public FrmMain()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            ResetLabels();
            this.sensorViewList = new BindingList<SensorView>();
            this.ruleList = new BindingList<TriggerRule>();

            dataGridView_rules.AutoGenerateColumns = false;
            dataGridView_sensors.AutoGenerateColumns = false;
            dataGridView_sensors.DataSource = this.sensorViewList;
            dataGridView_rules.DataSource = this.ruleList;

            // Default background is set in the Properties/Resources.resx file
            pictureBox_Map.Image = Config.getBackgroundImage() ?? Properties.Resources.DefaultBackground;

            com = new Communication();
            com.SensorsChanged += com_SensorsChanged;

            try
            {
                Config.loadDB(this);
            }
            catch (Exception)
            {
                if (MessageBox.Show("Erreur lors du chargement du fichier de configuration.\r\n\r\nVoulez-vous continuer ? Le fichier sera écrasé en cas de modification.", "Fichier de configuration corrompu", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != System.Windows.Forms.DialogResult.Yes)
                {
                    Environment.Exit(1);
                    return;
                }
            }
            com.synchronize(this, sensorList, ruleList.ToList());
        }

        void com_SensorsChanged(object sender, EventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)(() =>
                {
                    foreach (Sensor sensor in sensorList)
                    {
                        Communication.SensorItem item = com.sensors.FirstOrDefault(s =>  Communication.IeeeArrayToString(s.info.ieee_addr) == sensor.Address);
                        sensor.updateFromItem(item);
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                // Happens if Form is not ready, just ignore the event.
            }
        }

        private void BindSelection(SensorView sensorView)
        {
            Sensor sensor = sensorView.Sensor;
            Communication.SensorItem sensorItem = com.getSensor(sensor.Address);

            ResetLabels();

            label_name.DataBindings.Add("Text", sensor, "Name");
            label_address.DataBindings.Add("Text", sensor, "Address");
            label_type.DataBindings.Add("Text", sensor, "TypeString");
            label_value.DataBindings.Add("Text", sensor, "ValueString");
            label_threshold.DataBindings.Add("Text", sensor, "ThresholdString");
            label_lastActivityState.DataBindings.Add("Text", sensor, "LastActivity", true, DataSourceUpdateMode.Never, "Inconnue", "d/M/yy HH:mm:ss");
            label_wirelessSignal.DataBindings.Add("Text", sensor, "LinkQualityString");

            bool isActuator = sensor.IsActuator;
            label_thresholdText.Visible = !isActuator;
            label_threshold.Visible = !isActuator;
        }

        private void ResetLabels()
        {
            label_name.DataBindings.Clear();
            label_address.DataBindings.Clear();
            label_type.DataBindings.Clear();
            label_value.DataBindings.Clear();
            label_threshold.DataBindings.Clear();
            label_lastActivityState.DataBindings.Clear();
            label_wirelessSignal.DataBindings.Clear();

            label_name.Text = "";
            label_address.Text = "";
            label_type.Text = "";
            label_value.Text = "";
            label_threshold.Text = "";
            label_lastActivityState.Text = "";
            label_wirelessSignal.Text = "";
        }

        private void PositionSensor(SensorView view)
        {
            if (view.Location.X == 0) {
                view.Location = new Point(pictureBox_Map.Height / 2, pictureBox_Map.Height / 2);
                var pos = this.PointToScreen(view.Location);
                pos = pictureBox_Map.PointToClient(pos);
                view.Location = pos;
            }
            view.Parent = pictureBox_Map;
            view.MouseDown += sensor_MouseDown;
            view.MouseMove += sensor_MouseMove;
            view.MouseUp += sensor_MouseUp;
            view.Click += sensor_Click;
        }

        public void addDevice(Sensor sensor)
        {
            SensorView view = new SensorView(sensor);
            this.sensorViewList.Add(view);
            PositionSensor(view);
        }

        private void sensor_Click(object sender, EventArgs e)
        {
            SensorView s = (SensorView)sender;

            foreach (DataGridViewRow row in dataGridView_sensors.Rows)
            {
                if ((SensorView)row.DataBoundItem == s)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private void sensor_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            currentX = e.X;
            currentY = e.Y;
        }

        private void sensor_MouseMove(object sender, MouseEventArgs e)
        {
            SensorView s = sender as SensorView;
            if (isDragging)
            {
                s.Top = s.Top + (e.Y - currentY);
                s.Left = s.Left + (e.X - currentX);
            }
        }

        private void sensor_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            saveDB();
        }

        private void toolStripButton_add_Click(object sender, EventArgs e)
        {
            CreateOrModifyComponentForm newForm = new CreateOrModifyComponentForm(this.OrphanSensors);

            if (newForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                addDevice(newForm.Sensor);
                com.addSensor(newForm.Sensor);
                saveDB();
            }
        }

        private void saveDB()
        {
            Config.saveDB(sensorList, ruleList.ToList());
        }

        private void toolStripButton_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_sensors.SelectedRows.Count > 0)
            {
                SensorView view = (SensorView)dataGridView_sensors.SelectedRows[0].DataBoundItem;
                sensorViewList.Remove(view);

                // Delete sensor on the coordinator
                com.deleteSensor(view.Sensor);
                saveDB();
                view.Dispose();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un composant à supprimer", "Suppression d'un composant",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView_sensors.SelectedRows.Count > 0)
            {
                SensorView view = (SensorView)dataGridView_sensors.SelectedRows[0].DataBoundItem;

                CreateOrModifyComponentForm newForm = new CreateOrModifyComponentForm(view.Sensor);
                if (newForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    com.addSensor(view.Sensor);
                    saveDB();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un composant à modifier", "Modification d'un composant",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Sélectionnez le plan à charger";
            dlg.Filter = "Fichiers images|*.jpg,*.jpeg,*.png,*.bmp,*.gif|Tous les fichiers|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pictureBox_Map.Image = Image.FromStream(dlg.OpenFile());
                Config.setBackgroundImage(pictureBox_Map.Image);
            }

            dlg.Dispose();
        }

        private void toolStripButton_eventAdd_Click(object sender, EventArgs e)
        {
            if (this.sensorList.Exists(s => s.IsActuator) && this.sensorList.Exists(s => !s.IsActuator))
            {
                Sensor selected = null;
                if (dataGridView_sensors.SelectedRows.Count > 0)
                {
                    selected = ((SensorView)dataGridView_sensors.SelectedRows[0].DataBoundItem).Sensor;
                }
                CreateRuleForm form = new CreateRuleForm(sensorList, selected);

                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    addRule(form.Rule);
                    com.addRule(form.Rule);
                    saveDB();
                }
            }
            else
            {
                MessageBox.Show("Veuillez créer au moins un capteur et un actionneur", "Creation d'une règle",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton_eventDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView_rules.SelectedRows.Count > 0)
            {
                var row = dataGridView_rules.SelectedRows[0];
                TriggerRule rule = (TriggerRule)row.DataBoundItem;
                com.deleteRule(rule);
                dataGridView_rules.Rows.Remove(row);
                this.ruleList.Remove(rule);
                saveDB();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une règle à supprimer", "Suppression d'une règle",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView_sensors_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_sensors.SelectedRows.Count > 0)
            {
                SensorView selectedSensor = (SensorView)dataGridView_sensors.SelectedRows[0].DataBoundItem;
                foreach (SensorView view in sensorViewList)
                {
                    if (view != selectedSensor)
                    {
                        view.Selected = false;
                    }
                }

                selectedSensor.Selected = true;
                BindSelection(selectedSensor);
            }
            else
            {
                ResetLabels();
            }
        }

        public void addRule(TriggerRule rule)
        {
            ruleList.Add(rule);
        }
    }

    /** A label subclass that lets mouse event pass through and hit the parent control.
     * 
     * This is from http://stackoverflow.com/a/8635626/179926
     */
    public class MouseTransparentLabel : Label
    {
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
