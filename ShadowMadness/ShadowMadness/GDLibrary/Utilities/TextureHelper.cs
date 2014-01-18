using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;

namespace GDLibrary.Utilities
{
    public class TextureHelper
    {
        public static void createTexture(
            GraphicsDevice graphics,
            string name, byte format,
            int width, int height,
            uint[] colorData)
        {
            Stream fStream
                = new FileStream(name,
                    FileMode.CreateNew);

            Texture2D outputTexture
                = new Texture2D(graphics,
                    width, height, false, SurfaceFormat.Color);
            outputTexture.SetData<uint>(colorData);
            outputTexture.SaveAsPng(fStream, width, height);
        }

        public static void copyTexture(
           GraphicsDevice graphics, string name,
            Texture2D sourceTexture, SurfaceFormat format)//, Texture2D textureB)
        {
            Stream fStream 
                = new FileStream(name, FileMode.CreateNew);

            uint[] colorData = new uint[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData<uint>(colorData);

            Texture2D outputTexture = new Texture2D(graphics,
                                sourceTexture.Width, sourceTexture.Height, 
                                    false, format);
            outputTexture.SetData<uint>(colorData);

            outputTexture.SaveAsPng(fStream, outputTexture.Width, outputTexture.Height);

        }

        /// <summary>
        /// Under construction - nmcg - 8.10.13
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="name"></param>
        /// <param name="textureA"></param>
        /// <param name="textureB"></param>
        public static void packTextures(
         GraphicsDevice graphics, string name,
          Texture2D textureA, Texture2D textureB)
        {
            int numberOfTextures = 2;

            //the output image is going to be N times the width and the same height. N = 2
            int outColorDataSize = textureA.Width * numberOfTextures * textureA.Height;

            //create an array for the output color data
            uint[] outColorData = new uint[outColorDataSize];

            //create the arrays to store the original color data from each texture
            uint[] colorDataTextureA = new uint[textureA.Width * textureA.Height];
            uint[] colorDataTextureB = new uint[textureB.Width * textureB.Height];

            //actually copy the data from the texture2d object into each array
            textureA.GetData<uint>(colorDataTextureA);
            textureB.GetData<uint>(colorDataTextureB);

            //the row number in the output image
            int rowNumber = 0;

            int finalWidth = textureA.Width * numberOfTextures;
            int finalHeight = textureA.Height;

            for (int i = 0; i < outColorDataSize; i++)
            {
                if(rowNumber%2 == 0)
                    outColorData[i] 
                        = colorDataTextureA[i % 20 
                        + rowNumber * finalWidth];
                else
                    outColorData[i]
                        = colorDataTextureB[i % 20
                        + rowNumber * finalWidth];

                rowNumber = (int)Math.Floor((double)i/finalWidth);
            }

            Texture2D outputTexture = new Texture2D(graphics,
                              finalWidth, finalHeight, false, SurfaceFormat.Color);

            Stream fStream = new FileStream(name, FileMode.CreateNew);

            outputTexture.SaveAsPng(fStream, finalWidth, finalHeight);

        }




    }
}
