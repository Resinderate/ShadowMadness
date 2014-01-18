using Microsoft.Xna.Framework;
using GDLibrary;
using GDLibrary.Managers;
using System.Collections.Generic;

namespace ShadowMadness
{
    public class UIManager : DrawableGameComponent
    {
        //three hearts.
        //three modular anm. spr. with full / empty states.
        //when you update lives, update these.
       
        //amount of stars. Star x 0
 
        private Sprite healthBar;
     
        //key y/n
        private Sprite key;

        private float depth;



        public UIManager(Main game)
            : base(game)
        {
            this.depth = 0.05f;        

            this.healthBar = new Sprite("healthBar",
                SpriteManager.GAME.TEXTUREMANAGER.Get("HealthBar"),
                new SpritePresentationInfo(new Rectangle(0, 0, 128, 26), depth),
                new SpritePositionInfo(new Vector2(10, 15), 128, 28));

            this.key = new Sprite("keySheet",
                SpriteManager.GAME.TEXTUREMANAGER.Get("keySheet"),
                new SpritePresentationInfo(new Rectangle(0, 0, 58, 58), depth),
                new SpritePositionInfo(new Vector2(10, 50), 58, 58));
        }


        public void updateHealth(int lives)
        {
            if (lives == 4)
            {
                healthBar.PRESENTATIONINFO.changeSourceRectangle(0,0,128,26);
            }
            if (lives == 3)
            {
                healthBar.PRESENTATIONINFO.changeSourceRectangle(0, 1, 128, 26);
            }
            else if (lives == 2)
            {
                healthBar.PRESENTATIONINFO.changeSourceRectangle(0, 2, 128, 26);
               
            }
            else if (lives == 1)
            {
                healthBar.PRESENTATIONINFO.changeSourceRectangle(0, 3, 128, 26);
            }
            else if (lives == 0)
            {
                healthBar.PRESENTATIONINFO.changeSourceRectangle(0, 4, 128, 26);
            }
        }

        public void updateKey(bool hasKey)
        {
            if (hasKey)
            {
                key.PRESENTATIONINFO.changeSourceRectangle(0, 1, 58, 58);
            }
            else
            {
                key.PRESENTATIONINFO.changeSourceRectangle(0, 0, 58, 58);
            }
        }


        public override void Update(GameTime gameTime)
        {
        //    updateHearts(SpriteManager.GAME.PLAYER.HEALTH);
            updateHealth(SpriteManager.GAME.PLAYER.HEALTH);
            updateKey(SpriteManager.GAME.PLAYER.HASKEY);
         

            healthBar.Update(gameTime);
            key.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteManager.GAME.SPRITEBATCH.Begin();
            healthBar.Draw(gameTime);
            key.Draw(gameTime);
            SpriteManager.GAME.SPRITEBATCH.End();
            //base.Draw(gameTime);
        }
    }
}
