using SheetCutter.DataModels;
using SheetCutter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SheetCutter
{
    public partial class MainWindow : Window
    {
        public List<ProductDetail> Details { get; set; }
        public int SheetWidth { get; set; }
        public int SheetHeight { get; set; }

        public MainWindow()
        {
            Details = new List<ProductDetail>()
            {
                // temporary detail set   
                 new ProductDetail(){Count =20, Width =20, Height = 20},
                 new ProductDetail(){Count =10, Width =50, Height = 50},
                 new ProductDetail(){Count =5, Width =150, Height = 50},
                 new ProductDetail(){Count =2, Width =200, Height = 200}
            };
            DataContext = this;
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            RectangleMapper.Width = SheetWidth;
            RectangleMapper.Height = SheetHeight;

            FillSheet();
        }

        private void FillSheet()
        {
            // refresh mapper
            RectangleMapper.Children.Clear();

            // init packer params
            var packer = new ArevaloRectanglePacker(SheetWidth, SheetHeight);
            CalculatePositions(packer);

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

        private void CalculatePositions(ArevaloRectanglePacker packer)
        {
            // get position of each details using algorithm
            foreach (var detail in Details.OrderByDescending(x => x.Height))
            {
                for (int i = 0; i < detail.Count; i++)
                {
                    try
                    {
                        packer.Pack(detail.Width, detail.Height);
                    }
                    catch (OutOfSpaceException ex)
                    {
                        MessageBox.Show(ex.Message);
                        packer.packedRectangles.Clear();
                        return;
                    }
                }
            }
        }
    }
}
