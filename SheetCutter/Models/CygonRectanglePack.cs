using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetCutter.Models
{
    public class CygonRectanglePack : RectanglePacker
    {
        #region class SliceStartComparer
        private class SliceStartComparer : IComparer<Point>
        {
            public static SliceStartComparer Default = new();
            public int Compare(Point left, Point right)
            {
                return left.X - right.X;
            }
        }
        #endregion

        public List<Point> heightSlices;

        public CygonRectanglePack(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
        {
            heightSlices = new List<Point> { new Point(0, 0) };
        }

        public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
        {
            if ((rectangleWidth > PackingAreaWidth) || (rectangleHeight > PackingAreaHeight))
            {
                placement = Point.Empty;
                return false;
            }

            bool fits = TryFindBestPlacement(rectangleWidth, rectangleHeight, out placement);

            if (fits)
            {
                IntegrateRectangle(placement.X, rectangleWidth, placement.Y + rectangleHeight);
            }
            return fits;
        }

        private bool TryFindBestPlacement(int rectangleWidth, int rectangleHeight, out Point placement)
        {
            int bestSliceIndex = -1;
            int bestSliceY = 0;
            int bestScore = PackingAreaHeight;

            int leftSliceIndex = 0;

            int rightSliceIndex = heightSlices.BinarySearch(new Point(rectangleWidth, 0), SliceStartComparer.Default);
            if (rightSliceIndex < 0)
                rightSliceIndex = ~rightSliceIndex;

            while (rightSliceIndex <= heightSlices.Count)
            {
                int highest = heightSlices[leftSliceIndex].Y;
                for (int index = leftSliceIndex + 1; index < rightSliceIndex; ++index)
                    if (heightSlices[index].Y > highest)
                        highest = heightSlices[index].Y;

                if ((highest + rectangleHeight <= PackingAreaHeight))
                {
                    int score = highest;

                    if (score < bestScore)
                    {
                        bestSliceIndex = leftSliceIndex;
                        bestSliceY = highest;
                        bestScore = score;
                    }
                }

                ++leftSliceIndex;
                if (leftSliceIndex >= heightSlices.Count)
                    break;

                int rightRectangleEnd = heightSlices[leftSliceIndex].X + rectangleWidth;
                for (; rightSliceIndex <= heightSlices.Count; ++rightSliceIndex)
                {
                    int rightSliceStart;
                    if (rightSliceIndex == heightSlices.Count)
                        rightSliceStart = PackingAreaWidth;
                    else
                        rightSliceStart = heightSlices[rightSliceIndex].X;

                    if (rightSliceStart > rightRectangleEnd)
                        break;
                }

                if (rightSliceIndex > heightSlices.Count)
                    break;
            }

            if (bestSliceIndex == -1)
            {
                placement = Point.Empty;
                return false;
            }
            else
            {
                placement = new Point(heightSlices[bestSliceIndex].X, bestSliceY);
                return true;
            }
        }

        private void IntegrateRectangle(int left, int width, int bottom)
        {
            int startSlice = heightSlices.BinarySearch(new Point(left, 0), SliceStartComparer.Default);
            int firstSliceOriginalHeight;


            firstSliceOriginalHeight = heightSlices[startSlice].Y;
            heightSlices[startSlice] = new Point(left, bottom);

            int right = left + width;
            ++startSlice;

            if (startSlice >= heightSlices.Count)
            {
                if (right < PackingAreaWidth)
                    heightSlices.Add(new Point(right, firstSliceOriginalHeight));
            }
            else
            {
                int endSlice = heightSlices.BinarySearch(startSlice, heightSlices.Count - startSlice, new Point(right, 0), SliceStartComparer.Default);

                if (endSlice > 0)
                {
                    heightSlices.RemoveRange(startSlice, endSlice - startSlice);
                }
                else
                {
                    endSlice = ~endSlice;
                    int returnHeight;
                    if (endSlice == startSlice)
                        returnHeight = firstSliceOriginalHeight;
                    else
                        returnHeight = heightSlices[endSlice - 1].Y;

                    heightSlices.RemoveRange(startSlice, endSlice - startSlice);
                    if (right < PackingAreaWidth)
                        heightSlices.Insert(startSlice, new Point(right, returnHeight));
                }
            }
        }
    }
}
