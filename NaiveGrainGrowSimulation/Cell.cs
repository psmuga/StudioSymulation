using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace NaiveGrainGrowSimulation
{
    [JsonObject]
    class Cell
    {
        [JsonIgnore]
        public Rectangle MyRectangle { get; set; }
        [JsonIgnore]
        public Brush MyBrush { get; set; }
        [JsonIgnore]
        public bool Stan { get; set; } = false;
        [JsonIgnore]
        public byte Orientation { get; set; } = 0;
        [JsonProperty]
        public int Id { get; set; } = 0;

        //public Cell()
        //{
        //    //Stan = false;

        //    //MyRectangle = new Rectangle();
        //    //MyBrush = new SolidColorBrush(Colors.White);
        //}


    }
}
