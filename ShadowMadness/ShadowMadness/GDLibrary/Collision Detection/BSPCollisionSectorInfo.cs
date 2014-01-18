using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GDLibrary.Utilities;

namespace GDLibrary
{
    /// <summary>
    /// Stores the rectangle and sector number for each collision sector onscreen.
    /// The greater the number of collision sectors, the less CDCR tests and single sprite must perform.
    /// </summary>
    public class BSPCollisionSectorInfo
    {
        private static int currentSectorIndex = 0;     //index of the current sector, increases as we add, maximum is totalSectorNumber
        private Rectangle sectorBounds;    //bounding rectangle for each sector
        private int sectorNumber;             //sector number for each rectangle (must be power of 2)

        public int SectorNumber
        {
            get
            {
                return this.sectorNumber;
            }
        }
        public Rectangle SectorBounds
        {
            get
            {
                return this.sectorBounds;
            }
        }
        public BSPCollisionSectorInfo(int row, int column, int width, int height)
        {
            //every new sector gets a power of two sector number, e.g. 1, 2, 4, 8 etc
            this.sectorNumber = (int)Math.Pow(2, currentSectorIndex++);
            this.sectorBounds = new Rectangle(row * width, column * height, width, height);
        }

       
    }
}
