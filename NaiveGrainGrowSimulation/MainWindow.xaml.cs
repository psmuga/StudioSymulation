using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Microsoft.Win32;

namespace NaiveGrainGrowSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        
        private Settings _settings;
        private Controller _controller;

        

        private static int _addedGrainNumber = 0;
        private bool _allSteps = false;


        public MainWindow()
        {
            _settings = new Settings();
            
            InitializeController();

            InitializeComponent();
            InitializeSettings();

            //InitializeBoard();
        }

        private void InitializeSettings()
        {
            GrainNumbersTextBox.Text = _settings.GrainNumber.ToString();
            NetHightTextBox.Text = _settings.NetHeight.ToString();
            NetWidthTextBox.Text = _settings.NetWidth.ToString();
            RandomWithRadiusTextBox.Text = _settings.Radius.ToString();
            CellSpaceTextBox.Text = _settings.CellSpace.ToString();

            TypeComboBox.SelectedIndex = (int) _settings.NeighborhoodType;
            EdgeComboBox.SelectedIndex = (int) _settings.EdgeCondition;

            switch (_settings.Option)
            {
                case GrainGrow.Random:
                    RandomGrainGrowRadioButton.IsChecked = true;
                    break;
                case GrainGrow.Even:
                    EvenGrainGrowRadioButton.IsChecked = true;
                    break;
                case GrainGrow.Click:
                    ClickPointGrowRadioButton.IsChecked = true;
                    break;
                case GrainGrow.Radius:
                    RandomWithRadiusGrowRadioButton.IsChecked = true;
                    break;
                default:
                    RandomGrainGrowRadioButton.IsChecked = true;
                    break;
            }
        }

        //perform simulation
        private void InitializeBoard()
        {
            ClearBoard();
            InitializeController();

            switch (_settings.Option)
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


        private void InitializeController()
        {
            _controller = new Controller(_settings, BoardCanvas);
            _controller.InitialiseTable();
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

        private void SetRadius()
        {
            if (int.TryParse(RandomWithRadiusTextBox.Text, out int radius))
            {
                _settings.Radius = radius;
                SimulationTab.IsEnabled = true;
            }
            else
            {
                SimulationTab.IsEnabled = false;
                MessageBox.Show("Wrong radius parameter!");
            }
        }

        private void SetEdgeType()
        {
            _settings.EdgeCondition = (EdgeCondition) EdgeComboBox.SelectionBoxItem;
        }

        private void SetNeighborhoodType()
        {
            _settings.NeighborhoodType = (NeighborhoodType) TypeComboBox.SelectedIndex;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
           
           // _controller.InitialiseTable();
            InitializeBoard();

            PauseButton.IsEnabled = !PauseButton.IsEnabled;
            StartButton.IsEnabled = !StartButton.IsEnabled;
            ClearButton.IsEnabled = false;
            FitButton.IsEnabled = false;

          //  _controller.PerformMooreIteration();
        }

        //save button from settings
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearButton_Click(sender,e);

            SetNetHeight();
            SetNetWidth();
            SetNumbersOfGrains();
            SetRadius();
            SetEdgeType();
            SetNeighborhoodType();

           // _controller.InitialiseTable();

            SetResponsiveCanvas();

            
            SetStepOption();

            if (EvenGrainGrowRadioButton.IsChecked == true)
            {
                SetCellSpace();
                _settings.Option = GrainGrow.Even;
            }
            else if (RandomGrainGrowRadioButton.IsChecked ==true)
            {
                _settings.Option = GrainGrow.Random;
            }
            else if (ClickPointGrowRadioButton.IsChecked ==true)
            {
                _settings.Option = GrainGrow.Click;
            }
            else if (RandomWithRadiusGrowRadioButton.IsChecked == true)
            {
                _settings.Option = GrainGrow.Radius;
            }

        }

        private void SetStepOption()
        {
            _allSteps = AllStepRadioButton.IsChecked == true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = !PauseButton.IsEnabled;
            ClearButton.IsEnabled = true;

            _controller.ResetFlag = !_controller.ResetFlag;
            _allSteps = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            //_controller.ResetTable();
            FitButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;

            _addedGrainNumber = 0;
            
            ClearBoard();
            SetStepOption();
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
            if (_addedGrainNumber < _settings.GrainNumber && _settings.Option==GrainGrow.Click)
            {
                var p = e.GetPosition(BoardCanvas);

                _controller.PointByClick(p);

                _addedGrainNumber++;
            }
        }

        private void TypeComboBox_Initialized(object sender, EventArgs e)
        {
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(NeighborhoodType)).Cast<NeighborhoodType>();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _controller.PerformIterations(_allSteps);
        }

        private void EdgeComboBox_Initialized(object sender, EventArgs e)
        {
            EdgeComboBox.ItemsSource = Enum.GetValues(typeof(EdgeCondition)).Cast<EdgeCondition>();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PNG Image (*.png) |*.png |All files |*.*",
                RestoreDirectory = true
            };

            var size = BoardCanvas.RenderSize;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int) size.Height,96d,96d,PixelFormats.Pbgra32);
            BoardCanvas.Measure(size);
            BoardCanvas.Arrange(new Rect(size));
            rtb.Render(BoardCanvas);
            var pngEncoderr = new PngBitmapEncoder();
            pngEncoderr.Frames.Add(BitmapFrame.Create(rtb));
            
            MemoryStream ms;
            using ( ms = new MemoryStream())
            {
                pngEncoderr.Save(ms);
            }

            if (dialog.ShowDialog() == true)
            {
                if (dialog.FilterIndex ==1)
                {
                    File.WriteAllBytes(dialog.FileName, ms.ToArray());
                }


            }
        }

        private void SaveSettingsAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Settings (*.json) |*.json |Structure (*.json) |*.json |All files |*.*",
                
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.FilterIndex == 1)
                {
                    var jsonFormatter = new JsonFormatter(_settings);
                    File.WriteAllText(dialog.FileName, jsonFormatter.Serialize());
                }
                else if (dialog.FilterIndex ==2)
                {
                    var dupa = new int[3,4]
                    {
                        {1,2,3,4},
                        { 5,6,7,10},
                        { 8,9,0,11}
                    };
                    var jsonString = JsonFormatter.Serialize(_controller.GetStructure());
                    File.WriteAllText(dialog.FileName, jsonString);
                }
            }
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Json file (*.json) |*.json |All files |*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = true,
                FilterIndex = 2
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string text = string.Empty;
                    using (var sr = new StreamReader(dialog.FileName))
                    {
                        text = sr.ReadToEnd();
                    }
                    ///TODO validate input settings
                    _settings = JsonFormatter.Deserialize(text);

                    InitializeSettings();
                    //SetResponsiveCanvas();
                    
                    Button_Click(sender,e);

                }
                catch (Exception)
                {
                    MessageBox.Show($"Wrong file");
                }
            }

        }


    }
}
