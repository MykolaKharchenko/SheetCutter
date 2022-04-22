namespace SheetCutter.DataModels
{
    public class ProductDetail
    {
        private byte count;
        public byte Count
        {
            get { return count; }
            set { count = value; }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}
