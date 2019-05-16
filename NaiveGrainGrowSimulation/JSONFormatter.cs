using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaiveGrainGrowSimulation
{
    class JsonFormatter
    {
        private readonly Settings _settings;
        public JsonFormatter(Settings settings)
        {
            _settings = settings;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_settings,Formatting.Indented);
        }

        public static Settings Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<Settings>(json);
        }

        public static string Serialize(Cell[,] table)
        {

            return JsonConvert.SerializeObject(table, Formatting.None,new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        public static string Serialize(int[,] table)
        {
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }

    }
}
