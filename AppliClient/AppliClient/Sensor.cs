using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace AppliClient
{
    public class Sensor : INotifyPropertyChanged
    {
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*
         * This set of data will be created by the user when he defines the sensor in the user interface
         */
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private Communication.sensor_type type;
        public Communication.sensor_type Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        private Communication.sensor_datatype dataType;
        public Communication.sensor_datatype DataType
        {
            get { return dataType; }
            set { dataType = value; OnPropertyChanged("DataType"); }
        }

        private Communication.threshold_type thresholdType;
        public Communication.threshold_type? ThresholdType
        {
            get
            {
                if (this.IsActuator)
                {
                    return null;
                }
                return thresholdType;
            }
            set
            {
                thresholdType = value.Value;
                OnPropertyChanged("ThresholdType");
                OnPropertyChanged("ThresholdString");
            }
        }

        private object threshold;
        public object Threshold
        {
            get
            {
                if (IsActuator)
                    return null;

                return threshold;
            }
            set
            {
                if (!IsActuator)
                {
                    switch (this.DataType)
                    {
                        case Communication.sensor_datatype.INT:
                            threshold = int.Parse(value.ToString());
                            break;
                        case Communication.sensor_datatype.FLOAT:
                            threshold = float.Parse(value.ToString());
                            break;
                        case Communication.sensor_datatype.BOOL:
                            threshold = bool.Parse(value.ToString());
                            break;
                    }
                    OnPropertyChanged("Threshold");
                    OnPropertyChanged("ThresholdString");
                }
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { address = value.ToUpper(); OnPropertyChanged("Address"); }
        }

        public int X { get; set; }
        public int Y { get; set; }

        /*
         * These variables are the one displayed on the interface
         */

        private object value;
        [YamlIgnore]
        public object Value
        {
            get { return this.value; }
            private set { this.value = value; OnPropertyChanged("Value"); }
        }

        private bool alarmed;
        [YamlIgnore]
        public bool Alarmed
        {
            get { return alarmed; }
            private set { alarmed = value; OnPropertyChanged("Alarmed"); }
        }

        private int linkQuality;
        [YamlIgnore]
        public int LinkQuality
        {
            get { return linkQuality; }
            private set { linkQuality = value; OnPropertyChanged("LinkQuality"); }
        }

        private DateTime? lastActivity;
        [YamlIgnore]
        public DateTime? LastActivity
        {
            get { return lastActivity; }
            private set { lastActivity = value; OnPropertyChanged("LastActivity"); OnPropertyChanged("LastActivityString"); }
        }

        public bool IsActuator
        {
            get
            {
                return Communication.IsActuatorType(this.Type);
            }
        }

        public string LinkQualityString
        {
            // TODO: the max value should be adjusted to match to the reality
            get { return (100 * this.LinkQuality / Byte.MaxValue) + "%"; }
        }


        public string ValueString
        {
            get
            {
                return FormatValue(this.Value);
            }
        }

        public string ThresholdString
        {
            get
            {
                string thresholdType = "";
                switch (this.ThresholdType)
                {
                    case Communication.threshold_type.NONE:
                        return "Désactivé";
                    case Communication.threshold_type.EQ:
                        if (this.DataType != Communication.sensor_datatype.BOOL)
                            thresholdType = "= ";
                        break;
                    case Communication.threshold_type.LT:
                        thresholdType = "< ";
                        break;
                    case Communication.threshold_type.LTE:
                        thresholdType = "<= ";
                        break;
                    case Communication.threshold_type.GT:
                        thresholdType = "> ";
                        break;
                    case Communication.threshold_type.GTE:
                        thresholdType = ">= ";
                        break;
                }
                return "Si " + thresholdType + FormatValue(this.Threshold);
            }
        }

        public string TypeString
        {
            get { return Communication.sensorTypeToString[this.Type]; }
        }

        public override string ToString()
        {
            return Name + " (" + Address + ")";
        }

        private string FormatValue(object value)
        {
            if (value == null)
                return "";

            switch (this.DataType)
            {
                case Communication.sensor_datatype.BOOL:
                    return ((bool)value) ? "Actif" : "Inactif";
                default:
                    return value.ToString();
            }
        }

        public Communication.sensor_info getInfo()
        {
            Communication.sensor_info info = new Communication.sensor_info();
            info.ieee_addr = Communication.StringToIeeeArray(this.Address);
            info.type = this.Type;
            info.datatype = this.DataType;
            if (this.Type != Communication.sensor_type.LED_ACTUATOR)
            {
                info.threshold_type = this.ThresholdType.Value;

                switch (this.DataType)
                {
                    case Communication.sensor_datatype.INT:
                        info.threshold.int_val = (int)this.Threshold;
                        break;
                    case Communication.sensor_datatype.FLOAT:
                        info.threshold.float_val = (float)this.Threshold;
                        break;
                    case Communication.sensor_datatype.BOOL:
                        info.threshold.bool_val = (bool)this.Threshold;
                        break;
                }
            }
            return info;
        }

        public void updateFromItem(Communication.SensorItem item)
        {
            if (item == null)
            {
                this.LastActivity = null;
                this.Value = null;
                this.LinkQuality = 0;
                this.Alarmed = false;
            }
            else
            {
                this.Type = item.info.type;
                this.DataType = item.info.datatype;
                this.ThresholdType = item.info.threshold_type;
                this.Address = Communication.IeeeArrayToString(item.info.ieee_addr);
                this.LinkQuality = item.lqi;
                this.LastActivity = item.last_activity;
                this.Alarmed = item.alarmed;

                switch (this.DataType)
                {
                    case Communication.sensor_datatype.INT:
                        this.Threshold = item.info.threshold.int_val;
                        this.Value = item.last_val.int_val;
                        break;
                    case Communication.sensor_datatype.BOOL:
                        this.Threshold = item.info.threshold.bool_val;
                        this.Value = item.last_val.bool_val;
                        break;
                    case Communication.sensor_datatype.FLOAT:
                        this.Threshold = item.info.threshold.float_val;
                        this.Value = item.last_val.float_val;
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
