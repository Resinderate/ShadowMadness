using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary.Utilities;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class BSPCollisionSectorLayout
    {
        private int rows, columns, sectorWidth, sectorHeight;
        private int sectorCount;

        #region PROPERTIES
        public int Rows
        {
            get
            {
                return this.rows;
            }
        }
        public int Columns
        {
            get
            {
                return this.columns;
            }
        }
        public int SectorWidth
        {
            get
            {
                return this.sectorWidth;
            }
        }
        public int SectorHeight
        {
            get
            {
                return this.sectorHeight;
            }
        }
        public int SectorCount
        {
            get
            {
                return this.sectorCount;
            }
        }
        #endregion

        public BSPCollisionSectorLayout(int rows, int columns, Rectangle screenBounds)
        {
            //number of rows x columns e.g. in a system with 8 sectors we could have 2x4 or 4x2
            this.rows = rows;
            this.columns = columns;

            //dimensions in pixels of each sector
            this.sectorWidth = screenBounds.Width / columns;
            this.sectorHeight = screenBounds.Height / rows;

            //total count of sectors e.g. 4, 8, 16, 25, 32
            this.sectorCount = rows * columns;
        }

    }
}
