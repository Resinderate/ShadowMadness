using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;

namespace ShadowMadness
{
    public class Block : Sprite
    {

        private int health;
        public int destructionFrame = 0;

        public int HEALTH
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
     

        public Block(string name, TextureData textureData,
                    SpritePresentationInfo spritePresentationInfo,
                        SpritePositionInfo spritePositionInfo) :
            base(name, textureData, spritePresentationInfo, spritePositionInfo)
        {
            this.health = 5;
        }

        public override void Update(GameTime gameTime)
        {
            //Any Block specific updates here.
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }


        public void loseHealth()
        {
            health -= 1;
        }
    }
}
