using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace AppliClient
{
    /**
     * This class is responsible for storing and loading the configuration from disk.
     * Files will be saved relative to the current directory (usually the executable path)
     */
    public static class Config
    {
        private const string DB_FILENAME = "db.yaml";
        private const string MAP_FILENAME = "map.png";

        /*
         * Data to be be saved to a yaml file
         */
        private class YamlData
        {
            public List<Sensor> sensors { get; set; }
            public List<TriggerRule> rules { get; set; }
        }

        /**
         * Load all saved data from DB_FILENAME
         */
        public static void loadDB(FrmMain frmMain)
        {
            YamlData data = new YamlData();
            if (File.Exists(DB_FILENAME))
            {
                Deserializer deser = new Deserializer();
                StreamReader reader = File.OpenText(DB_FILENAME);
                data = deser.Deserialize<YamlData>(reader);
                reader.Close();

            }
            if (data != null && data.sensors != null)
            {
                foreach (Sensor sensor in data.sensors)
                {
                    frmMain.addDevice(sensor);
                }
            }

            if (data != null && data.rules != null)
            {
                foreach (TriggerRule rule in data.rules)
                {
                    frmMain.addRule(rule);
                }
            }
        }

        /**
         * Save all data to DB_FILENAME
         */
        public static void saveDB(List<Sensor> sensorList, List<TriggerRule> ruleList)
        {
            YamlData data = new YamlData();
            data.sensors = sensorList;
            data.rules = ruleList;

            Serializer ser = new Serializer(SerializationOptions.Roundtrip);
            using (TextWriter writer = File.CreateText(DB_FILENAME)) {
                ser.Serialize(writer, data);
            }
        }

        /**
         * Return the background image
         */
        public static Image getBackgroundImage()
        {
            try
            {
                return Image.FromFile(MAP_FILENAME);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /**
         * Set a new background image
         */
        public static void setBackgroundImage(Image image)
        {
            image.Save(MAP_FILENAME);
        }
    }
}
