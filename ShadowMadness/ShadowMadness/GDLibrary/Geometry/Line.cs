using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class Line
    {
        private Vector2 origEnd;
        public Vector2 start;
        public Vector2 end;
        public float slope;

        public Line(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            this.origEnd = end - start;
            this.slope = (end.Y - start.Y) / (end.X - start.X);
        }

        public Line(float startX, float startY, float endX, float endY)
        {
            this.start = new Vector2(startX, startY);
            this.end = new Vector2(endX, endY);
            this.origEnd = end - start;
            this.slope = (end.Y - start.Y) / (end.X - start.X);
        }

        public Vector2 findPointOfX(float x)
        {
            float c = start.Y - (slope * start.X);

            return new Vector2(x, (slope * x) + c);
        }

        public bool isInXRange(float x)
        {
            float min = Math.Min(start.X, end.X);
            float max = Math.Max(start.X, end.X);

            return (x > min && x < max);
        }

        public void moveTo(Vector2 point)
        {
            start = point;
            end = point + origEnd;
        }

        public static List<Line> lineFactory(Rectangle rect)
        {
            List<Line> lines = new List<Line>();
            lines.Add(new Line(rect.Left, rect.Top, rect.Right, rect.Top));
            lines.Add(new Line(rect.Right, rect.Y, rect.Right, rect.Bottom));
            lines.Add(new Line(rect.Right, rect.Bottom, rect.Left, rect.Bottom));
            lines.Add(new Line(rect.Left, rect.Bottom, rect.Left, rect.Top));
            return lines;
        }

        public static Line getTopLine(Rectangle rect)
        {
            return new Line(rect.Left, rect.Top, rect.Right, rect.Top);
        }

        public static void DrawLine(SpriteBatch batch, Texture2D blank,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
