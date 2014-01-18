using Microsoft.Xna.Framework;
using GDLibrary.Managers;
using GDLibrary.Utilities;
using ShadowMadness;
using System;

namespace GDLibrary
{
    
    public class Sprite
    {
        public static SpriteManager spriteManager;

        private int sectorNumber;

        private string name;
        private TextureData textureData;
        private SpritePresentationInfo spritePresentationInfo;
        private SpritePositionInfo spritePositionInfo;

        #region PROPERTIES
        public SpritePresentationInfo PRESENTATIONINFO
        {
            get
            {
                return spritePresentationInfo;
            }
        }
        public SpritePositionInfo POSITIONINFO
        {
            get
            {
                return spritePositionInfo;
            }
        }
        public TextureData TEXTUREDATA
        {
            get
            {
                return textureData;
            }
        }
        public string NAME
        {
            get
            {
                return name;
            }
        }
        #endregion

        public Sprite(string name, TextureData textureData, 
                    SpritePresentationInfo spritePresentationInfo,
                        SpritePositionInfo spritePositionInfo)
        {
            this.name = name;
            this.textureData = textureData;
            this.spritePresentationInfo = spritePresentationInfo;
            this.spritePositionInfo = spritePositionInfo;

            //make sure we set first time around
            this.sectorNumber = Collision.getSectorNumber(this.POSITIONINFO.BOUNDS);

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
            //update bounds if anything changed e.g. translation, rotation, scale, origin
            this.spritePositionInfo.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime)
        {       
            SpriteManager.GAME.SPRITEBATCH.Draw(textureData.TEXTURE,
                spritePositionInfo.TRANSLATION, 
                spritePresentationInfo.SOURCERECTANGLE,
                spritePresentationInfo.COLOR,
                MathHelper.ToRadians(spritePositionInfo.ROTATIONINDEGREES),
                spritePositionInfo.ORIGIN,
                spritePositionInfo.SCALE,
                spritePresentationInfo.SPRITEEFFECTS,
                spritePresentationInfo.LAYERDEPTH);

            if(GameData.DEBUG_SHOW_BOUNDING_RECTANGLES)
                DebugHelper.drawDebugRectangle(this);
        }

        /// <summary>
        /// Test if the sprite is outside the screen
        /// </summary>
        protected virtual void checkBounds()
        {

        }
        /// <summary>
        /// Test for user input
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void handleInput(GameTime gameTime)
        {
            //Does nothing. Called only be moveable sprites
        }

        /// <summary>
        /// Test for CDCR with other sprites
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void checkCollision(GameTime gameTime)
        {
           
        }

        /// <summary>
        /// Translate the Sprite by the amount specified
        /// </summary>
        /// <param name="translateBy">Vector2 representing the amount by which the sprite should move</param>
        protected virtual void TranslateBy(float x, float y)
        {
            this.POSITIONINFO.TRANSLATIONX += x;
            this.POSITIONINFO.TRANSLATIONY += y;
        }

        protected virtual void TranslateBy(Vector2 translation)
        {
            this.POSITIONINFO.TRANSLATION += translation;
        }

        /// <summary>
        /// Translate the Sprite to a new position on screen.
        /// </summary>
        /// <param name="translateTo">Vector2 representing the new position on screen</param>
        protected virtual void TranslateTo(float x, float y)
        {
            this.POSITIONINFO.TRANSLATIONX = x;
            this.POSITIONINFO.TRANSLATIONY = y;
        }
        
        /// <summary>
        /// Allows us to rotate by a user-defined number of degrees - see EnemySprite::Update()
        /// </summary>
        /// <param name="rotateBy"></param>
        protected virtual void RotateBy(float rotateBy)
        {
            this.POSITIONINFO.ROTATIONINDEGREES += rotateBy;
        }

        protected virtual void RotateTo(float rotateTo)
        {
            this.POSITIONINFO.ROTATIONINDEGREES = rotateTo;
        }

        protected void flipHorizontally()
        {
            this.PRESENTATIONINFO.flipHorizontally();
        }
    
    }
}
