using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveGrainGrowSimulation
{
    class Settings
    {
        public int GrainNumber { get; set; } = 10;
        public int NetHeight { get; set; } = 50;
        public int NetWidth { get; set; } = 50;
        public int CellSize { get; set; }
        public int CellSpace { get; set; }
        public int Radius { get; set; } = 3;
    }
}
