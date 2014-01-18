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
using GDLibrary.Managers;

namespace GDLibrary
{
    
    public class AnimatedSprite : Sprite
    {
        protected AnimatedTextureData animatedTextureData;
        protected int frameRate, startFrame, currentFrame = 0;
        protected bool bRepeatAnimation, bPause;
        protected double timeSinceLastFrameInMs, timeBetweenFrameInMs;

        #region PROPERTIES
        public AnimatedTextureData ANIMATEDTEXTUREDATA
        {
            get  
            {
                return animatedTextureData;
            }
        }
        #endregion

        public AnimatedSprite(string name, AnimatedTextureData animatedTextureData,
                    SpritePresentationInfo spritePresentationInfo,
                        SpritePositionInfo spritePositionInfo,
                            int frameRate, int startFrame, bool bRepeatAnimation)
            : base(name, animatedTextureData, spritePresentationInfo, spritePositionInfo)
        {
            this.animatedTextureData = animatedTextureData;// (AnimatedTextureData)animatedTextureData.Clone();
            this.TEXTUREDATA.TEXTURECOLORDATA2D = animatedTextureData[0];

            this.frameRate = frameRate;
            timeBetweenFrameInMs = 1000.0 / frameRate; //time between each frame if they play at frameRate per second (e.g. 24fps gives timeBetweenFrameInMs = 1000ms/24 per second)
            timeSinceLastFrameInMs = timeBetweenFrameInMs; //means no initial delay in animation
            this.startFrame = startFrame;
            currentFrame = startFrame;
            this.bRepeatAnimation = bRepeatAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            if (!bPause)
            {
                animate(gameTime);
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void animate(GameTime gameTime)
        {
            timeSinceLastFrameInMs += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrameInMs > timeBetweenFrameInMs) //time to play the next frame?
            {
                //can we advance a frame without overrunning the end of the animation?
                if (currentFrame < animatedTextureData.NUMBEROFFRAMES)
                {
                    Set();
                    currentFrame++;
                    timeSinceLastFrameInMs = 0;
                }
                else //are we at end of animation?
                {
                    if (bRepeatAnimation) //if repeat, reset to start of animation
                    {
                        currentFrame = 0;
                        timeSinceLastFrameInMs = timeBetweenFrameInMs; //means no reset delay in animation
                        Set();
                    }
                    else //if not repeat, then remove
                    {
                        spriteManager.Remove(this);
                    }
                }
            }
         }

        public void Pause()
        {
            this.bPause = true;
        }
        public void Play()
        {
            this.bPause = false;
            this.timeSinceLastFrameInMs = 0;
        }
        public void Set()
        {
            this.PRESENTATIONINFO.SOURCERECTANGLE = this.animatedTextureData.FRAMESOURCES[currentFrame];
           // this.POSITIONINFO.BOUNDS = this.animatedTextureData.FRAMESOURCES[currentFrame];
            setTextureColorData();
        }

        protected void setTextureColorData()
        {
            this.TEXTUREDATA.TEXTURECOLORDATA2D = animatedTextureData[currentFrame];
        }
    }
}
