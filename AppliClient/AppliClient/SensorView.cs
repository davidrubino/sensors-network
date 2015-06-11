using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliClient
{
    public partial class SensorView : UserControl
    {
        /** Color for unknown sensors/actuators (stored in the coordinator but have never been active) since its boot */
        public static readonly Color UNKNOWN_COLOR = Color.Gray;

        /** Color for inactive sensors/actuators (that had not been active for some defined time) */
        public static readonly Color INACTIVE_COLOR = Color.Orange;

        /** Color for normal sensors/actuators which are not in alarm mode */
        public static readonly Color NO_ALARM_COLOR = Color.Green;

        /** Color for sensors in alarm mode or actuators that are active */
        public static readonly Color ALARM_COLOR = Color.Red;

        /** Time after which a sensor/actuator is considered to be inactive */
        public static readonly TimeSpan INACTIVITY_TIME = new TimeSpan(0, 0, 5); 

        public Sensor Sensor { get; private set; }

        public SensorView(Sensor sensor)
        {
            this.Sensor = sensor;
            this.Location = new Point(sensor.X, sensor.Y);

            InitializeComponent();
            Sensor.PropertyChanged += Sensor_PropertyChanged;
            this.Cursor = Cursors.Hand;
            update();
        }

        public string SensorName
        {
            get { return Sensor.Name; }
        }

        public string SensorType
        {
            get { return Sensor.IsActuator ? "Actionneur" : "Capteur"; }
        }

        public string Address
        {
            get { return Sensor.Address; }
        }

        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.BorderStyle = selected ? BorderStyle.FixedSingle : BorderStyle.None;
            }
        }
        void Sensor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            update();
        }

        private void SensorView_LocationChanged(object sender, EventArgs e)
        {
            this.Sensor.X = Location.X;
            this.Sensor.Y = Location.Y;
        }

        void update()
        {
            label_type.Text = Sensor.IsActuator ? "A" : "C";

            if (!Sensor.LastActivity.HasValue)
            {
                this.BackColor = makeTransparent(UNKNOWN_COLOR);
            }
            else if (Sensor.Alarmed || (Sensor.IsActuator && Sensor.DataType == Communication.sensor_datatype.BOOL && Boolean.Equals(true, Sensor.Value)))
            {
                this.BackColor = makeTransparent(ALARM_COLOR);
            }
            else if ((DateTime.Now - Sensor.LastActivity.Value) > INACTIVITY_TIME)
            {
                this.BackColor = makeTransparent(INACTIVE_COLOR);
            }
            else
            {
                this.BackColor = makeTransparent(NO_ALARM_COLOR);
            }
        }

        private Color makeTransparent(Color color)
        {
            return Color.FromArgb(100, color.R, color.G, color.B);
        }
    }
}
