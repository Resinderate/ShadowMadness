/*
 * Written by Niall McG, Modified by Ronan Murphy 30/10/13.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowMadness;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDLibrary.Utilities;

namespace GDLibrary
{
    
    public class AnimatedTextureData : TextureData, ICloneable
    {
        //width and height of a single frame inside the animation
        private int numberOfFrames;
        private int frameWidth;
        private int frameHeight;

        //this is a list containing all the source rectangle color data

        public List<Rectangle> frameSources;
        public List<Color[,]> textureColorData2DList;


        #region PROPERTIES
        public Color[,] this[int index]
        {
            get
            {
                return textureColorData2DList[index];
            }
        }

        public int NUMBEROFFRAMES
        {
            get
            {
                return numberOfFrames;
            }
        }
        public List<Rectangle> FRAMESOURCES
        {
            get
            {
                return frameSources;
            }
        }
        public int FRAMEWIDTH
        {
            get
            {
                return frameWidth;
            }
        }
        public int FRAMEHEIGHT
        {
            get
            {
                return frameHeight;
            }
        }
        #endregion

        //these don't work if animations are empty.
        public override int Width()
        {
            return frameSources[0].Width;
        }
        public override int Height()
        {
            return frameSources[0].Height;
        }

        public AnimatedTextureData(Main game, string path, List<Rectangle> frameSources)
            : base()
        {
            this.texture = game.Content.Load<Texture2D>(@"" + path);
 
            this.numberOfFrames = frameSources.Count;
            this.frameWidth = frameSources[0].Width;
            this.frameHeight = frameSources[0].Height;

            this.fullSourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight);
            this.centreOrigin = new Vector2(frameWidth / 2, frameHeight / 2);

            this.frameSources = frameSources;
            this.textureColorData2DList = new List<Color[,]>(numberOfFrames);
            setColorData(texture);
        }

        //code repition not good.
        public AnimatedTextureData(Main game, Texture2D texture, List<Rectangle> frameSources)
            : base()
        {
            this.texture = texture;

            this.numberOfFrames = frameSources.Count;
            this.frameWidth = frameSources[0].Width;
            this.frameHeight = frameSources[0].Height;

            this.fullSourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight);
            this.centreOrigin = new Vector2(frameWidth / 2, frameHeight / 2);

            this.frameSources = frameSources;
            this.textureColorData2DList = new List<Color[,]>(numberOfFrames);
            setColorData(texture);
        }


        // NEEDS TESTING FOR CORRECT Color[,] ARRAYS BEING GEN'D, COULOD IMPACT COLLISION DETECTION.


        /// <summary>
        /// Converts a Texture2D into a list of Color[,] array data
        /// e.g. an image with 8 frames will have 8 Color[,] entries in the list.
        /// Each Color[,] is a 2D array of color data for the frame.
        /// This 2D color array is used for Non-AA CDCR - see Collision class
        /// </summary>
        /// <param name="texture"></param>
        protected override void setColorData(Texture2D texture) //you have access to the texture in parent.
        {
            int width = texture.Width;
            int height = texture.Height;


            //read data into 1d array of whole texture STUFF IN HERE
            Color[] colors1D = new Color[width * height];
            texture.GetData(colors1D);

            //going to put the whole texture into a 2D array to make it easier to work with.
            Color[,] colors2DWholeTexture = new Color[width, height];
            int color1DIndex = 0;

            //think this is right;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //if (colors1D[color1DIndex].R + colors1D[color1DIndex].G + colors1D[color1DIndex].B + colors1D[color1DIndex].A > 0)
                        //System.Diagnostics.Debug.WriteLine(colors1D[color1DIndex].R + colors1D[color1DIndex].G + colors1D[color1DIndex].B + colors1D[color1DIndex].A);
                    colors2DWholeTexture[x, y] = colors1D[color1DIndex];
                    color1DIndex++;
                }
            }



            //create 2d array to store data for single frame
            Color[,] colors2D = new Color[frameWidth, frameHeight]; //this.frameWidth, if legit

            //read each frame into a seperate colors2D array and add it to the list
            //then when we want to now the color data for a particular frame we just query the list
            for (int frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
            {
                for (int x = 0; x < frameWidth; x++)
                {
                    for (int y = 0; y < frameHeight; y++)
                    {
                        //THESE ARE 0

                        //Based on the rect we are dealing with, it will start at it's X, Y ..  and then fill the new
                        //rect by looping through the width and height of the frame.
                        colors2D[x, y] = colors2DWholeTexture[this.frameSources[frameIndex].X + x, 0];
                    }
                }
                textureColorData2DList.Add(colors2D);
            }
        }

        public Object Clone()
        {
            //perform a deep copy clone of all value types (i.e. primitives) and c# reference types (e.g. Rectangle)
            AnimatedTextureData copy = (AnimatedTextureData)this.MemberwiseClone();
            copy.textureColorData2DList = new List<Color[,]>(this.textureColorData2DList);
            return copy;
        }
    }
}
