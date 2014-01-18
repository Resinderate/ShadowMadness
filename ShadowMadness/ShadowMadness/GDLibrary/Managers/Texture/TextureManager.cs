using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ShadowMadness;
using Microsoft.Xna.Framework;

namespace GDLibrary.Managers
{
    public class TextureManager
    {
        //Debug to allow us to visualise the bounding rectangle - remove for release
        public static Texture2D DEBUG_BOUNDING_RECTANGLE_TEXTURE;

        protected Dictionary<string, TextureData> textureDictionary;
        private Main game;

        public TextureManager(Main game)
        {
            //Debug to allow us to visualise the bounding rectangle - remove for release
            if (GameData.DEBUG_SHOW_BOUNDING_RECTANGLES)
            {
                DEBUG_BOUNDING_RECTANGLE_TEXTURE = game.Content.Load<Texture2D>(@"Assets\\Debug\\debugrect");
            }

            this.game = game;
            this.textureDictionary = new Dictionary<string, TextureData>();
        }

        /// <summary>
        /// Adds a texture data object to the dictionary.
        /// </summary>
        /// <param name="id">unique string id for texture</param>
        /// <param name="path">relative path to the asset in the content folder</param>
        public void Add(string id, string path)
        {
            if (!textureDictionary.ContainsKey(id))
            {
                textureDictionary.Add(id, new TextureData(game, path));
            }
            else
            {
              //  System.Diagnostics.Debug.WriteLine(id + " already exists in dictionary!");
            }
        }
        
       /// <summary>
        /// Adds a animated texture data object to the dictionary.
       /// </summary>
       /// <param name="id"></param>
       /// <param name="path"></param>
       /// <param name="numberOfFrames"></param>
       /// <param name="frameWidth"></param>
       /// <param name="frameHeight"></param>
        public void Add(string id, string path, List<Rectangle> frameSources)
        {
            if (!textureDictionary.ContainsKey(id))
            {
                textureDictionary.Add(id, new AnimatedTextureData(game, path,
                    frameSources));
            }
            else
            {
               // System.Diagnostics.Debug.WriteLine(id + " already exists in dictionary!");
            }
        }

        /// <summary>
        /// Overloaded version to take in an existing texture.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="texture"></param>
        public void Add(string id, Texture2D texture)
        {
            if (!textureDictionary.ContainsKey(id))
            {
                textureDictionary.Add(id, new TextureData(game, texture));
            }
            else
            {
              //  System.Diagnostics.Debug.WriteLine(id + " already exists in dictionary!");
            }
        }

        /// <summary>
        /// Overloaded version to take in an existing texture.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="texture"></param>
        /// <param name="frameSources"></param>
        public void Add(string id, Texture2D texture, List<Rectangle> frameSources)
        {
            if (!textureDictionary.ContainsKey(id))
            {
                textureDictionary.Add(id, new AnimatedTextureData(game, texture,
                    frameSources));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(id + " already exists in dictionary!");
            }
        }
        /// <summary>
        /// Removes a texture from the dictionary by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed, otherwise false</returns>
        public bool Remove(string id)
        {
            if (textureDictionary.ContainsKey(id))
            {
                return textureDictionary.Remove(id);
            }
            return false;
        }

        /// <summary>
        /// Retrieves a handle to a texture from user-specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TextureData handle</returns>
        public TextureData Get(string id)
        {
            if (textureDictionary.ContainsKey(id))
            {
                return textureDictionary[id];
            }
            return null;
        }

        /// <summary>
        /// Returns a integer indicating the number of elements in the dictionary.
        /// </summary>
        /// <returns>int</returns>
        public int Size()
        {
            return textureDictionary.Count;
        }

        /// <summary>
        /// Disposes of all texture assets and clears the dictionary
        /// </summary>
        public void Clear()
        {
            foreach (TextureData textureData in textureDictionary.Values)
            {
                textureData.TEXTURE.Dispose();
               // textureData = null;
            }
            //possible bug???
            textureDictionary.Clear();
        }
        /// <summary>
        /// Returns a list of string IDs contained in the dictionary
        /// </summary>
        /// <returns>string array</returns>
        public string[] List()
        {
            string[] idArray = new string[textureDictionary.Count];
            textureDictionary.Keys.CopyTo(idArray, 0);
            return idArray;
        }


    }
}
