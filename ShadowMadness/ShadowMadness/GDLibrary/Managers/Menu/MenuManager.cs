using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMadness;
using GDLibrary.Menu;
using GDLibrary.Utilities;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class MenuManager : DrawableGameComponent
    {
        protected List<MenuItem> menuItemList;

        public Main game;
        public SpriteFont menuFont;

        private Texture2D[] menuTextures;
        private Rectangle textureRectangle;

        private MenuItem menuContinue, menuRestart, menuQuit;
        private MenuItem menuEndRestart, menuEndQuit;

        protected int currentMenuTextureIndex = 0; //0 = main, 1 = volume

        #region PROPERTIES
   
        #endregion

        public MenuManager(Main game, String[] strMenuTextures,
            String strMenuFont, Integer2 textureBorderPadding)
            : base(game)
        {
            this.game = game;

            //nmcg - create an array of textures
            this.menuTextures = new Texture2D[strMenuTextures.Length];

            //nmcg - load the textures
            for (int i = 0; i < strMenuTextures.Length; i++)
            {
                this.menuTextures[i] =
                    game.Content.Load<Texture2D>(@"" + strMenuTextures[i]);
            }

            //nmcg - load menu font
            this.menuFont = game.Content.Load<SpriteFont>(@"" + strMenuFont);

            //nmcg - stores all menu item (e.g. Save, Resume, Exit) objects
            this.menuItemList = new List<MenuItem>();

            //sets menu texture to fullscreen minus and padding on XY
            this.textureRectangle = menuTextures[0].Bounds;

        }
        public override void Initialize()
        {
            //add the basic items - "Resume", "Save", "Exit"
            initialiseMenuOptions();
            showMainMenuScreen();


            base.Initialize();
        }

        private void initialiseMenuOptions()
        {
            //nmcg - add the menu items to the list
            this.menuContinue = new MenuItem(GameData.MENU_CONTINUE, GameData.MENU_CONTINUE,
                new Rectangle(265, 200, 250, 55), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuRestart = new MenuItem(GameData.MENU_RESTART, GameData.MENU_RESTART,
                new Rectangle(300, 270, 180, 50), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuQuit = new MenuItem(GameData.MENU_QUIT, GameData.MENU_QUIT,
               new Rectangle(340, 335, 100, 60), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);

           
            this.menuEndRestart = new MenuItem(GameData.MENU_RESTART, GameData.MENU_RESTART,
               new Rectangle(300, 250, 180, 50), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuEndQuit = new MenuItem(GameData.MENU_QUIT, GameData.MENU_QUIT,
               new Rectangle(340, 310, 100, 60), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);

            //nmcg - static variable used by the MenuItem class
            MenuItem.menuManager = this;
        }

        private void showMainMenuScreen()
        {
            removeAll();    //bug - fixed - 15/10/13 - nmcg

            add(menuContinue);
            add(menuRestart);
            add(menuQuit);
            currentMenuTextureIndex = 0;
        }

        public void showEndGameScreen()
        {
            removeAll();

            add(menuEndRestart);
            add(menuEndQuit);
        }

        public void add(MenuItem theMenuItem)
        {
            menuItemList.Add(theMenuItem);
        }

        public void remove(MenuItem theMenuItem)
        {
            menuItemList.Remove(theMenuItem);
        }

        public void removeAll()
        {
            menuItemList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < menuItemList.Count; i++)
            {
                //nmcg - call the update() to test for collisions with the mouse
                menuItemList[i].Update(gameTime);
                if (menuItemList[i].isItemClicked())
                {
                    menuAction(menuItemList[i].name);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SPRITEBATCH.Begin();
            //nmcg - draw whatever background we expect to see based on what menu or sub-menu we are viewing
            game.SPRITEBATCH.Draw(menuTextures[currentMenuTextureIndex],
                textureRectangle, Color.White);

            //nmcg - draw the text on top of the background
            for (int i = 0; i < menuItemList.Count; i++)
            {
                menuItemList[i].Draw(game.SPRITEBATCH);
            }

            game.SPRITEBATCH.End();

            base.Draw(gameTime);
        }

        //nmcg - perform whatever actions are listed on the menu
        private void menuAction(String name)
        {
            if (name.Equals(GameData.MENU_CONTINUE))
            {
                game.SPRITEMANAGER.PAUSE = false;
                game.Components.Remove(this);
                game.BACKGROUNDMUSIC.Play();
            }
            else if (name.Equals(GameData.MENU_RESTART))
            {
                //restart
                game.PLAYER.resetGame();
                game.SPRITEMANAGER.PAUSE = false;
                game.Components.Remove(this);
                game.BACKGROUNDMUSIC.Play();
            }

            else if (name.Equals(GameData.MENU_QUIT))
            {
                this.game.Exit();
            }
            
        }

        /// <summary>
        /// Clear all items from the menu list. Used when transitioning between menus (e.g. Main <-> Audio)
        /// </summary>
        private void clearMenuList()
        {
            this.menuItemList.Clear();
        }
    }
}
