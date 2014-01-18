using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowMadness
{
    //Needs pathing or something.
    public class Enemy : ModularAnimatedSprite
    {
        private Vector2 startPoint;
        private Vector2 endPoint;
        private int numberOfSteps; // need to be careful with this number. Some divisions wont actually hit the end point, which is how it is queried to turn around.
        private Vector2 step;

        public Enemy(Vector2 startPoint, Vector2 endPoint, int numberOfSteps, string name, List<AnimatedTextureData> animationsList,
                    SpritePresentationInfo spritePresentationInfo,
                    SpritePositionInfo spritePositionInfo,             
                    int frameRate) :
            base(name, animationsList, spritePresentationInfo, spritePositionInfo, frameRate)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.numberOfSteps = numberOfSteps;
            this.step = (endPoint - startPoint) / numberOfSteps;
        }

        //unused.
        public Enemy() : base("", null, null, null, 0)
        {

        }

        public override void Update(GameTime gameTime)
        {
            walkPath();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void walkPath()
        {
            //add the step to its position until it hits the endPoint
            this.TranslateBy(step);

            if (this.POSITIONINFO.TRANSLATION == endPoint || this.POSITIONINFO.TRANSLATION == startPoint)
            {
                step *= -1;
                if (this.PRESENTATIONINFO.SPRITEEFFECTS == SpriteEffects.FlipHorizontally)
                    this.PRESENTATIONINFO.SPRITEEFFECTS = SpriteEffects.None;
                else
                    this.PRESENTATIONINFO.SPRITEEFFECTS = SpriteEffects.FlipHorizontally;
            }
            //when it hits the end point reverse the step ( * -1)
            //repeat until you hit the start point again

            //repeat from start
        }
    }
}
