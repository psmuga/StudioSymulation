using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            _fields = new Cell[_settings.NetHeight+2, _settings.NetWidth+2];
            for (int i = 0; i < _settings.NetHeight+2; i++)
            {
                for (int j = 0; j < _settings.NetWidth+2; j++)
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
                var i = _rand.Next(_settings.NetHeight)+1;
                var j = _rand.Next(_settings.NetWidth )+1;
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
                    Canvas.SetLeft(_fields[i, j].MyRectangle, (j-1) * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, (i-1) * _settings.CellSize);
                    index++;
                    _fields[i, j].Stan = true;
                }
            }
        }

        public void EvenGrain()
        {
            for (int i = 1; i < _settings.NetHeight+1; i+=(_settings.CellSpace+1))
            {
                for (int j = 1; j < _settings.NetWidth+1; j+=(_settings.CellSpace+1))
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;
                    var color = new SolidColorBrush(GetRandomColor());
                    r.Fill = color;
                    _fields[i, j].MyRectangle = r;
                    _fields[i, j].MyBrush = color;


                    _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, (j - 1) * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, (i - 1) * _settings.CellSize);
                    _fields[i, j].Stan = true;
                }
            }
        }

        public void PointByClick(Point point)
        {

            int i = (int) (point.Y / _settings.CellSize)+1;
            int j = (int) (point.X / _settings.CellSize)+1;

            var r = new Rectangle();
            r.Width = _settings.CellSize;
            r.Height = _settings.CellSize;
            var color = new SolidColorBrush(GetRandomColor());
            r.Fill = color;
            _fields[i, j].MyRectangle = r;// SetRectangle(r);
            _fields[i, j].MyBrush = color;


            _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
            Canvas.SetLeft(_fields[i, j].MyRectangle, (j - 1) * _settings.CellSize);
            Canvas.SetTop(_fields[i, j].MyRectangle, (i - 1) * _settings.CellSize);
            _fields[i, j].Stan = true;
        }

        public void RandomWithRadius()
        {
            IsTableClear();
            var index = 0;
           
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (index < _settings.GrainNumber)
            {
                var i = _rand.Next(_settings.NetHeight) + 1;
                var j = _rand.Next(_settings.NetWidth) + 1;

                var canDraw = true;
                int m = i - _settings.Radius;
                if (m < 1)
                {
                    m = 1;
                }
                int n = j - _settings.Radius;
                if (n <1)
                {
                    n = 1;
                }
                int x = i + _settings.Radius;
                if (x >= _fields.GetLength(0))
                {
                    x = _fields.GetLength(0) - 2;
                }
                int y = j + _settings.Radius;
                if (y >= _fields.GetLength(1))
                {
                    y = _fields.GetLength(1) - 2;
                }
                for (int k = m; k <= x; k++)
                {
                    for (int l = n; l <= y; l++)
                    {
                        if (_fields[k,l].Stan == true)
                        {
                            canDraw = false;
                            break;
                        }
                    }
                }

                if (stopwatch.Elapsed.TotalMilliseconds >= 1000)
                {
                    break;
                }


                if ((_fields[i, j].Stan == false) && canDraw )
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;
                    var color = new SolidColorBrush(GetRandomColor());
                    r.Fill = color;
                    _fields[i, j].MyRectangle = r;// SetRectangle(r);
                    _fields[i, j].MyBrush = color;


                    _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, (j - 1) * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, (i - 1) * _settings.CellSize);
                    index++;
                    _fields[i, j].Stan = true;
                }
            }
        }

        private void IsTableClear()
        {
            var help = true;
            for (int i = 0; i < _settings.NetHeight + 2; i++)
            {
                for (int j = 0; j < _settings.NetWidth + 2; j++)
                {
                    if (_fields[i,j].Stan==true)
                    {
                        help = false;
                        break;
                        
                    }
                }
            }
            Console.WriteLine($"Is Table clear: {help}");
        }

        public void ResetTable()
        {
            
        }

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)_rand.Next(255), (byte)_rand.Next(255), (byte)_rand.Next(255));
        }


    }
}
