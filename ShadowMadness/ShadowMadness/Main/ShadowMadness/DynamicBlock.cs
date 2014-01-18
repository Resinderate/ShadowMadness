using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;

namespace ShadowMadness
{
    public class DynamicBlock : ModularAnimatedSprite
    {
        public DynamicBlock(string name, List<AnimatedTextureData> animationsList,
                    SpritePresentationInfo spritePresentationInfo,
                    SpritePositionInfo spritePositionInfo,
                    int frameRate) :
            base(name, animationsList, spritePresentationInfo, spritePositionInfo, frameRate)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
