/*
 * Ronan Murphy, 31/10/2013
 * Modular Animated Sprite. Used to include multiple sets of Data for different animations.
 * Fairly basic and only changes the current AnimatedTextureData, but assumes that additional details like
 * frame rate and start position stay the same.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;

namespace ShadowMadness
{
    
    public class ModularAnimatedSprite : AnimatedSprite
    {
        private List<AnimatedTextureData> animationsList;
        int currentAnimation;

        public int CURRENTANIMATION
        {
            get
            {
                return currentAnimation;
            }
        }

        //for some of the values to be passed into AnimatedSprite I took them out and assumed they
        //would use "generic values". Especially for the repeat bool, it would cause trouble if
        //set to false. Could extend it further to return to "base" animaiton when dont with an
        //animation if needed. 
        //(Current implementation would remove the sprite after one animation is done if
        //set to fasle)
        public ModularAnimatedSprite(string name, List<AnimatedTextureData> animationsList,
                    SpritePresentationInfo spritePresentationInfo,
                    SpritePositionInfo spritePositionInfo,
                    int frameRate) :
            base(name, animationsList[0], spritePresentationInfo, spritePositionInfo,
                frameRate, 0, true)
        {
            this.animationsList = animationsList;
            this.currentAnimation = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void changeAnimation(int newAnimationIndex)
        {
            if (newAnimationIndex >= 0 && newAnimationIndex < animationsList.Count)
            {
                
                //keeps the frames "expanding" from the bottom if they have different heights.
                int yAxisHeightDif = this.animatedTextureData.FRAMEHEIGHT - animationsList[newAnimationIndex].FRAMEHEIGHT;
                
                this.animatedTextureData = animationsList[newAnimationIndex];
                this.TEXTUREDATA.TEXTURECOLORDATA2D = animationsList[newAnimationIndex].TEXTURECOLORDATA2D;
                // -1 fixed bug when adjusting position based on different height animations.
                // keeps it out of the obstacles and getting stuck
                //messes with other dynamic objects that you dont want to move when changing animation
                this.TranslateBy(0, yAxisHeightDif);

                //Rectangle oldRect = this.POSITIONINFO.BOUNDS;

                this.POSITIONINFO.BOUNDSWIDTH = animationsList[newAnimationIndex].FRAMEWIDTH;
                this.POSITIONINFO.BOUNDSHEIGHT = animationsList[newAnimationIndex].FRAMEHEIGHT;
                this.currentAnimation = newAnimationIndex;
            }
            //reset the animation to start.
            this.currentFrame = 0;
        }
    }
}
