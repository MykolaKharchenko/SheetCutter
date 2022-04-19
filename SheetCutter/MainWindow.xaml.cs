using SheetCutter.DataModels;
using SheetCutter.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SheetCutter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Detail> Collection { get; set; }
        public MainWindow()
        {
            Collection = new ObservableCollection<Detail>()
            {
                 new Detail(){Count =1, Width =15, Height = 20},
                 new Detail(){Count =1, Width =50, Height = 60}
            };
            DataContext = this;
            InitializeComponent();
        }

        private void CreateRectangle(ObservableCollection<Detail> details)
        {
            var rec1 = new ArevaloRectanglePacker((int)RectangleMapper.ActualWidth, (int)RectangleMapper.ActualHeight);

            foreach (var detail in details)
            {
                rec1.Pack(detail.Width, detail.Height);
            }

            foreach (var rect in rec1.packedRectangles)
            {
                var rec = new Rectangle()
                {
                    Height = rect.Height,
                    Width = rect.Width,
                    StrokeThickness = 4,
                    Fill = new SolidColorBrush(Colors.Blue),
                    Stroke = new SolidColorBrush(Colors.Black),
                };
                Canvas.SetLeft(rec, rect.X);
                Canvas.SetTop(rec, rect.Y);

                RectangleMapper.Children.Add(rec);
            }

            #region old implement

            //Rectangle newRect = new()
            //{
            //    Height = 100,
            //    Width = 200,
            //    StrokeThickness = 4,
            //    Fill = new SolidColorBrush(Colors.Blue),
            //    Stroke = new SolidColorBrush(Colors.Black),

            //    //  Canvas.Left="0" Canvas.Bottom="0"  
            //};
            //Canvas.SetLeft(newRect,50);
            //Canvas.SetTop(newRect, 50);
            //RectangleMapper.Children.Add(newRect);

            //Rectangle newRect2 = new()
            //{
            //    Height = 200,
            //    Width = 100,
            //    StrokeThickness = 4,
            //    Fill = new SolidColorBrush(Colors.Blue),
            //    Stroke = new SolidColorBrush(Colors.Black),

            //    //  Canvas.Left="0" Canvas.Bottom="0"  
            //};
            //Canvas.SetLeft(newRect2, 10);
            //Canvas.SetTop(newRect2, 10);
            //RectangleMapper.Children.Add(newRect2);


            #endregion
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            //var i = Collection[0];      //   checking catch collection
            CreateRectangle(Collection);
        }
    }
}
