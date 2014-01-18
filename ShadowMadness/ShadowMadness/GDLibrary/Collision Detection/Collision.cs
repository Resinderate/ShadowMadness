using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ShadowMadness;

namespace GDLibrary
{
    public class Collision
    {
        //class method Per-pixel CD
        //comment - add property - 7.10.13 - nmcg
        private static int alphaThreshold = 10;
        //Four sectors of the screen - we can increase to 2^N sectors to reduce number of bounding box collision tests.
        private static List<BSPCollisionSectorInfo> collisionSectors;// topLeft, topRight, bottomLeft, bottomRight;

        //temp variables
        private static Vector2 leftTop, rightTop, leftBottom, rightBottom, min, max;
        private static Vector2 pos1 = Vector2.Zero, pos2 = Vector2.Zero;
        private static Matrix mat1to2;
        private static int x2 = 0, y2 = 0, widthA, heightA, widthB, heightB;

        #region BOUNDING RECTANGLE CDCR
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }
        public static bool Touches(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }

        public static bool TouchesEdgeOnly(Rectangle a, Rectangle b)
        {
            return (a.Right == b.Left || a.Left == b.Right ||
                    a.Bottom == b.Top || a.Top == b.Bottom);
        }

        /// <summary>
        /// Simple BSP + Bounding Box intersection method
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Intersects(Sprite a, Sprite b)
        {
            if (isBSPCollision(a.POSITIONINFO.SECTORNUMBER, b.POSITIONINFO.SECTORNUMBER))
            {
                if (Collision.Intersects(a.POSITIONINFO.BOUNDS, b.POSITIONINFO.BOUNDS))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Intersects(SpriteShell a, Sprite b)
        {
            if (isBSPCollision(a.sectorNumber, b.POSITIONINFO.SECTORNUMBER))
            {
                if (Collision.Intersects(a.bounds, b.POSITIONINFO.BOUNDS))
                {
                    return true;
                }
            }
            return false;
        }
        
        #endregion
         

        #region Non AA CDCR
        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateTransformedBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            //   Matrix inverseMatrix = Matrix.Invert(transform);
            // Get all four corners in local space
            leftTop = new Vector2(rectangle.Left, rectangle.Top);
            rightTop = new Vector2(rectangle.Right, rectangle.Top);
            leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)Math.Round(min.X), (int)Math.Round(min.Y),
                                 (int)Math.Round(max.X - min.X), (int)Math.Round(max.Y - min.Y));
        }
 
        //http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Coll_Detection_Overview.php
        public static Vector2 IntersectsNonAA(Sprite a, Sprite b)
        {
            if (Intersects(a, b)) //BSP + Bounding Box
            {
                mat1to2 = a.POSITIONINFO.WORLDMATRIX * b.POSITIONINFO.INVERSEWORLDMATRIX;
                //widthA = a.TEXTUREDATA.Width();
                //heightA = a.TEXTUREDATA.Height();
                //widthB = b.TEXTUREDATA.Width();
                //heightB = b.TEXTUREDATA.Height();
                widthA = a.PRESENTATIONINFO.SOURCERECTANGLE.Width;
                heightA = a.PRESENTATIONINFO.SOURCERECTANGLE.Height;
                widthB = b.PRESENTATIONINFO.SOURCERECTANGLE.Width;
                heightB = b.PRESENTATIONINFO.SOURCERECTANGLE.Height;

                //loop for width of A
                for (int x1 = 0; x1 < widthA; x1++)
                {
                    //loop of height of A
                    for (int y1 = 0; y1 < heightA; y1++)
                    {
                        //(0, 0) to (a.w, a.h)
                        pos1 = new Vector2(x1, y1);
                        //corresponding b value
                        pos2 = Vector2.Transform(pos1, mat1to2);
                        x2 = (int)Math.Round(pos2.X);
                        y2 = (int)Math.Round(pos2.Y);

                        //check if within b bounds.
                        if (((x2 >= 0) && (x2 < widthB))
                            && ((y2 >= 0) && (y2 < heightB)))
                        {
                            //
                            if (a.TEXTUREDATA.TEXTURECOLORDATA2D != null && b.TEXTUREDATA.TEXTURECOLORDATA2D != null && x2 < b.TEXTUREDATA.TEXTURECOLORDATA2D.GetLength(0))
                            {
                                if ((a.TEXTUREDATA.TEXTURECOLORDATA2D[x1, y1].A > alphaThreshold) && (b.TEXTUREDATA.TEXTURECOLORDATA2D[x2, y2].A > alphaThreshold))
                                {
                                    return Vector2.Transform(pos1, a.POSITIONINFO.WORLDMATRIX);
                                }
                            }
                        }
                    }
                } //Non AA CDCR
            }
            return new Vector2(-1, -1);
        }
        #endregion

        #region BSP
        /// <summary>
        /// Creates the sectors which divide the screen space e.g. 4, 8, 9, 16, 25, 32
        /// </summary>
        /// <param name="sectorLayout"></param>
        public static void setCollisionSectors(BSPCollisionSectorLayout sectorLayout)
        {
            collisionSectors = new List<BSPCollisionSectorInfo>(sectorLayout.SectorCount);

            for (int row = 0; row < sectorLayout.Rows; row++)
            {
                for (int col = 0; col < sectorLayout.Columns; col++)
                {
                    collisionSectors.Add(new BSPCollisionSectorInfo(row, col,
                                        sectorLayout.SectorWidth, sectorLayout.SectorHeight));
                }
            }
        }

        /// <summary>
        /// Returns true if two sprites are in the same sector
        /// </summary>
        /// <param name="sectorNumberA"></param>
        /// <param name="sectorNumberB"></param>
        /// <returns></returns>
        private static bool isBSPCollision(int sectorNumberA, int sectorNumberB)
        {
            return ((sectorNumberA & sectorNumberB) > 0);
        }

        /// <summary>
        /// Returns the sector number for a sprite with the specified spritebounds rectangle
        /// </summary>
        /// <param name="spriteBounds"></param>
        /// <returns></returns>
        public static int getSectorNumber(Rectangle spriteBounds)
        {
            int bSectorNumber = 0;

            foreach (BSPCollisionSectorInfo collisionSectorInfo in collisionSectors)
            {
                if (collisionSectorInfo.SectorBounds.Intersects(spriteBounds))
                {
                    bSectorNumber += collisionSectorInfo.SectorNumber;
                }
            }
            return bSectorNumber;
        }

        #endregion

        #region LinesAndShit
        public static bool Intersects(Rectangle rect, Line line)
        {
            List<Line> lines = Line.lineFactory(rect);
            for (int i = 0; i < lines.Count; i++)
            {
                if (Intersects(lines[i], line))
                    return true;
            }
            return false;
        }

        public static bool Intersects(Line line1, Line line2)
        {
            //bool intersection = false;
            //bool coincident = false;

            Vector2 b = line1.end - line1.start;
            Vector2 d = line2.end - line2.start;

            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = line2.start - line1.start;
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }
        
        #endregion
    }
}
