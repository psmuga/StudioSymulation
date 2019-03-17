using System;
using System.Collections.Generic;
using System.Linq;
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
        private Random _rand;
        private List<Brush> _idUsedColors;
        private List<Brush> _usedColors;

        public MainWindow()
        {
            _settings = new Settings();
            _rand = new Random();

            InitializeComponent();
            InitializeSettings();

            

            InitializeBoard();
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
                Brushes.Cyan
            };

            BoardCanvas.Children.Clear();

            for (int i = 0; i < _settings.GrainNumber; i++)
            {
                _idUsedColors.Add(GetRandomBrush());
            }

            //SetResponsiveCanvas();


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
           //skrajne wartośći 10/50 np wyrzuca poza ekran

            int h = (int) RowDefinitionCanvas.ActualHeight;
            int w = (int) (MyWindow.ActualWidth-20);

            int size = 0;
            if (h<w)
            {
                size = h / _settings.NetHeight;
                do
                {
                    size--;
                } while (size * _settings.NetWidth > (MyWindow.ActualWidth-30));
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

        private void GrainNumbersTextBox_TextChanged(object sender, TextChangedEventArgs e) => SetNumbersOfGrains();

        private void NetWidthTextBox_TextChanged(object sender, TextChangedEventArgs e) => SetNetWidth();

        private void NetHightTextBox_TextChanged(object sender, TextChangedEventArgs e) => SetNetHeight();

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SetResponsiveCanvas();
        }

        //save button from settings
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetResponsiveCanvas();
        }
    }
}
