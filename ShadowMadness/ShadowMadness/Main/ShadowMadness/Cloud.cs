using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;

namespace ShadowMadness
{
    public class Cloud : Sprite
    {
        float speed;
        float moveAmount;

        public Cloud(float speed, string name, TextureData textureData,
                    SpritePresentationInfo spritePresentationInfo,
                        SpritePositionInfo spritePositionInfo) :
            base(name, textureData, spritePresentationInfo, spritePositionInfo)
        {
            this.speed = speed;
            this.moveAmount = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            moveAmount = speed * gameTime.ElapsedGameTime.Milliseconds;

            this.TranslateBy(moveAmount, 0);

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
