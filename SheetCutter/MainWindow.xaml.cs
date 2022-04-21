using SheetCutter.DataModels;
using SheetCutter.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SheetCutter
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Detail> Details { get; set; }
        public int SheetWidth { get; set; }
        public int SheetHeight { get; set; }

        public MainWindow()
        {
            Details = new ObservableCollection<Detail>()
            {
                // temporary detail set   
                 new Detail(){Count =20, Width =20, Height = 20},
                 new Detail(){Count =10, Width =50, Height = 50},
                 new Detail(){Count =5, Width =150, Height = 50},
                 new Detail(){Count =2, Width =200, Height = 200}
            };
            DataContext = this;
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            RectangleMapper.Width = SheetWidth;
            RectangleMapper.Height = SheetHeight;
            RectangleMapper.RenderSize = new Size(SheetWidth, SheetHeight);

            FillSheet(Details);
        }

        private void FillSheet(ObservableCollection<Detail> details)
        {
            // refresh mapper
            RectangleMapper.Children.Clear();

            // init packer params
            var packer = new ArevaloRectanglePacker((int)RectangleMapper.ActualWidth, (int)RectangleMapper.ActualHeight);
            CalculatePositions(details, packer);

            //  locate details in mapper
            foreach (var rectangleData in packer.packedRectangles)
            {
                var rectangleShape = new Rectangle()
                {
                    Height = rectangleData.Height,
                    Width = rectangleData.Width,
                };
                Canvas.SetLeft(rectangleShape, rectangleData.X);
                Canvas.SetTop(rectangleShape, rectangleData.Y);

                RectangleMapper.Children.Add(rectangleShape);
            }
        }

        private void CalculatePositions(ObservableCollection<Detail> _details, ArevaloRectanglePacker _packer)
        {
            // get location of each details using algorithm
            foreach (var detail in _details.OrderByDescending(x => x.Height))
            {
                for (int i = 0; i < detail.Count; i++)
                {
                    try
                    {
                        _packer.Pack(detail.Width, detail.Height);
                    }
                    catch (OutOfSpaceException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}
