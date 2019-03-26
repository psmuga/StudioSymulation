using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NaiveGrainGrowSimulation
{
    class Controller
    {
        private readonly Random _rand;
        private Cell[,] _fields;
        private readonly Settings _settings;

        private Canvas _boardCanvas;
        public Controller(Settings settings, Canvas canvas)
        {
            _rand = new Random();
            _settings = settings;
            _boardCanvas = canvas;
        }

        public void InitialiseTable()
        {
            _fields = new Cell[_settings.NetHeight, _settings.NetWidth];
            for (int i = 0; i < _settings.NetHeight; i++)
            {
                for (int j = 0; j < _settings.NetWidth; j++)
                {
                    _fields[i, j] = new Cell();
                }
            }
        }

        public void RandomGrain()
        {
            var index = 0;

            while (index < _settings.GrainNumber)
            {
                var i = _rand.Next(_settings.NetHeight - 1);
                var j = _rand.Next(_settings.NetWidth - 1);
                if (_fields[i, j].Stan == false)
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;
                    var color = new SolidColorBrush(GetRandomColor());
                    r.Fill = color;
                    _fields[i, j].MyRectangle = r;// SetRectangle(r);
                    _fields[i, j].MyBrush = color;


                    _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, j * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, i * _settings.CellSize);
                    index++;
                    _fields[i, j].Stan = true;
                }
            }
        }

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)_rand.Next(255), (byte)_rand.Next(255), (byte)_rand.Next(255));
        }


    }
}
