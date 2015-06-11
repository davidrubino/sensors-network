using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace AppliClient
{
    /**
     * Communication class responsible for communicating with the coordinator over the virtual serial port.
     */
    public class Communication
    {
        private SerialPort port;
        private const int IEEE_ADDR_SIZE = 8;
        private const String PONG = "PONG!";

        public event EventHandler SensorsChanged;

        // These structures are used to communicate through the serial ports
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct sensor_val
        {
            [FieldOffset(0)]
            public int int_val;

            [FieldOffset(0)]
            public float float_val;

            [FieldOffset(0)]
            private byte _bool_val;
            // see https://stackoverflow.com/questions/28999550/strange-unmarshalling-behavior-with-union-in-c-sharp
            public bool bool_val
            {
                get { return _bool_val != 0; }
                set { _bool_val = (byte)(value ? 1 : 0); }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct sensor_info
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = IEEE_ADDR_SIZE)]
            public byte[] ieee_addr;           // Unique 64 bits address

            public sensor_type type;
            public sensor_datatype datatype;
            public sensor_val threshold;
            public threshold_type threshold_type;
        }

        enum operation_code : byte
        {
            CMD_GET_SENSOR_LIST = 0,
            CMD_DELETE_SENSOR = 1,
            CMD_MODIFY_SENSOR = 2,
            CMD_DUMP_SENSORS = 3,
            CMD_ADD_RULE = 4,
            CMD_GET_RULE_LIST = 5,
            CMD_DELETE_RULE = 6,
            CMD_DUMP_RULES = 7,
            CMD_GET_TIME = 8,
            CMD_SEND_DATA = 9,
            CMD_NVRAM_INFO = 10,
            CMD_PING = 0x42,
            CMD_CLEAR_ALL = 0xA0,
        }

        private static Dictionary<error_code, string> errorMessagesString = new Dictionary<error_code, string>() {
            {error_code.ERROR_SUCCESS, "Pas d'erreur"},
            {error_code.ERROR_INVALID_FORMAT, "Données invalides envoyées par l'application"},
            {error_code.ERROR_NOT_FOUND, "Elément introuvable"},
            {error_code.ERROR_INTERNAL, "Erreur interne au coordinateur"},
        };


        enum error_code : byte
        {
            ERROR_SUCCESS = 0,         /* no error */
            ERROR_INVALID_FORMAT = 1,  /* some field was missing or had an invalid value */
            ERROR_NOT_FOUND = 2,       /* requested item was not found */
            ERROR_INTERNAL = 3,        /* internal error */
            ERROR_UNKNOWN = 0xFF,      /* unknown error */
        }

        public enum sensor_type : byte
        {
            SWITCH_SENSOR = 0,
            LIGHT_SENSOR = 1,
            LED_ACTUATOR = 2
        };

        public static Dictionary<sensor_type, string> sensorTypeToString = new Dictionary<sensor_type, string>() {
            {sensor_type.SWITCH_SENSOR, "Capteur : Interrupteur"},
            {sensor_type.LIGHT_SENSOR, "Capteur : Luminosité"},
            {sensor_type.LED_ACTUATOR, "Actionneur : LED"}
        };

        public enum sensor_datatype : byte
        {
            BOOL = 0,
            INT = 1,
            FLOAT = 2
        };

        public enum threshold_type : byte
        {
            NONE = 0,  /* No threshold set */
            EQ,        /* Equal */
            LTE,       /* Less than or equal (valid only for FLOAT and INT types) */
            LT,        /* Less than (strict, valid only for FLOAT and INT types) */
            GTE,       /* Greater than or equal (valid only for FLOAT and INT types) */
            GT         /* Greater than (strict, valid only for FLOAT and INT types) */
        };

        public static Dictionary<threshold_type, string> thresholdTypeString = new Dictionary<threshold_type, string>() {
            {threshold_type.EQ, "="},
            {threshold_type.LTE, "<="},
            {threshold_type.LT, "<"},
            {threshold_type.GTE, ">="},
            {threshold_type.GT, ">"}
        };

        public List<SensorItem> sensors;
        public List<EventRule> rules;
        private const int TIMER_INTERVAL = 100; // Sensor refresh rate
        //Le coordinateur doit répondre plus rapidement à un ping
        private const int TIMEOUT_CHECK_CONNECTED = 500; // Timeout for initial connection check
        private const int TIMEOUT_COORDINATOR = 2000; // Timeout for normal operation
        
        /**
         * Number of seconds needed to add to the time given to the coordinator to get the real time 
         */
        public long DeltaTime { get; set; }

        public class EventRule
        {
            public UInt16 id;
            public string ieee_addr_target;
            public List<string> ieee_addr_conds;
        }

        public class SensorItem
        {
            public sensor_info info;
            public DateTime? last_activity;      // Time of last communication with the sensor (in seconds,
                                                 // measured relative to the coordinator start-up (0))
            public sensor_val last_val;     // last data communicated by the sensor
            public bool alarmed;            // Alarm status
            public byte lqi;                // Link quality indicator
        }

        public static sensor_datatype? GetDataType(sensor_type type)
        {
            switch (type)
            {
                case sensor_type.LED_ACTUATOR:
                case sensor_type.SWITCH_SENSOR:
                    return sensor_datatype.BOOL;
                case sensor_type.LIGHT_SENSOR:
                    return sensor_datatype.INT;
                default:
                    return null;
            }
        }


        public static bool IsActuatorType(sensor_type type)
        {
            switch (type)
            {
                case sensor_type.LED_ACTUATOR:
                    return true;
                default:
                    return false;
            }
        }

        public Communication()
        {
            sensors = new List<SensorItem>();
            rules = new List<EventRule>();

            // Try to communicate with the cordinator
            while (!initiateConnection())
            {
                DialogResult result = MessageBox.Show("Impossible de communiquer avec le coordinateur.\r\n\r\nAssurez-vous que celui-ci est bien relié et qu'aucune application n'utilise le port série.", "Erreur de communication", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }
            }

            // We change the timeout to allow longer operations
            port.ReadTimeout = TIMEOUT_COORDINATOR;

            //We synchronize the time once and we do a first update of the data
            synchronizeTime();
            updateData();

            new Thread(updateDataThread).Start();
        }

        private void synchronizeTime()
        {
            sendCmd((byte)operation_code.CMD_GET_TIME);
            DeltaTime = DateTime.Now.Ticks - TimeSpan.TicksPerSecond * readStruct<UInt32>();
        }

        private void updateDataThread()
        {
            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                // TODO: enabling this this causes unexpected errors
                // perhaps enclosing this in "lock (port) {...}" will help
                //synchronizeTime();

                updateData();
                Thread.Sleep(TIMER_INTERVAL);
            }
        }

        // This function is called periodically to update the list of sensor_item and of event_rule
        private void updateData()
        {
            try
            {
                lock (port)
                {
                    updateSensorItems();
                    updateRules();
                }
            }
            catch (System.TimeoutException e)
            {
                Console.Error.WriteLine("TimeOut lors d'une tentative de mise à jour : " + e);
                MessageBox.Show("Connexion avec le coordinateur perdue", "Connexion perdue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Update the list of event_rule according to the communication protocol
        private void updateRules()
        {
            List<EventRule> new_rules = new List<EventRule>();
            sendCmd((byte)operation_code.CMD_GET_RULE_LIST);
            int nb_rules = port.ReadByte();
            for (int i = 0; i < nb_rules; i++)
            {
                EventRule rule = new EventRule();
                rule.id = readStruct<UInt16>();
                byte[] addr = readNBytes(IEEE_ADDR_SIZE);
                rule.ieee_addr_target = IeeeArrayToString(addr);
                int nb_conds = port.ReadByte();
                rule.ieee_addr_conds = new List<string>();
                for (int j = 0; j < nb_conds; j++)
                {
                    addr = readNBytes(IEEE_ADDR_SIZE);
                    rule.ieee_addr_conds.Add(IeeeArrayToString(addr));
                }
                new_rules.Add(rule);
            }
            rules = new_rules;
        }

        byte[] readNBytes(int n)
        {
            int offset = 0;
            byte[] buffer = new byte[n];
            while (offset < n)
            {
                offset += port.Read(buffer, offset, n - offset);
            }
            return buffer;
        }

        //Update the list of sensor_item according to the communication protocol
        private void updateSensorItems()
        {
            sendCmd((byte)operation_code.CMD_GET_SENSOR_LIST);
            int count = port.ReadByte();
            List<SensorItem> new_sensors = new List<SensorItem>();
            for (int i = 0; i < count; i++)
            {
                SensorItem sensor = new SensorItem();
                sensor.info = readStruct<sensor_info>();
                UInt32 last_act = readStruct<UInt32>();
                
                if (last_act != 0)
                    sensor.last_activity = new DateTime(DeltaTime + last_act * TimeSpan.TicksPerSecond);
                else
                    sensor.last_activity = null;

                sensor.last_val = readStruct<sensor_val>();
                sensor.alarmed = (readStruct<Byte>() != 0);
                sensor.lqi = (byte)port.ReadByte();
                new_sensors.Add(sensor);
            }

            sensors = new_sensors;

            if (SensorsChanged != null)
            {
                SensorsChanged(this, EventArgs.Empty);
            }
        }

        private void sendCmd(byte cmd)
        {
            port.DiscardInBuffer();
            port.Write(new byte[] { cmd }, 0, 1);
        }

        T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        T readStruct<T>() where T : struct
        {
            int len = Marshal.SizeOf(typeof(T));
            return ByteArrayToStructure<T>(readNBytes(len));
        }

        public static Byte[] StructureToByteArray<T>(T s) where T : struct
        {
            int objsize = Marshal.SizeOf(s);
            Byte[] ret = new Byte[objsize];
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(s, buff, false);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);

            return ret;
        }

        /**
         * Return true if we could communicate with the coordinator
         */
        public bool initiateConnection()
        {
            // For each available port, we try to "ping" the coordinator and wait for the good answer. If not given, we switch to another port.
            foreach (string portName in SerialPort.GetPortNames())
            {
                Console.WriteLine("Portname : " + portName);
                port = new SerialPort(portName, 115200);
                port.Handshake = Handshake.RequestToSend;
                port.ReadTimeout = TIMEOUT_CHECK_CONNECTED;

                try
                {
                    port.Open();
                }
                catch (System.UnauthorizedAccessException e)
                {
                    //On passe au port suivant
                    Console.WriteLine("Unauthorized access");
                    continue;
                }

                sendCmd((byte)operation_code.CMD_PING);
                try
                {
                    byte[] buff = readNBytes(PONG.Length);
                    String pong = System.Text.Encoding.UTF8.GetString(buff);

                    Console.WriteLine("Ping ? " + pong);
                    if (pong == PONG)
                    {
                        return true;
                    }
                }
                catch (System.TimeoutException e)
                {
                    //On passe au port suivant
                    Console.WriteLine("Timeout lors de l'initialisation");
                    continue;
                }
                port.Close();
            }

            return false;
        }


        public SensorItem getSensor(string addr)
        {
            foreach (SensorItem sensor in sensors)
            {
                if (IeeeArrayToString(sensor.info.ieee_addr) == addr)
                {
                    return sensor;
                }
            }
            return null;
        }

        //Convert an ieee_addr byte array to a string
        public static string IeeeArrayToString(byte[] ba)
        {
            //The ieee_addr byte array is reversed
            byte[] ba_reversed = new byte[ba.Length];
            for (int i = 0; i < ba.Length; i++)
            {
                ba_reversed[i] = ba[ba.Length - 1 - i];
            }
            string hex = BitConverter.ToString(ba_reversed);
            return hex.Replace("-", "");
        }

        public static byte[] StringToIeeeArray(string str)
        {
            int NumberChars = str.Length;
            byte[] bytes = new byte[NumberChars / 2];
            //The ieee_addr byte array is reversed
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(str.Substring(NumberChars - 2 - i, 2), 16);
            return bytes;
        }

        /**
         * Add a sensor by sending data according to the communication protocol
         */
        public void addSensor(Sensor sensor)
        {
            //We fill a 'sensor_item_data' structure which needs to be sent
            sensor_info info = sensor.getInfo();

            new Thread(() =>
            {
                //We want to send all the bytes in only one write to avoid error due to delays
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)operation_code.CMD_MODIFY_SENSOR);
                bytes.AddRange(StructureToByteArray(info));
                lock (port)
                {
                    port.DiscardInBuffer();
                    sendBytes(bytes.ToArray());
                    checkAnswer();
                }
            }).Start();
        }

        /**
         * Delete a sensor by sending data according to the communication protocol
         */
        public void deleteSensor(Sensor sensor)
        {
            new Thread(() =>
            {
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)operation_code.CMD_DELETE_SENSOR);
                bytes.AddRange(StringToIeeeArray(sensor.Address));
                lock (port)
                {
                    port.DiscardInBuffer();
                    sendBytes(bytes.ToArray());
                    checkAnswer();
                }
            }).Start();
        }

        private void sendBytes(byte[] bytes)
        {
            port.DiscardInBuffer();
            port.Write(bytes, 0, bytes.Length);
        }

        /*
         * Check if the coordinator replied successfully with ERROR_SUCCESS
         */
        private bool checkAnswer()
        {
            error_code answer;
            try
            {
                answer = (error_code) readStruct<Byte>();
            }
            catch (System.TimeoutException)
            {
                MessageBox.Show("Le coordinateur n'a pas répondu", "Erreur du coordinateur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (answer != error_code.ERROR_SUCCESS)
            {
                string error = errorMessagesString[answer];
                MessageBox.Show("Le coordinateur a renvoyé une erreur : " + error, "Erreur du coordinateur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /**
         * Add a rule by sending data according to the communication protocol
         */
        public void addRule(TriggerRule alert)
        {
            new Thread(() =>
            {
                // We need to send all the bytes in one write to avoid error due to delays
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)operation_code.CMD_ADD_RULE);
                bytes.AddRange(StringToIeeeArray(alert.Actuator));
                bytes.Add((byte)alert.Sensors.Count);
                foreach (string sensor in alert.Sensors)
                {
                    bytes.AddRange(StringToIeeeArray(sensor));
                }
                lock (port)
                {
                    port.DiscardInBuffer();
                    sendBytes(bytes.ToArray());
                    checkAnswer();
                }
            }).Start();
        }

        /*
         * Delete a rule by sending data according to the communication protocol
         */
        public void deleteRule(EventRule rule)
        {
            new Thread(() =>
            {
                // We need to send all the bytes in one write to avoid error due to delays
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)operation_code.CMD_DELETE_RULE);
                bytes.AddRange(StructureToByteArray(rule.id));
                lock (port)
                {
                    port.DiscardInBuffer();
                    sendBytes(bytes.ToArray());
                    checkAnswer();
                }
            }).Start();
        }

        /*
         * Find the matching rule in the list of rules
         */
        public EventRule getRule(string actuator, List<string> conds)
        {
            foreach (EventRule rule in rules)
            {
                bool same = true;
                same &= String.Equals(rule.ieee_addr_target, actuator, StringComparison.InvariantCultureIgnoreCase);
                same &= conds.Count == rule.ieee_addr_conds.Count;
                foreach (string cond in rule.ieee_addr_conds)
                {
                    same &= conds.Exists(e => String.Equals(e, cond, StringComparison.InvariantCultureIgnoreCase));
                }
                if (same)
                {
                    return rule;
                }
            }
            return null;
        }

        /**
         * Delete a rule by sending data according to the communication protocol
         */
        public void deleteRule(TriggerRule rule)
        {
            EventRule evrule = getRule(rule.Actuator, rule.Sensors);

            // The rule should already exist
            if (evrule != null) {
                deleteRule(evrule);
            }
        }

        /**
         * Synchronize with the coordinator in case there are inconsistencies
         */
        public void synchronize(FrmMain frmMain, List<Sensor> sensorList, List<TriggerRule> ruleList)
        {
            foreach (Sensor sensor in sensorList)
            {
                SensorItem sensorItem = getSensor(sensor.Address);
                if (sensorItem == null)
                {
                    // This sensor is no longer known on the network, we have to add it back
                    addSensor(sensor);
                }
                else
                {
                    // We update data in case it was changed
                    sensor.updateFromItem(sensorItem);
                }
            }
            foreach (TriggerRule rule in ruleList)
            {
                if (getRule(rule.Actuator, rule.Sensors) == null)
                {
                    //The rule is missing, we must add it
                    addRule(rule);
                }
            }
            // We check if the software is missing rules from the coordinator
            foreach (EventRule rule in this.rules)
            {
                bool found = false;
                foreach (TriggerRule triggerRule in ruleList)
                {
                    bool same = true;
                    same &= rule.ieee_addr_target == triggerRule.Actuator;
                    same &= rule.ieee_addr_conds.Count == triggerRule.Sensors.Count;
                    foreach (string cond in rule.ieee_addr_conds)
                    {
                        same &= triggerRule.Sensors.Contains(cond);
                    }
                    if (same)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    //TODO: Faire que des ieeeAlert pour prendre en compte les modifications de nom notamment
                    frmMain.addRule(new TriggerRule()
                    {
                        Name = "Règle n°" + rule.id,
                        Actuator = rule.ieee_addr_target,
                        Sensors = rule.ieee_addr_conds
                    });
                }
            }
        }
    }
}
