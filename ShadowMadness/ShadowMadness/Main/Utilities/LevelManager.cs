using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDLibrary;
using GDLibrary.Managers;
using GDLibrary.Utilities;
using ShadowMadness;

namespace ShadowMadness
{
    public class LevelManager
    {
        private static char[] dataFileSeparator = { ',' };
        private static float backDepth = 0.3f;
        private static float frontDepth = 0.1f;
        private static int i = 1;
        public static List<Rectangle> loadShadowGrid(string path, string name, int tileWidth, int tileHeight)
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            path += name;
            String[] fileData = System.IO.File.ReadAllLines(path);
            String[] lineData = null;
            Vector2 position = Vector2.Zero;
            int textureNumber, rowCount = fileData.Length, colCount = 0;

            lineData = fileData[0].Split(dataFileSeparator);
            colCount = lineData.Length;

            bool[,] grid = new bool[rowCount, colCount];

            for (int row = 0; row < rowCount; row++)
            {
                lineData = fileData[row].Split(dataFileSeparator);
                colCount = lineData.Length;
                for (int col = 0; col < colCount; col++)
                {
                    textureNumber = Convert.ToInt32(lineData[col]);
                    //insert a range here to add to the shadow casters.
                    if (textureNumber == 2 || textureNumber == 6)
                    {
                        //there is a shadow present
                        grid[row, col] = true;
                    }
                    else
                    {
                        grid[row, col] = false;
                    }
                }
            }

            return Util.optimizeGridIntoRectangles(grid, tileWidth); 
        }

        public static List<Block>[] load(string path, string name, int tileWidth, int tileHeight, TextureManager textureManager)
        {
            path += name;
            String[] fileData = System.IO.File.ReadAllLines(path);
            String[] lineData = null;
            Vector2 position = Vector2.Zero;
            int textureNumber, rowCount = fileData.Length, colCount = 0;

            List<Block> collidables = new List<Block>();
            List<Block> nonCollidables = new List<Block>();
            List<Block> exits = new List<Block>();
            List<Block> springs = new List<Block>();
            List<Block> spikes = new List<Block>();
            List<Block> coins = new List<Block>();
            List<Block> key = new List<Block>();
            List<Block> destructables = new List<Block>();
            List<Block> solidCollidables = new List<Block>();

            for (int row = 0; row < rowCount; row++)
            {
                lineData = fileData[row].Split(dataFileSeparator);
                colCount = lineData.Length;
                for (int col = 0; col < colCount; col++)
                {
                    position = new Vector2(col * tileWidth, row * tileHeight);
                    textureNumber = Convert.ToInt32(lineData[col]);
                    if (textureNumber != 0) //not black 
                    {

                        if (textureNumber == 1)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("grassTiles");
                            if (textureData != null)
                            {
                            
                              SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                              solidCollidables.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }
                        }
                        else if (textureNumber == 2)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("tileSheet");
                            if (textureData != null)
                            {

                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                solidCollidables.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }
                        }
                        else if (textureNumber > 2 && textureNumber <= 5)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("tileSheet");
                            if (textureData != null)
                            {

                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                nonCollidables.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }
                        }

                        else if (textureNumber > 5 && textureNumber <= 7)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("destructableTile");
                            if (textureData != null)
                            {

                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                destructables.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }

                        }
                        else if (textureNumber == 8)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("spikes");
                            if (textureData != null)
                            {
                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                spikes.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }
                            
                        }
                        else if (textureNumber == 9)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("door");
                            if (textureData != null)
                            {
                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                exits.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }               
                        }
                        else if (textureNumber == 10)
                        {
                            TextureData textureData = null;
                            textureData = textureManager.Get("key");
                            if (textureData != null)
                            {
                                SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                                key.Add(getSprite(PositionInfo, textureData, textureNumber, tileWidth, tileHeight));
                            }
                        }
                        //else if (textureNumber > 11 && textureNumber <= 22)
                        //{
                        //    AnimatedTextureData textureData = null;
                        //    textureData = (AnimatedTextureData)game.TextureManager.Get("grassTiles");
                        //    if (textureData != null)
                        //    {
                        //        Transform transform = new Transform(position, 0, textureData.OriginTopLeft, Vector2.One, textureData.Dimensions, true);
                        //        game.SpriteManager.Add(getForgroundSprite(game, transform, textureData, textureNumber));
                        //    }
                        //}
                        //else
                        //{
                        //    TextureData textureData = null;
                        //    textureData = TextureManager.Get(textureNumber);
                        //    if (textureData != null)
                        //    {
                        //        SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(576, 864, tileWidth, tileHeight), backDepth);
                        //        SpritePositionInfo PositionInfo = new SpritePositionInfo(new Vector2(tileWidth * col, tileHeight * row), tileWidth, tileHeight);
                        //        collidables.Add(getBackgroundSprite(PresentationInfo, PositionInfo, textureData, textureNumber));
                        //    }
                        //}
                      //  textureData = game.TextureManager.Get(textureNumber);

                    }
                }
            }

            List<Block>[] blocks = new List<Block>[9];
            blocks[0] = collidables;
            blocks[1] = nonCollidables;
            blocks[2] = exits;
            blocks[3] = springs;
            blocks[4] = spikes;
            blocks[5] = key;
            blocks[6] = coins;
            blocks[7] = destructables;
            blocks[8] = solidCollidables;

            return blocks;
        } //end method


        /*
         * Student should change this code to indicate the depth of each loaded sprite.
         * The logic of how your game instanciates different sprite types at different levels is contained in this method.
         * */
        private static Block getSprite(SpritePositionInfo PositionInfo, TextureData textureData, int textureNumber, int tileWidth, int tileHeight)
        {
          
                Block sprite = null;
                
                //think this should be static if we leave like this.
                
                //use layer depth to say what things dont receive light e.g. textureNumber > 20 set layerDepth = 0.5 - See RenderScene pixel shader for depth value
             //   float layerDepth = 1;
                //use textureNumber to specify what types of sprite to create e.g. PlayerSprite, Moveable
                if (textureNumber == 1)
                {
                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(60, 60, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else if (textureNumber == 2)
                {
                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(480, 120, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else if (textureNumber == 4)
                {
                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(60, 60, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else if (textureNumber == 6)
                {

                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(0, 0, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);


                }
                else if (textureNumber == 8)
                {

                    SpritePresentationInfo PresentationInfo =  new SpritePresentationInfo(new Rectangle(0, 0, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else if (textureNumber == 9)
                {

                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(0, 0, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else if (textureNumber == 10)
                {

                    SpritePresentationInfo PresentationInfo = new SpritePresentationInfo(new Rectangle(0, 0, tileWidth, tileHeight), backDepth);
                    sprite = new Block("Tile" + i, textureData, PresentationInfo, PositionInfo);
                }
                else
                {
                    return null;
                }
                
                i++;
                return sprite;
            
         
        }

    


        //private static Sprite getForgroundSprite(Main game, Transform transform, TextureData textureData, int textureNumber)
        //{
        //    if (textureNumber > 11 && textureNumber <= 22)
        //    {
        //        TileSprite sprite = null;

        //        //use layer depth to say what things dont receive light e.g. textureNumber > 20 set layerDepth = 0.5 - See RenderScene pixel shader for depth value
        //       // float layerDepth = 1;
        //        //use textureNumber to specify what types of sprite to create e.g. PlayerSprite, Moveable
        //        if (textureNumber == 12)
        //        {
        //            sprite = new TileSprite(game, transform, (AnimatedTextureData)textureData, Presentation.Front, 0, 2);
        //        }
        //        else if (textureNumber == 13)
        //        {
        //            sprite = new TileSprite(game, transform, (AnimatedTextureData)textureData, Presentation.Front, 0, 0);
        //        }




        //        return sprite;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
