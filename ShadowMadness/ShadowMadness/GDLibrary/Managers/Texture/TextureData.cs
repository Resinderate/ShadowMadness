
using ShadowMadness;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDLibrary.Utilities;
using System;

namespace GDLibrary
{
    
    public class TextureData 
    {
        protected Color[,] textureColorData2D;
        protected Texture2D texture;
        protected Rectangle fullSourceRectangle;
        protected Vector2 centreOrigin;

        #region PROPERTIES
        public Color[,] TEXTURECOLORDATA2D
        {
            get
            {
                return textureColorData2D;
            }
            set
            {
                textureColorData2D = value;
            }
        }
        public Texture2D TEXTURE
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        public string NAME
        {
            get
            {
                return texture.Name;
            }
        }
        public Vector2 CENTREORIGIN
        {
            get
            {
                return centreOrigin;
            }
        }
        public Rectangle FULLSOURCERECTANGLE
        {
            get
            {
                return fullSourceRectangle;
            }
        }
        #endregion

        //Called by AnimatedTextureData - does nothing because AnimatedTextureData() does everything instead
        public TextureData()
        {
        }

        public TextureData(Main game, string path)
        {
            this.texture = game.Content.Load<Texture2D>(@"" + path);
            setColorData(texture);
           
            this.fullSourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            this.centreOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public TextureData(Main game, Texture2D texture)
        {
            this.texture = texture;
            setColorData(texture);

            this.fullSourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            this.centreOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public virtual int Width()
        {
            return texture.Width;
        }
        public virtual int Height()
        {
            return texture.Height;
        }

        //converts color data from texture from 1d to 2d array
        protected virtual void setColorData(Texture2D texture)
        {
            int width = texture.Width;
            int height = texture.Height;

            //read data into 1d array
            Color[] colors1D = new Color[width * height];
            texture.GetData(colors1D);

            //create 2d array to store data
            this.textureColorData2D = new Color[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    textureColorData2D[x, y] = colors1D[x + y * width];
                }
            }
        }
    }
}
