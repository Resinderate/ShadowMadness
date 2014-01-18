using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary.Behaviours
{
    public class Behaviour
    {
        public static bool Contains(Rectangle areaBounds,
            Rectangle spriteBounds)
        {
            return areaBounds.Contains(spriteBounds);
        }

        public static bool ScreenContains(Rectangle areaBounds,
          Rectangle spriteBounds)
        {
            return areaBounds.Contains(spriteBounds);
        }



    }
}
