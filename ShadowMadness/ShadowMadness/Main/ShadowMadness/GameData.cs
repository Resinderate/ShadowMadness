using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShadowMadness
{
    class GameData
    {

        public static bool DEBUG_SHOW_BOUNDING_RECTANGLES = false;

        #region MENU STRINGS;
        //all the strings shown to the user through the menu
        public static String GAME_TITLE = "ShadowMadness";
        public static String MENU_CONTINUE = "Continue";
        public static String MENU_RESTART = "Restart";
        public static String MENU_QUIT = "Quit";

        public static Color MENU_INACTIVE_COLOR = Color.White;
        public static Color MENU_ACTIVE_COLOR = Color.Red;

        #endregion;

        #region LEVEL VARIABLES
        public static int LEVEL_WIDTH;
        public static int LEVEL_HEIGHT;
        public static int WINDOW_WIDTH = 800;
        public static int WINDOW_HEIGHT = 600;
        public static Vector2 WINDOW_SIZE = new Vector2(800, 600);
        #endregion

    }
}
