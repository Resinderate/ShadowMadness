// *****************************************************************************************
// * Useful 2D functions (18 April 2011) by jlucard - gdchaos.net
// *    SetPixel - Draw a pixel on screen
// *    Rectangle - Draw the outline of a rectangle
// *    FillRectangle - Draw a filled (solid) rectangle
// *    Line - Draw a line connecting two points
// *    Triangle - Draw the outline of a triangle
// *    FillTriangle - Draw a solid triangle given three points
// *
// ****************************************************************************************

// C# Libraries
using System;

// XNA Libraries 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Primitive2D
{
    //*************************************************************************************
    //* Draw2D class: uses a small texture (1x1 white square) to provide some basic 2D
    //* drawing primitives. The code is intended for testing things rather than for 2D
    //* game development (thus limited set of operations, not fully optimised).
    //*************************************************************************************
    public class Draw2D
    {
        private Game gameObject;        // Copy of the game object ("this" from main)
        private SpriteBatch sprites;    // The spriteBatch object
        private Texture2D pixel;        // Hold the texture used to draw the primitives

        // Constructor - where the pixel texture is made and private members are initialised
        public Draw2D(Game game, SpriteBatch sprites)
        {
            gameObject = game;
            this.sprites = sprites;

            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }
        // SetPixel - draw small rectangle of size weight at specific position
        public void SetPixel(Vector2 pos, int weight, Color color)
        {
            SetPixel((int)pos.X, (int)pos.Y, weight, color);
        }
        public void SetPixel(int x, int y, int weight, Color color)
        {
            // Try to center rectangle around coordinates
            // Need to go back weight/2 pixels
            // And draw rectangle at full width from that point or origin
            int b = weight / 2;
            sprites.Draw(pixel, new Rectangle(x - b, y - b, weight, weight), color);
        }
        public void SetPixel(float x, float y, float weight, Color color)
        {
            SetPixel((int)x, (int)y, (int)weight, color);
        }
        // Rectangle - draw a rectangle outline given its position, size and line weight
        public void Rectangle(Rectangle r, int weight, Color color)
        {
            Rectangle(r.X, r.Y, r.Width, r.Height, weight, color);
        }
        public void Rectangle(int x, int y, int width, int height, int weight, Color color)
        {
            // Draw four thin sub-rectangles one for each line our rectangle is made of
            sprites.Draw(pixel, new Rectangle(x, y, width, weight), color);
            sprites.Draw(pixel, new Rectangle(x, y, weight, height), color);
            sprites.Draw(pixel, new Rectangle(x, y + height, width, weight), color);
            sprites.Draw(pixel, new Rectangle(x + width, y, weight, height), color);
        }
        public void Rectangle(float x, float y, float width, float height, float weight, Color color)
        {
            Rectangle((int)x, (int)y, (int)width, (int)height, weight, color);
        }
        // FillRectangle - draw a filled rectangle (or horizontal/vertical line)
        public void FillRectangle(Rectangle r, Color color)
        {
            FillRectangle(r.X, r.Y, r.Width, r.Height, color);
        }
        public void FillRectangle(int x, int y, int width, int height, Color color)
        {
            sprites.Draw(pixel, new Rectangle(x, y, width, height), color);
        }
        public void FillRectangle(float x, float y, float width, float height, Color color)
        {
            FillRectangle((int)x, (int)y, (int)width, (int)height, color);
        }
        // Line - draw a line of a given weight connecting two points
        public void Line(Vector2 from, Vector2 to, int weight, Color color)
        {
            //float w2 = (float)weight / 2;
            //float size = Vector2.Distance(from, to);
            //float angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);

            //sprites.Draw(pixel, from, null, color,
            //  angle, Vector2.Zero, new Vector2(size, weight),
            //  SpriteEffects.None, 0);
            Line((int)from.X, (int)from.Y, (int)to.X, (int)to.Y, weight, color);
        }
        public void Line(int x1, int y1, int x2, int y2, int weight, Color color)
        {

            // Digital Differential Analyser (DDA) Algorithm
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int steps = dy;
            if (dx > dy) steps = dx;
            float sx = (float)(x2 - x1) / (float)steps;
            float sy = (float)(y2 - y1) / (float)steps;

            float x = x1;
            float y = y1;
            SetPixel(x1, y1, weight, color);
            for (int i = 0; i < steps; i++)
            {
                x += sx;
                y += sy;
                SetPixel((int)x, (int)y, weight, color);
            }

            // Then again it is easier to rotate a texture...
            //Line(new Vector2(x1, y1), new Vector2(x2, y2), weight, color);
        }
        public void Line(float x1, float y1, float x2, float y2, float weight, Color color)
        {
            Line(new Vector2(x1, y1), new Vector2(x2, y2), (int)weight, color);
        }
        // Triangle - draw a triangle outline given three points
        public void Triangle(Vector2 p1, Vector2 p2, Vector2 p3, int weight, Color color)
        {
            Line(p1, p2, weight, color);
            Line(p1, p3, weight, color);
            Line(p2, p3, weight, color);
        }
        public void Triangle(int x1, int y1, int x2, int y2, int x3, int y3, int weight, Color color)
        {
            // Draw three lines
            Line(x1, y1, x2, y2, weight, color);
            Line(x1, y1, x3, y3, weight, color);
            Line(x2, y2, x3, y3, weight, color);
        }
        public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3, float weight, Color color)
        {
            Triangle((int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)weight, color);
        }
        // FillTriangle - Obviously draws a filled triangle...
        public void FillTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            // Order points by swapping them around
            Vector2 tp;
            if (p2.Y > p1.Y) { tp = p1; p1 = p2; p2 = tp; }
            if (p3.Y > p1.Y) { tp = p1; p1 = p3; p3 = tp; }
            if (p3.Y > p2.Y) { tp = p2; p2 = p3; p3 = tp; }

            // Number of steps for each edge
            int steps13 = (int)(p1.Y - p3.Y);
            int steps12 = (int)(p1.Y - p2.Y);
            int steps23 = (int)(p2.Y - p3.Y);

            // Displacement value for interpolation
            float sx13 = (float)(p1.X - p3.X) / (float)steps13;
            float sx12 = (float)(p1.X - p2.X) / (float)steps12;
            float sx23 = (float)(p2.X - p3.X) / (float)steps23;

            // Draw LOWER part of triangle
            float x13 = p1.X;
            float x12 = p1.X;
            float dx = 1;   // Distance (length) of horizontal line to draw
            for (int i = 0; i < steps12; i++)
            {
                // Decrease by step
                x13 -= sx13;
                x12 -= sx12;
                // Calculate new distance between points of the two edges
                dx = x13 - x12;
                // Draw horizontal line (rectangle used)
                if (dx > 0)
                    FillRectangle((int)x12, (int)p1.Y - i, (int)Math.Abs(dx), 1, color);
                else
                    FillRectangle((int)x13, (int)p1.Y - i, (int)Math.Abs(dx), 1, color);
            }
            // Draw upper part of triangle
            // x13 should still hold a midpoint value from last loop so we need to 
            // connect this with points of x23 starting at point 2 (p2)
            float x23 = p2.X;
            for (int i = 0; i < steps23; i++)
            {
                // Decrease both by step as we move towards top corner
                x13 -= sx13;
                x23 -= sx23;
                // Length of horizontal line to draw
                dx = x13 - x23;
                if (dx > 0)
                    FillRectangle((int)x23, (int)p2.Y - i, (int)Math.Abs(dx), 1, color);
                else
                    FillRectangle((int)x13, (int)p2.Y - i, (int)Math.Abs(dx), 1, color);
            }
        }
        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Color color)
        {
            FillTriangle(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), color);
        }
        public void FillTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Color color)
        {
            FillTriangle((int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, color);
        }


        //Added by Ronan Murphy
        //Draws a triangle strip based on a list of Vector2 points.
        public void TriangleStrip(List<Vector2> vertList, Color color)
        {
            //is a triangle
            if (vertList.Count > 2)
            {
                FillTriangle(vertList[0], vertList[1], vertList[2], color);
                for (int i = 1; i < vertList.Count - 2; i++)
                {
                    FillTriangle(vertList[i], vertList[i + 1], vertList[i + 2], color);
                }
            }
        }

    }
}