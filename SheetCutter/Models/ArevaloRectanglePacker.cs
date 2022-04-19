using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetCutter.Models
{
    public class ArevaloRectanglePacker : RectanglePacker
    {
        private int actualPackingAreaWidth;
        private int actualPackingAreaHeight;
        public List<Rectangle> packedRectangles;
        private List<Point> anchors;

        public ArevaloRectanglePacker(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
        {
            packedRectangles = new List<Rectangle>();
            anchors = new List<Point> { new Point(0, 0) };

            actualPackingAreaWidth = 1;
            actualPackingAreaHeight = 1;
        }
        public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
        {
            int anchorIndex = SelectAnchorRecursive(rectangleWidth, rectangleHeight, actualPackingAreaWidth, actualPackingAreaHeight);

            if (anchorIndex == -1)
            {
                placement = Point.Empty;
                return false;
            }
            placement = anchors[anchorIndex];
            OptimizePlacement(ref placement, rectangleWidth, rectangleHeight);

            {
                bool blocksAnchor =
                  ((placement.X + rectangleWidth) > anchors[anchorIndex].X) &&
                  ((placement.Y + rectangleHeight) > anchors[anchorIndex].Y);

                if (blocksAnchor)
                    anchors.RemoveAt(anchorIndex);

                InsertAnchor(new Point(placement.X + rectangleWidth, placement.Y));
                InsertAnchor(new Point(placement.X, placement.Y + rectangleHeight));
            }

            packedRectangles.Add(new Rectangle(placement.X, placement.Y, rectangleWidth, rectangleHeight));
            return true;
        }

        private void OptimizePlacement(ref Point placement, int rectangleWidth, int rectangleHeight)
        {
            Rectangle rectangle = new(placement.X, placement.Y, rectangleWidth, rectangleHeight);

            int leftMost = (int)placement.X;
            while (IsFree(ref rectangle, PackingAreaWidth, PackingAreaHeight))
            {
                leftMost = (int)rectangle.X;
                --rectangle.X;
            }

            rectangle.X = placement.X;

            int topMost = (int)placement.Y;
            while (IsFree(ref rectangle, PackingAreaWidth, PackingAreaHeight))
            {
                topMost = (int)rectangle.Y;
                --rectangle.Y;
            }

            if ((placement.X - leftMost) > (placement.Y - topMost))
                placement.X = leftMost;
            else
                placement.Y = topMost;
        }

        private int SelectAnchorRecursive(int rectangleWidth, int rectangleHeight, int testedPackingAreaWidth, int testedPackingAreaHeight)
        {
            int freeAnchorIndex = FindFirstFreeAnchor(rectangleWidth, rectangleHeight, testedPackingAreaWidth, testedPackingAreaHeight);

            if (freeAnchorIndex != -1)
            {
                actualPackingAreaWidth = testedPackingAreaWidth;
                actualPackingAreaHeight = testedPackingAreaHeight;

                return freeAnchorIndex;
            }

            bool canEnlargeWidth = testedPackingAreaWidth < PackingAreaWidth;
            bool canEnlargeHeight = testedPackingAreaHeight < PackingAreaHeight;
            bool shouldEnlargeHeight = (!canEnlargeWidth) || (testedPackingAreaHeight < testedPackingAreaWidth);

            if (canEnlargeHeight && shouldEnlargeHeight)
            {
                return SelectAnchorRecursive(rectangleWidth, rectangleHeight, testedPackingAreaWidth, Math.Min(testedPackingAreaHeight * 2, PackingAreaHeight));
            }
            else if (canEnlargeWidth)
            {
                return SelectAnchorRecursive(rectangleWidth, rectangleHeight, Math.Min(testedPackingAreaWidth * 2, PackingAreaWidth), testedPackingAreaHeight);
            }
            else
            {
                return -1;
            }
        }

        private int FindFirstFreeAnchor(int rectangleWidth, int rectangleHeight, int testedPackingAreaWidth, int testedPackingAreaHeight)
        {
            Rectangle potentialLocation = new(0, 0, rectangleWidth, rectangleHeight);

            for (int index = 0; index < anchors.Count; ++index)
            {
                potentialLocation.X = anchors[index].X;
                potentialLocation.Y = anchors[index].Y;

                if (IsFree(ref potentialLocation, testedPackingAreaWidth, testedPackingAreaHeight))
                    return index;
            }
            return -1;
        }

        private bool IsFree(ref Rectangle rectangle, int testedPackingAreaWidth, int testedPackingAreaHeight)
        {
            bool leavesPackingArea =
              (rectangle.X < 0) ||
              (rectangle.Y < 0) ||
              (rectangle.Right > testedPackingAreaWidth) ||
              (rectangle.Bottom > testedPackingAreaHeight);

            if (leavesPackingArea)
                return false;

            for (int index = 0; index < packedRectangles.Count; ++index)
            {
                //if (this.packedRectangles[index].Intersects(rectangle))
                if (packedRectangles[index].IntersectsWith(rectangle))
                    return false;
            }
            return true;
        }

        private void InsertAnchor(Point anchor)
        {
            int insertIndex = anchors.BinarySearch(anchor, AnchorRankComparer.Default);
            if (insertIndex < 0)
                insertIndex = ~insertIndex;

            anchors.Insert(insertIndex, anchor);
        }

        #region class AnchorRankComparer
        private class AnchorRankComparer : IComparer<Point>
        {
            public static AnchorRankComparer Default = new();

            public int Compare(Point left, Point right)
            {
                return (left.X + left.Y) - (right.X + right.Y);
            }
        }
        #endregion
    }
}
