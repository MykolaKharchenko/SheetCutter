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
        public ObservableCollection<Detail> Collection { get; set; }
        public MainWindow()
        {
            Collection = new ObservableCollection<Detail>()
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

        private void FillMapper(ObservableCollection<Detail> details)
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
            // locate each detail in packer  algorithm
            foreach (var detail in _details.OrderByDescending(x => x.Height))
            {
                for (int i = 0; i < detail.Count; i++)
                {
                    _packer.Pack(detail.Width, detail.Height);
                }
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            FillMapper(Collection);
        }
    }
}
