using SheetCutter.DataModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SheetCutter.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<Detail> details;
        public ObservableCollection<Detail> Details
        {
            get
            { return details; }
            set
            {
                details = value;
                OnPropertyChanged(nameof(Details));
            }
        }

        private Detail updatedDetail;
        public Detail UpdatedDetail
        {
            get
            { return updatedDetail; }
            set
            {
                updatedDetail = value;
                OnPropertyChanged(nameof(UpdatedDetail));
            }
        }

        private RelayCommand calculateCommand;
        public RelayCommand CalculateCommand => calculateCommand ??= new RelayCommand(CalculateCommandExecute, CalculateCommandCanExecute());

        public MainWindowViewModel()
        {
            Details = new ObservableCollection<Detail>();
        }

        private void CalculateCommandExecute()
        {
            var s = Details;
        }

        private Func<object, bool> CalculateCommandCanExecute()
        {
            return null;
        }

        public void CreateRectangle()
        {
            // Create a Rectangle  
            Rectangle blueRectangle = new()
            {
                Height = 100,
                Width = 200
            };
            // Create a blue and a black Brush  
            SolidColorBrush blueBrush = new()
            {
                Color = Colors.Blue
            };
            SolidColorBrush blackBrush = new()
            {
                Color = Colors.Black
            };
            // Set Rectangle's width and color  
            blueRectangle.StrokeThickness = 4;
            blueRectangle.Stroke = blackBrush;
            blueRectangle.Fill = blueBrush;
        }
    }
}
