using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    
    public class SpritePresentationInfo : ICloneable
    {
        //Rectangle defining how much of the original image we draw to screen. Maximum is (0,0,texture.Width,texture.Height)
        protected Rectangle sourceRectangle;
        protected Color color;
        //we can define horizontal/vertical flipping here
        protected SpriteEffects effect;
        //0->1 where 0 = front of the screen, 1 = back of the screen
        protected float layerDepth;

        #region PROPERTIES
      
        public SpriteEffects SPRITEEFFECTS
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }
        public float LAYERDEPTH
        {
            get
            {
                return layerDepth;
            }
            set
            {
                layerDepth = value;
            }
        }
     
        public Color COLOR
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        public Rectangle SOURCERECTANGLE
        {
            get
            {
                return sourceRectangle;
            }
            set
            {
                sourceRectangle = value;
            }
        }
        public int SOURCERECTANGLEX
        {
            get
            {
                return sourceRectangle.X;
            }
            set
            {
                sourceRectangle.X = value;
            }
        }
        public int SOURCERECTANGLEY
        {
            get
            {
                return sourceRectangle.Y;
            }
            set
            {
                sourceRectangle.Y = value;
            }
        }
        #endregion

        public SpritePresentationInfo(Rectangle sourceRectangle, Color color, SpriteEffects effect, float layerDepth)
        {
            this.sourceRectangle = sourceRectangle;
            this.layerDepth = layerDepth;
            this.color = color;
            this.effect = effect;
        }

        public SpritePresentationInfo(Rectangle sourceRectangle, Color color, float layerDepth)
        {
            this.sourceRectangle = sourceRectangle;
            this.layerDepth = layerDepth;
            this.color = color;
            this.effect = SpriteEffects.None;
        }

        public SpritePresentationInfo(Rectangle sourceRectangle, float layerDepth)
        {
            this.sourceRectangle = sourceRectangle;
            this.layerDepth = layerDepth;
            this.color = Color.White;
            this.effect = SpriteEffects.None;
        }

        public void flipHorizontally()
        {
            if(this.effect != SpriteEffects.FlipHorizontally)
                this.effect = SpriteEffects.FlipHorizontally;
        }

        public void changeSourceRectangle(int x, int y, int tileWidth, int tileHeight)
        {
            this.sourceRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
        }

        public Object Clone()
        {
            //perform a deep copy clone of all value types (i.e. primitives) and c# reference types (e.g. Rectangle)
            return this.MemberwiseClone();
        }
    }
}
