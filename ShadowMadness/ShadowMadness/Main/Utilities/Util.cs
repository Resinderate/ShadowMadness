/*
 *  Ronan Murphy - Last edited 31/12/2013 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDLibrary;

namespace ShadowMadness
{
    public class Util
    {
        
        /// <summary>
        /// Pass in the parametres that describe the shadow you want, and it
        /// will return a List of Vector2 describing the shadow generated.
        /// (The list can be drawn with the Draw2D class).
        /// </summary>
        /// <param name="rect">The rectangle that will cast the shadow.</param>
        /// <param name="source">The point where the light is shining from.</param>
        /// <param name="windowSize">The minimum size of the space you want to cover (Typically the window size).</param>
        /// <returns></returns>
        public static List<Vector2> shadowFactory(Rectangle rect, Vector2 source, Vector2 windowSize)
        {
            List<Vector2> lines;
            int shadowType = findRelativeShadowType(rect, source);
            int side = findShadowSide(shadowType, rect, source);

            lines = createShadow(shadowType, side, rect, source, windowSize);

            return lines;
        }

        /// <summary>
        /// This is similar to the above, except that instead of returning a graphical
        /// representation of a shadow, it will return 2 lines that act as 'contact lines'
        /// for the shadow generated. If you can picture the 2 edges of a shadow around a block,
        /// these are the lines returned.
        /// </summary>
        /// <param name="rect">Rectangle casting the shadow.</param>
        /// <param name="source">The source of the light.</param>
        /// <param name="windowSize">The minimum size of the space you want to cover (Typically the window size)</param>
        /// <returns></returns>
        public static List<Line> contactLineFactory(Rectangle rect, Vector2 source, Vector2 windowSize)
        {
            List<Line> lines;
            int shadowType = findRelativeShadowType(rect, source);
            int side = findShadowSide(shadowType, rect, source);

            lines = createContactLines(shadowType, side, rect, source, windowSize);

            return lines;
        }

        
        /// <summary>
        /// Finds if a position relative to a rectangle is either:
        /// 1 - inside the rect - thus not producing a shadow
        /// 2 - to the side or top, (not diagonal)
        /// 3 - to the corner side, (diagonal)
        /// </summary>
        /// <param name="rect">Rectangle casting the shadow</param>
        /// <param name="source">Source of the light.</param>
        /// <returns></returns>
        public static int findRelativeShadowType(Rectangle rect, Vector2 source)
        {
            if (rect.Contains((int)source.X, (int)source.Y))
                return 1;
            else if ((source.X >= rect.Left && source.X <= rect.Right) || (source.Y >= rect.Top && source.Y <= rect.Bottom))
                return 2;
            else
                return 3;
        }

        /// <summary>
        /// Finds what side the point is at relative to the rect.
        /// This is described with both "types" in mind.
        /// It starts with 0 being either on top / or to the
        /// top left diagonally and works clockwise with each zone.
        /// </summary>
        /// <param name="type">The type of shadow</param>
        /// <param name="rect">Rect casting the shadow</param>
        /// <param name="source">Source of the light</param>
        /// <returns></returns>
        public static int findShadowSide(int type, Rectangle rect, Vector2 source)
        {
            //side rectangle
            if (type == 2)
            {
                if (source.Y < rect.Top)
                    return 0;
                else if (source.X > rect.Right)
                    return 1;
                else if (source.Y > rect.Bottom)
                    return 2;
                else if (source.X < rect.Left)
                    return 3;
            }
            //diagonal corner
            else if (type == 3)
            {
                if(source.X < rect.Left) //Left side
                {
                    if (source.Y < rect.Top)
                        return 0;
                    else if (source.Y > rect.Bottom)
                        return 3;
                }
                else if (source.X > rect.Right) //Right side
                {
                    if (source.Y < rect.Top)
                        return 1;
                    else if (source.Y > rect.Bottom)
                        return 2;
                }
            }
            //something wrong
            return 0;
        }

        /// <summary>
        /// Given all the neccesary parametres it generates the shadow.
        /// Based on performing the previous check of the 
        /// side and type of shadow to be generated.
        /// </summary>
        /// <param name="type">Type of shadow</param>
        /// <param name="side">Relative side of the source</param>
        /// <param name="rect">Rect casting the shadow</param>
        /// <param name="source">Source of the light</param>
        /// <param name="windowSize">The minimum size of the space you want to cover (Typically the window size)</param>
        /// <returns></returns>
        public static List<Vector2> createShadow(int type, int side, Rectangle rect, Vector2 source, Vector2 windowSize)
        {
            List<Vector2> shadowVerts = new List<Vector2>();

            Queue<Vector2> cornerQueue = new Queue<Vector2>();
            cornerQueue.Enqueue(new Vector2(rect.Right, rect.Top));     // 0
            cornerQueue.Enqueue(new Vector2(rect.Right, rect.Bottom));  // 1
            cornerQueue.Enqueue(new Vector2(rect.Left, rect.Bottom));   // 2
            cornerQueue.Enqueue(new Vector2(rect.Left, rect.Top));      // 3

            //dont need to rotate if its the first type of side.
            if (side != 0)
            {
                //shift the corner 'side' times. (1-3)
                for (int i = 0; i < side; i++)
                {
                    Vector2 rotatedCorner = cornerQueue.Dequeue();
                    cornerQueue.Enqueue(rotatedCorner);
                }
            }

            List<Vector2> cornerList = cornerQueue.ToList();

            //System.Diagnostics.Debug.WriteLine(type);

            if (type == 2) //side or top
            {
                shadowVerts.Add(cornerList[0]);
                shadowVerts.Add(projectPoint(source, cornerList[0], windowSize));
                shadowVerts.Add(cornerList[1]);
                shadowVerts.Add(projectPoint(source, cornerList[3], windowSize));
                shadowVerts.Add(cornerList[2]);
                shadowVerts.Add(cornerList[3]);
            }
            else if (type == 3) //diagonal corner
            {
                shadowVerts.Add(cornerList[0]);
                shadowVerts.Add(projectPoint(source, cornerList[0], windowSize));
                shadowVerts.Add(cornerList[1]);
                shadowVerts.Add(projectPoint(source, cornerList[2], windowSize));
                shadowVerts.Add(cornerList[2]);
            }

            return shadowVerts;
        }

        /// <summary>
        /// Similar to createShadow() but in that it generates the 
        /// contacts lines used for colliding with shadows instead
        /// of the graphical representation.
        /// </summary>
        /// <param name="type">Type of shadow</param>
        /// <param name="side">Relative side</param>
        /// <param name="rect">Rect casting the shadow</param>
        /// <param name="source">Source of the light.</param>
        /// <param name="windowSize">The minimum size of the space you want to cover (Typically the window size)</param>
        /// <returns></returns>
        public static List<Line> createContactLines(int type, int side, Rectangle rect, Vector2 source, Vector2 windowSize)
        {
            List<Line> contactLines = new List<Line>();

            Queue<Vector2> cornerQueue = new Queue<Vector2>();
            cornerQueue.Enqueue(new Vector2(rect.Right, rect.Top));     // 0
            cornerQueue.Enqueue(new Vector2(rect.Right, rect.Bottom));  // 1
            cornerQueue.Enqueue(new Vector2(rect.Left, rect.Bottom));   // 2
            cornerQueue.Enqueue(new Vector2(rect.Left, rect.Top));      // 3

            //dont need to rotate if its the first type of side.
            if (side != 0)
            {
                //shift the corner 'side' times. (1-3)
                for (int i = 0; i < side; i++)
                {
                    Vector2 rotatedCorner = cornerQueue.Dequeue();
                    cornerQueue.Enqueue(rotatedCorner);
                }
            }

            List<Vector2> cornerList = cornerQueue.ToList();

            if (type == 2) //side or top
            {
                contactLines.Add(new Line(cornerList[0], projectPoint(source, cornerList[0], windowSize)));
                contactLines.Add(new Line(cornerList[3], projectPoint(source, cornerList[3], windowSize)));
            }
            else if (type == 3) //diagonal corner
            {
                contactLines.Add(new Line(cornerList[0], projectPoint(source, cornerList[0], windowSize)));
                contactLines.Add(new Line(cornerList[2], projectPoint(source, cornerList[2], windowSize)));
            }

            return contactLines;
        }

        /// <summary>
        /// Used to project a point past another point, until 
        /// a minimum distance is met.
        /// Currently uses a fixed distance based on the diagonal width
        /// of the window. We tried a few different approaches, but this
        /// seemed like the best solution based on performance and
        /// visual quality trade offs.
        /// </summary>
        /// <param name="source">Source of the light</param>
        /// <param name="point">Point to the projected past.</param>
        /// <param name="windowSize">The minimum size of the space you want to cover (Typically the window size)</param>
        /// <returns></returns>
        public static Vector2 projectPoint(Vector2 source, Vector2 point, Vector2 windowSize)
        {
            //source is light
            //origin is point
            float minDistance = windowSize.Length() * 3;

            Vector2 difference = point - source;

            Vector2 answer = point;

            while((point - answer).Length() < minDistance)
            {
                answer += difference;
            }
	
	        return answer;
        }

        //going to attempt to build a greedy algorithm to optimize the boxes as rectangles.
        //going to make the casting of shadows look more natural and more visually pleasing.

        //a value of true in the grid represents an object that you want to cast a shadow.
        //tile size assumed the same in x / y
        /// <summary>
        /// Made a greedy algorirthim used to optimize a grid of squares into groups of 
        /// rectangles instead. (Sources from a typical level setup file etc.)
        /// 
        /// The goal is to make the 2D shadows generated look more natural and
        /// visually pleasing.
        /// </summary>
        /// <param name="grid">The grid of bools representing the desired shadows. True is a shadow, false is none.</param>
        /// <param name="tileSize">Tilesize used.</param>
        /// <returns></returns>
        public static List<Rectangle> optimizeGridIntoRectangles(bool[,] grid, int tileSize)
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            //a grid of bools that is the same as the grid passed in.
            //default values are false. when we run over them we will set to true;
            bool[,] checkedBoxes = new bool[grid.GetLength(0), grid.GetLength(1)];

            bool check = checkedBoxes[0, 0];

            int cornerRow = 0;
            int cornerCol = 0;

            int currentWidth = 1;
            int currentHeight = 1;

            int gridWidth = grid.GetLength(0);
            int gridHeight = grid.GetLength(1);

            for (int row = 0; row < gridWidth; row++)
            {
                for (int col = 0; col < gridHeight; col++)
                {
                    //check if there is a block here
                    if (grid[row, col] == true)
                    {
                        //see if it's been checked already
                        if (checkedBoxes[row, col] == false)
                        {
                            cornerRow = row;
                            cornerCol = col;

                            //extend to the right as far as you can, (seeing if there is blocks to the right) record width.
                            int i = 1;
                            while (row + i < gridWidth && grid[row + i, col] == true && checkedBoxes[row + i, col] == false)
                            {
                                currentHeight++;
                                i++;
                            }
                            
                            //extend down as far as you can, record height.
                            i = 1;
                            while (col + i < gridHeight && grid[row, col + i] == true && checkedBoxes[row, col + i] == false)
                            {
                                currentWidth++;
                                i++;
                            }

                            for (int j = row; j < row + currentHeight; j++)
                            {
                                for (int k = col; k < col+ currentWidth; k++)
                                {
                                    checkedBoxes[j, k] = true;
                                }
                            }

                            //add the rectangle
                            rectangles.Add(makeRectangleFromGrid(tileSize, cornerRow, cornerCol, currentHeight, currentWidth));
                            currentWidth = 1;
                            currentHeight = 1;
                        }
                        
                    }
                }
            }

            return rectangles;
        }

        /// <summary>
        /// Util used to make rectangles resulting from the grid.
        /// </summary>
        /// <param name="tileSize"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="widthCount"></param>
        /// <param name="heightCount"></param>
        /// <returns></returns>
        public static Rectangle makeRectangleFromGrid(int tileSize, int row, int column, int widthCount, int heightCount)
        {
            return new Rectangle(column * tileSize, row * tileSize, heightCount * tileSize, widthCount * tileSize);
        }
    }
}
