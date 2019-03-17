using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveGrainGrowSimulation
{
    class Settings
    {
        public int GrainNumber { get; set; } = 20;
        public int NetHeight { get; set; } = 50;
        public int NetWidth { get; set; } = 50;
        public int CellSize { get; set; }
    }
}
