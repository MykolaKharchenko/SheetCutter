using System;
using System.Drawing;

namespace SheetCutter.Models
{
    public abstract class RectanglePacker
    {
        private int packingAreaWidth;
        private int packingAreaHeight;

        protected int PackingAreaWidth => packingAreaWidth;

        protected int PackingAreaHeight => packingAreaHeight;

        /// <summary>Initializes a new rectangle packer</summary>                                     
        /// <param name="packingAreaWidth">Width of the packing area</param>                           
        /// <param name="packingAreaHeight">Height of the packing area</param>                       
        protected RectanglePacker(int packingAreaWidth, int packingAreaHeight)
        {
            this.packingAreaWidth = packingAreaWidth;
            this.packingAreaHeight = packingAreaHeight;
        }

        /// <summary>Allocates space for a rectangle in the packing area</summary>   
        /// <param name="rectangleWidth">Width of the rectangle to allocate</param>                 
        /// <param name="rectangleHeight">Height of the rectangle to allocate</param>                  
        /// <returns>The location at which the rectangle has been placed</returns>                 
        public virtual Point Pack(int rectangleWidth, int rectangleHeight)
        {
            if (!TryPack(rectangleWidth, rectangleHeight, out Point point))
            {
                System.Windows.MessageBox.Show("it is impossible to place details");
                return Point.Empty;
                //throw new OutOfSpaceException("Rectangle does not fit in packing area"); 
            }

            return point;
        }

        /// <summary>Tries to allocate space for a rectangle in the packing area</summary>            
        /// <param name="rectangleWidth">Width of the rectangle to allocate</param>              
        /// <param name="rectangleHeight">Height of the rectangle to allocate</param>               
        /// <param name="placement">Output parameter receiving the rectangle's placement</param>      
        /// <returns>True if space for the rectangle could be allocated</returns>                   
        public abstract bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement);
    }

    public class OutOfSpaceException : Exception
    {
        public OutOfSpaceException() { }

        /// <summary>Initializes the exception with an error message</summary>
        /// <param name="message">Error message describing the cause of the exception</param>
        public OutOfSpaceException(string message) : base(message) { }

        /// <summary>Initializes the exception as a followup exception</summary>
        /// <param name="message">Error message describing the cause of the exception</param>
        /// <param name="inner">Preceding exception that has caused this exception</param>
        public OutOfSpaceException(string message, Exception inner) : base(message, inner) { }
    }
}
