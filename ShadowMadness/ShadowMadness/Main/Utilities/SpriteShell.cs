using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShadowMadness
{
    public class SpriteShell
    {
        public int sectorNumber { get; set; }
        public Rectangle bounds { get; set; }

        public SpriteShell(int sectorNumber, Rectangle bounds)
        {
            this.sectorNumber = sectorNumber;
            this.bounds = bounds;
        }
    }
}
