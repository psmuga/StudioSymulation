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
        private List<Brush> _idUsedColors;
        private List<Brush> _usedColors;
        private Rectangle[,] _fields;
        private Brush[,] _tempBrushes;

        public MainWindow()
        {
            _settings = new Settings();
            _rand = new Random();

            InitializeComponent();
            InitializeSettings();

            

            //InitializeBoard();
        }

        private void InitializeSettings()
        {
            GrainNumbersTextBox.Text = _settings.GrainNumber.ToString();
            NetHightTextBox.Text = _settings.NetHeight.ToString();
            NetWidthTextBox.Text = _settings.NetWidth.ToString();
        }

        private void InitializeBoard()
        {
            _idUsedColors = new List<Brush>();
            _usedColors = new List<Brush>
            {
                Brushes.White,
                Brushes.Cyan,
                Brushes.Black
            };

            BoardCanvas.Children.Clear();

            for (var i = 0; i < _settings.GrainNumber; i++)
            {
                _idUsedColors.Add(GetRandomBrush());
            }

            _fields = new Rectangle[_settings.NetHeight,_settings.NetWidth];
            _tempBrushes = new Brush[_settings.NetHeight, _settings.NetWidth];


            FirstColorSystem();


        }

        //do usunięcia
        private void FirstColorSystem()
        {
            var brushesType = typeof(Brushes);
            var properties = brushesType.GetProperties();

            for (int i = 0; i < _settings.NetHeight; i++)
            {
                for (int j = 0; j < _settings.NetWidth; j++)
                {
                    var r = new Rectangle();
                    r.Width = _settings.CellSize;
                    r.Height = _settings.CellSize;

                    var tempId = _rand.Next(_settings.GrainNumber);
                    r.Fill = (Brush)properties[tempId].GetValue(null, null);
                    BoardCanvas.Children.Add(r);
                    Canvas.SetLeft(r, j * _settings.CellSize);
                    Canvas.SetTop(r, i * _settings.CellSize);
                    _fields[i, j] = r;
                }
            }
        }

        private Brush GetRandomBrush()
        {
            var properties = typeof(Brushes).GetProperties();
            while (true)
            {
                var random = _rand.Next(properties.Length);
                var result = (Brush) properties[random].GetValue(null, null);
                if (!_usedColors.Contains(result))
                {
                    return result;
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
            //SolidColorBrush mycolor = new SolidColorBrush();
            //mycolor.Color = Color.FromRgb(229,229,229);
            BoardCanvas.Children.Clear();
        }

        private void BoardCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetResponsiveCanvas();
        }
    }
}
