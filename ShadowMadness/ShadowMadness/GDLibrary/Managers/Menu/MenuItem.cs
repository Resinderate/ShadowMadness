using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GDLibrary.Menu
{
    public class MenuItem
    {
        public String name, text;
        protected Color inactiveColor, activeColor;
        public Rectangle bounds;
        private Vector2 position;
        private Color drawColor;

        public static MenuManager menuManager;
        public bool bIsOver = false;

        public MenuItem(String name, String text, Rectangle bounds, Color inactiveColor, Color activeColor)
        {
            this.name = name;
            this.text = text;
            this.inactiveColor = inactiveColor;
            this.activeColor = activeColor;
            this.drawColor = inactiveColor;
            this.bounds = bounds;
            this.position = new Vector2(bounds.X, bounds.Y);
        }

        public bool isItemClicked()
        {
            ////nmcg - if the mouse is within the bounds rectangle and the left mouse button is pressed then return true
            if ((menuManager.game.MOUSEMANAGER.newState.LeftButton.Equals(ButtonState.Pressed)) 
                && (bIsOver)
                    && (menuManager.game.MOUSEMANAGER.oldState.LeftButton.Equals(ButtonState.Pressed)))
            {
                return true;
            }
            
            return false;
        }


        public void checkMouseOver()
        {
            if (this.bounds.Contains(menuManager.game.MOUSEMANAGER.bounds))
            {
                bIsOver = true;
                drawColor = activeColor;
            }
            else
            {
                bIsOver = false;
                drawColor = inactiveColor;
            }
        }

        public void Update(GameTime gameTime) 
        {
            //nmcg - test for collisions with the mouse pointer
            checkMouseOver();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //why have these methods been commented out?
    //        spriteBatch.Begin();
            spriteBatch.DrawString(menuManager.menuFont, text, position, drawColor);
     //       spriteBatch.End();
        }
    }
}
