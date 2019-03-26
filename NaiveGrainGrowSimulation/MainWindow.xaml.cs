using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaiveGrainGrowSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Settings _settings;
        private readonly Random _rand;
        private Cell[,] _fields;


        public MainWindow()
        {
            _settings = new Settings();
            _rand = new Random();

            InitializeComponent();
            InitializeSettings();
            InitialiseTable();
            

            //InitializeBoard();
        }

        private void InitializeSettings()
        {
            GrainNumbersTextBox.Text = _settings.GrainNumber.ToString();
            NetHightTextBox.Text = _settings.NetHeight.ToString();
            NetWidthTextBox.Text = _settings.NetWidth.ToString();
        }

        private void InitialiseTable()
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
        //perform simulation
        private void InitializeBoard()
        {
            

            ClearBoard();

            FirstColorSystem();
        }

        //do usunięcia
        private void FirstColorSystem()
        {
            var index = 0;

            while (index<_settings.GrainNumber)
            {
                var i = _rand.Next(_settings.NetHeight - 1);
                var j = _rand.Next(_settings.NetWidth - 1);
                if (_fields[i,j].Stan ==false)
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;
                    var color = new SolidColorBrush(GetRandomColor());
                    r.Fill = color;
                    _fields[i, j].MyRectangle = r;// SetRectangle(r);
                    _fields[i, j].MyBrush = color;


                    BoardCanvas.Children.Add(_fields[i, j].MyRectangle);
                    Canvas.SetLeft(_fields[i, j].MyRectangle, j * _settings.CellSize);
                    Canvas.SetTop(_fields[i, j].MyRectangle, i * _settings.CellSize);
                    index++;
                    _fields[i, j].Stan = true;
                }
            }
        }


        private void SetResponsiveCanvas()
        {
            int h = (int) RowDefinitionCanvas.ActualHeight;
            int w = (int) (MyWindow.ActualWidth-20);

            int size = 0;
            if (h<w)
            {
                size = h / _settings.NetHeight;
                do
                {
                    size--;
                } while (size * _settings.NetWidth > (MyWindow.ActualWidth-40));
            }
            else
            {
                size = w / _settings.NetWidth;

                do
                {
                    size--;
                } while ((size* _settings.NetHeight) > RowDefinitionCanvas.ActualHeight-20);

            }
            BoardCanvas.Height = size * _settings.NetHeight;
            BoardCanvas.Width = size * _settings.NetWidth;
            _settings.CellSize = size;
        }

        private void SetNumbersOfGrains()
        {
            if (int.TryParse(GrainNumbersTextBox.Text, out int inputGrains))
            {
                _settings.GrainNumber = inputGrains;
                SimulationTab.IsEnabled = true;
            }
            else
            {
                SimulationTab.IsEnabled = false;
                MessageBox.Show("Wrong numbers of grains!");
                
            }
        }

        private void SetNetHeight()
        {
            if (int.TryParse(NetHightTextBox.Text, out int high))
            {
                _settings.NetHeight = high;
                SimulationTab.IsEnabled = true;
            }
            else
            {
                SimulationTab.IsEnabled = false;
                MessageBox.Show("Wrong net height parameter!");
            }
        }

        private void SetNetWidth()
        {
            if (int.TryParse(NetWidthTextBox.Text, out int width))
            {
                _settings.NetWidth = width;
                SimulationTab.IsEnabled = true;
            }
            else
            {
                SimulationTab.IsEnabled = false;
                MessageBox.Show("Wrong net width parameter!");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeBoard();
            InitialiseTable();

            PauseButton.IsEnabled = !PauseButton.IsEnabled;
            StartButton.IsEnabled = !StartButton.IsEnabled;
            ClearButton.IsEnabled = false;
            FitButton.IsEnabled = false;
        }

        //save button from settings
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearButton_Click(sender,e);

            SetNetHeight();
            SetNetWidth();
            SetNumbersOfGrains();

            InitialiseTable();

            SetResponsiveCanvas();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = !PauseButton.IsEnabled;
            ClearButton.IsEnabled = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FitButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;

            ClearBoard();
            
        }

        private void FitButton_Click(object sender, RoutedEventArgs e)
        {
            SetResponsiveCanvas();
        }

        private void ClearBoard()
        {

            BoardCanvas.Children.Clear();

        }


        private void BoardCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetResponsiveCanvas();
        }

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte) _rand.Next(255),(byte) _rand.Next(255), (byte) _rand.Next(255));
        }
    }
}
