using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliClient
{
    public class TriggerRule
    {
        public string Name { get; set; }

        private string actuator;
        public string Actuator
        {
            get { return actuator; }
            set { actuator = value.ToUpper(); }
        }

        public List<string> Sensors { get; set; }

        public string SensorsString
        {
            get { return String.Join(", ", Sensors); }
        }

        public TriggerRule()
        {
            Sensors = new List<string>();
        }
    }
}
