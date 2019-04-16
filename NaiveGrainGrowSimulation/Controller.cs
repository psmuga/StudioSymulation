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
using System.Windows.Threading;

namespace NaiveGrainGrowSimulation
{
    class Controller
    {
        private readonly Random _rand;
        private Cell[,] _fields;
        private Cell[,] _newFields;
        private readonly Settings _settings;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Canvas _boardCanvas;

        public bool ResetFlag { get; set; } = false;
        public bool Finished { get; set; } = false;



        public Controller(Settings settings, Canvas canvas)
        {
            _rand = new Random();
            _settings = settings;
            _boardCanvas = canvas;
        }

        public void InitialiseTable()
        {
            _fields = new Cell[_settings.NetHeight+2, _settings.NetWidth+2];
            _newFields = new Cell[_settings.NetHeight + 2, _settings.NetWidth + 2];
            for (int i = 0; i < _settings.NetHeight+2; i++)
            {
                for (int j = 0; j < _settings.NetWidth+2; j++)
                {
                    _fields[i, j] = new Cell();
                    _newFields[i, j] = new Cell();
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
                    _fields[i, j].MyRectangle = r;
                    _fields[i, j].MyBrush = color;

                    _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, (j-1) * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, (i-1) * _settings.CellSize);
                    index++;
                    _fields[i, j].Stan = true;
                    RandSideHexaRandom(i, j);
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
                    RandSideHexaRandom(i, j);
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
            _fields[i, j].MyRectangle = r;
            _fields[i, j].MyBrush = color;

            _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
            Canvas.SetLeft(_fields[i, j].MyRectangle, (j - 1) * _settings.CellSize);
            Canvas.SetTop(_fields[i, j].MyRectangle, (i - 1) * _settings.CellSize);
            _fields[i, j].Stan = true;

            RandSideHexaRandom(i,j);
        }

        public void RandomWithRadius()
        {
            //IsTableClear();
            var index = 0;
           
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (index < _settings.GrainNumber)
            {
                var i = _rand.Next(_settings.NetHeight) + 1;
                var j = _rand.Next(_settings.NetWidth) + 1;

                var canDraw = true;
                int m = (i - _settings.Radius < 1) ? 1 : i - _settings.Radius;
                //if (m < 1)
                //{
                //    m = 1;
                //}
                int n = (j - _settings.Radius < 1) ? 1 : j-_settings.Radius;
                //if (n <1)
                //{
                //    n = 1;
                //}
                int x = i + _settings.Radius >= _fields.GetLength(0) ? _fields.GetLength(0) - 2 : i + _settings.Radius;
                //if (x >= _fields.GetLength(0))
                //{
                //    x = _fields.GetLength(0) - 2;
                //}
                int y = j + _settings.Radius >= _fields.GetLength(1) ? _fields.GetLength(1) - 2 : j + _settings.Radius;
                //if (y >= _fields.GetLength(1))
                //{
                //    y = _fields.GetLength(1) - 2;
                //}

                for (var k = m; k <= x; k++)
                {
                    for (var l = n; l <= y; l++)
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
                    RandSideHexaRandom(i, j);
                }
            }
        }



        public void PerformIterations(bool toEnd = false)
        {
            if (toEnd ==false)
            {
                PerformIteration();
            }
            else
            {
                _timer.Interval = TimeSpan.FromMilliseconds(1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }


        private void RandSideHexaRandom(int i, int j)
        {
            _fields[i, j].Orientation = (byte) _rand.Next(4);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ResetFlag == true)
            {
                _timer.Stop();
            }
            PerformIteration();
            if (IsDone())
            {
                _timer.Stop();
                Finished = true;
               // Console.WriteLine("done");
            }
        }

        private void PerformIteration()
        {

            
            ///TODO czy na pewno to tutaj potrzebne
            /// 
            for (int i = 0; i < _settings.NetHeight + 2; i++)
            {
                for (int j = 0; j < _settings.NetWidth + 2; j++)
                {
                    _newFields[i, j].Stan = _fields[i, j].Stan;
                    _newFields[i, j].MyRectangle = _fields[i, j].MyRectangle;
                    _newFields[i, j].MyBrush = _fields[i, j].MyBrush;
                    _newFields[i, j].Orientation = _fields[i, j].Orientation;
                }
            }

            if (_settings.EdgeCondition.Equals(EdgeCondition.Periodic))
            {
                Periodic();
            }

            for (var i = 1; i < _settings.NetHeight+1; i++)
            {
                for (var j = 1; j < _settings.NetWidth+1; j++)
                {
                    switch (_settings.NeighborhoodType)
                    {
                        case (NeighborhoodType.Moore):
                            Moore(i, j);
                            break;
                        case (NeighborhoodType.VonNeumann):
                            Neumann(i,j);
                            break;
                        case NeighborhoodType.HeksagonalLeft:
                            HexaLeft(i,j);
                            break;
                        case NeighborhoodType.HeksagonalRight:
                            HexaRight(i,j);
                            break;
                        case NeighborhoodType.HeksagonalRandom:
                            if (_fields[i,j].Orientation < 2)
                            {
                                HexaLeft(i,j);
                            }
                            else
                            {
                                HexaRight(i,j);
                            }
                            break;
                        case NeighborhoodType.PentagonalRandom:
                            switch (_fields[i,j].Orientation)
                            {
                                case 0:
                                    PentaLeft(i,j);
                                    break;
                                case 1:
                                    PentaRight(i,j);
                                    break;
                                case 2:
                                    PentaTop(i,j);
                                    break;
                                default:
                                    PentaBottom(i,j);
                                    break;
                            }
                            break;

                    }
                }
            }

            UpdateFields();
            DrawUpdate();
        }

        private bool IsDone()
        {
            return _fields.Cast<Cell>().All(i => i.Stan);
        }

        private void SetCellValue(int i, int j, int m1, int m2)
        {
            _newFields[i + m1, j + m2].Stan = true;
            _newFields[i + m1, j + m2].MyBrush = _fields[i, j].MyBrush;
            _newFields[i + m1, j + m2].MyRectangle = _fields[i, j].MyRectangle;
            _newFields[i + m1, j + m2].Orientation = _fields[i, j].Orientation;
        }

        private void HexaLeft(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j - 1].Stan == false)
                SetCellValue(i,j,-1,-1);

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i, j, -1, 0);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);

            if (_fields[i + 1, j + 1].Stan == false)
                SetCellValue(i, j, 1, 1);
        }

        private void HexaRight(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i,j,-1,0);

            if (_fields[i - 1, j + 1].Stan == false)
                SetCellValue(i,j,-1,1);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i,j,0,-1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i,j,0,1);

            if (_fields[i + 1, j - 1].Stan == false)
                SetCellValue(i,j,1,-1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i,j,1,0);
        }

        private void Neumann(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i,j,-1,0);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);
        }

        private void Moore(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j - 1].Stan == false)
                SetCellValue(i, j, -1, -1);

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i, j, -1, 0);

            if (_fields[i - 1, j + 1].Stan == false)
                SetCellValue(i, j, -1, 1);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);

            if (_fields[i + 1, j - 1].Stan == false)
                SetCellValue(i, j, 1, -1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);

            if (_fields[i + 1, j + 1].Stan == false)
                SetCellValue(i, j, 1, 1);
        }

        private void PentaLeft(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j - 1].Stan == false)
                SetCellValue(i, j, -1, -1);

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i, j, -1, 0);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i + 1, j - 1].Stan == false)
                SetCellValue(i, j, 1, -1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);
        }

        private void PentaRight(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i, j, -1, 0);

            if (_fields[i - 1, j + 1].Stan == false)
                SetCellValue(i, j, -1, 1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);

            if (_fields[i + 1, j + 1].Stan == false)
                SetCellValue(i, j, 1, 1);
        }

        private void PentaBottom(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);

            if (_fields[i + 1, j - 1].Stan == false)
                SetCellValue(i, j, 1, -1);

            if (_fields[i + 1, j].Stan == false)
                SetCellValue(i, j, 1, 0);

            if (_fields[i + 1, j + 1].Stan == false)
                SetCellValue(i, j, 1, 1);
        }

        private void PentaTop(int i, int j)
        {
            if (_fields[i, j].Stan != true) return;

            if (_fields[i - 1, j - 1].Stan == false)
                SetCellValue(i, j, -1, -1);

            if (_fields[i - 1, j].Stan == false)
                SetCellValue(i, j, -1, 0);

            if (_fields[i - 1, j + 1].Stan == false)
                SetCellValue(i, j, -1, 1);

            if (_fields[i, j - 1].Stan == false)
                SetCellValue(i, j, 0, -1);

            if (_fields[i, j + 1].Stan == false)
                SetCellValue(i, j, 0, 1);
        }

        private void UpdateFields()
        {
            for (int i = 0; i < _settings.NetHeight+2; i++)
            {
                for (int j = 0; j < _settings.NetWidth+2; j++)
                {
                    _fields[i, j].Stan = _newFields[i, j].Stan;
                    _fields[i, j].MyRectangle = _newFields[i, j].MyRectangle;
                    _fields[i, j].MyBrush = _newFields[i, j].MyBrush;
                    _fields[i, j].Orientation = _newFields[i, j].Orientation;
                }
            }

           
        }

        private void DrawUpdate()
        {
            ClearBoardView();

            for (int i = 1; i < _settings.NetHeight + 1; i ++)
            {
                for (int j = 1; j < _settings.NetWidth + 1; j ++)
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;
                    
                    r.Fill = _fields[i,j].MyBrush;
                    _fields[i, j].MyRectangle = r;


                    _boardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, (j - 1) * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, (i - 1) * _settings.CellSize);
                    
                }
            }
            
        }

        public void ResetSimulationSettings()
        {
            _timer.Stop();
            ResetFlag = false;
        }

        private void ClearBoardView()
        {
            _boardCanvas.Children.Clear();

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

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)_rand.Next(255), (byte)_rand.Next(255), (byte)_rand.Next(255));
        }

        private void Periodic()
        {
            for (int i = 0; i < _settings.NetHeight+2; i++)
            {
                _fields[i, 0] = _fields[i, _settings.NetWidth ];
                _fields[i, _settings.NetWidth+1] = _fields[i, 1];
                _newFields[i, 0] = _newFields[i, _settings.NetWidth ];
                _newFields[i, _settings.NetWidth+1] = _newFields[i, 1];
            }
            for (int i = 0; i < _settings.NetWidth+2; i++)
            {
                _fields[0, i] = _fields[_settings.NetHeight, i];
                _fields[_settings.NetHeight + 1, i] = _fields[1, i];
                _newFields[0, i] = _newFields[_settings.NetHeight, i];
                _newFields[_settings.NetHeight+1,i] = _newFields[1,i];
            }
        }
    }
}
