namespace SheetCutter.DataModels
{
    public class Detail : NotifyPropertyChanged
    {
        private byte count;     
        public byte Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
    }
}
