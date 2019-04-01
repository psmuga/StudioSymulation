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
    /// 
    public partial class MainWindow : Window
    {
        
        private readonly Settings _settings;
        private readonly Controller _controller;

        private GrainGrow _option = GrainGrow.Random;

        private static int addedGrainNumber = 0;

        public MainWindow()
        {
            _settings = new Settings();
            


            InitializeComponent();
            InitializeSettings();

            _controller = new Controller(_settings, BoardCanvas);
            _controller.InitialiseTable();
            

            //InitializeBoard();
        }

        private void InitializeSettings()
        {
            GrainNumbersTextBox.Text = _settings.GrainNumber.ToString();
            NetHightTextBox.Text = _settings.NetHeight.ToString();
            NetWidthTextBox.Text = _settings.NetWidth.ToString();
        }


        //perform simulation
        private void InitializeBoard()
        {
            ClearBoard();

            switch (_option)
            {
                case GrainGrow.Random:
                    _controller.RandomGrain();
                    break;
                case GrainGrow.Even:
                    _controller.EvenGrain();
                    break;
                case GrainGrow.Click:
                   // _controller.PointByClick();
                    break;
                case GrainGrow.Radius:
                    _controller.RandomWithRadius();
                    break;
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

        private void SetCellSpace()
        {
            if (int.TryParse(CellSpaceTextBox.Text, out int cellSpace))
            {
                _settings.CellSpace = cellSpace;
                SimulationTab.IsEnabled = true;
            }
            else
            {
                SimulationTab.IsEnabled = false;
                MessageBox.Show("Wrong numbers of cell space!");

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
            _controller.InitialiseTable();

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

            _controller.InitialiseTable();

            SetResponsiveCanvas();


            if (EvenGrainGrowRadioButton.IsChecked == true)
            {
                SetCellSpace();
                _option = GrainGrow.Even;
            }
            else if (RandomGrainGrowRadioButton.IsChecked ==true)
            {
                _option = GrainGrow.Random;
            }
            else if (ClickPointGrowRadioButton.IsChecked ==true)
            {
                _option = GrainGrow.Click;
            }
            else if (RandomWithRadiusGrowRadioButton.IsChecked == true)
            {
                _option = GrainGrow.Radius;
            }

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


            addedGrainNumber = 0;
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

        private void BoardCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (addedGrainNumber < _settings.GrainNumber && _option==GrainGrow.Click)
            {
                var p = e.GetPosition(BoardCanvas);
                Console.WriteLine($"{p.X} , {p.Y}");

                _controller.PointByClick(p);

                addedGrainNumber++;
            }

            
        }
    }
}
