using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NaiveGrainGrowSimulation
{
    class Cell
    {
        public Rectangle MyRectangle { get; set; }
        public Brush MyBrush { get; set; }
        public bool Stan { get; set; }

        public Cell()
        {
            Stan = false;
            
            MyRectangle = new Rectangle();
            MyBrush = new SolidColorBrush(Colors.White);
        }

        public Rectangle GetRectangle()
        {
            return MyRectangle;
        }

        public void CleanCell()
        {
            MyRectangle.Fill = new SolidColorBrush(Colors.White);
            Stan = false;
        }

        //public Brush GetBrush()
        //{
        //    return MyBrush;
        //}

        //public void SetRectangle(Rectangle r)
        //{
        //    this.MyRectangle = r;
        //}

        //public void SetBrush(Brush b)
        //{
        //    this.MyBrush = b;
        //}
    }
}
