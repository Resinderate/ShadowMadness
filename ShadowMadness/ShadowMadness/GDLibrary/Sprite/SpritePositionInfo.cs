using System;
using Microsoft.Xna.Framework;
using GDLibrary.Utilities;

namespace GDLibrary
{
    
    public class SpritePositionInfo : ICloneable
    {
        private int sectorNumber;

        //Rectangle defining where we draw the original image on screen. Maximum is (0,0,graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight)
        private Rectangle destinationRectangle;
      
        //Measured in radians
        private float rotationInDegrees;
        private float scale;
        //Position onscreen of the sprite
        private Vector2 translation;
        //Origin defines the point of rotation for the texture - by default it is (0,0)
        private Vector2 origin;
        private Matrix worldMatrix;
        private Rectangle originalDestinationRectangle;

        //set to true when bounds needs to be updated - see Update() and properties (T, C, R, S)
        private bool bUpdate;
        private Matrix inverseWorldMatrix;

        #region PROPERTIES
        public int SECTORNUMBER
        {
            get
            {
                return this.sectorNumber;
            }
        }
        public Matrix INVERSEWORLDMATRIX
        {
            get
            {
                return this.inverseWorldMatrix;
            }

        }     
        public Matrix WORLDMATRIX
        {
            get 
            {
                return this.worldMatrix;
            }

        }     
       
        public float SCALE
        {
            get
            {
                return scale;
            }
            set
            {
                scale = (value >= 0) ? value : 1;
                bUpdate = true;
            }
        }
        public float ROTATIONINDEGREES
        {
            get
            {
                return rotationInDegrees;
            }
            set
            {
                rotationInDegrees = value;
                bUpdate = true;
            }
        }
        public float TRANSLATIONX
        {
            get
            {
                return translation.X;
            }
            set
            {
                translation.X = value;
                bUpdate = true;
            }
        }
        public float TRANSLATIONY
        {
            get
            {
                return translation.Y;
            }
            set
            {
                translation.Y = value;
                bUpdate = true;
            }
        }

        public Vector2 TRANSLATION
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;
                bUpdate = true;
            }
        }
        public Vector2 ORIGIN
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }
        public Rectangle BOUNDS
        {
            get
            {
                return destinationRectangle;
            }
        }
        public int BOUNDSWIDTH
        {
            get
            {
                return destinationRectangle.Width;
            }
            set
            {
                //destinationRectangle.Width = value;
                bUpdate = true;
                originalDestinationRectangle.Width = value;
            }
        }
        public int BOUNDSHEIGHT
        {
            get
            {
                return destinationRectangle.Height;
            }
            set
            {
                //destinationRectangle.Height = value;
                originalDestinationRectangle.Height = value;
                bUpdate = true;
            }
        }
        #endregion

        public SpritePositionInfo(Vector2 translation, int width, int height, float rotationInDegrees, float scale, Vector2 origin)
        {
            //call the property for the next four variables
            
            this.origin = origin;
            this.scale = scale;
            this.translation = translation;
            this.rotationInDegrees = rotationInDegrees;

            this.originalDestinationRectangle = new Rectangle(0, 0, width, height);
            setCollisionData(); //initial setup for world matrix, bounding rectangle, and collsion sector
        }

        public SpritePositionInfo(Vector2 translation, int width, int height)
        {
            this.origin = Vector2.Zero;
            this.scale = 1;
            this.translation = translation;
            this.rotationInDegrees = 0;

            this.originalDestinationRectangle = new Rectangle(0,0, width, height);
            setCollisionData(); //initial setup for world matrix, bounding rectangle, and collsion sector
        }

        public Object Clone()
        {
            //perform a deep copy clone of all value types (i.e. primitives) and c# reference types (e.g. Rectangle)
            return this.MemberwiseClone();
        }

        #region Non AA CDCR
        private void setCollisionData()
        {
            this.worldMatrix
              = Matrix.CreateTranslation(new Vector3(-origin, 0))
              * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationInDegrees))
              * Matrix.CreateScale(scale, scale, 1)
              * Matrix.CreateTranslation(new Vector3(translation, 0));

            //used by Collision::IntersectsNonAA()
            this.inverseWorldMatrix = Matrix.Invert(this.worldMatrix);

            //create the current bounding box by transforming the original destination rectangle by O, T, R, and S
            this.destinationRectangle
                = Collision.CalculateTransformedBoundingRectangle(originalDestinationRectangle, worldMatrix);

            //if something changes then update sector number
            this.sectorNumber = Collision.getSectorNumber(destinationRectangle);

        }

        /// <summary>
        /// Called from Sprite::Update() to update collision data based on clean/dirty flag
        /// </summary>
        public void Update()
        {
            if (bUpdate) //something changed that warrants a bounds update
            {
                setCollisionData();
                bUpdate = false;
            }
        }
        #endregion
    }
}
