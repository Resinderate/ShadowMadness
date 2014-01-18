using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMadness;

namespace GDLibrary
{
    /*
     * Performs camera transformations by transforming the viewport.
     * This will not affect the position, scale, or rotation of anything drawn on screen.
     */
    public class Camera2D : GameComponent
    {
        //minimum amount by which we can scale the Viewport (i.e. zoom out)
        private const float SCALE_MINIMUM = 0.05f;
        private const float MOVE_SPEED_INCREMENT = 0.1f;
        //roughly 1 degree of rotation every update if the correct key is pressed - assumes 16ms update
        private float ROTATION_SPEED_INCREMENT = MathHelper.ToRadians(1/16.0f);
        //how much to zoom in or out
        private const float SCALE_SPEED_INCREMENT = 0.001f; 

        //gets access to keyboard manager. could also use a service
        protected Main game;
        //See split screen example
        protected Viewport viewPort;

        //see SpriteManager::Draw()
        protected Matrix worldTransform; 

        //target position of camera (i.e. what position camera is looking at on-screen)
        protected Vector2 translation;

        //rotation and scale for camera
        protected float rotation, scale;

        //used by reset() - see handleInput()
        private Vector2 originalTranslation;
        private float originaRotation, originalScale;

        private int minY;
        private int maxY;
        private int minX;
        private int maxX;

        private float cameraElasticity;
        private Vector2 difference; //Should be a vector if dealing with 2 axis;

        #region PROPERTIES

        public Vector2 TRANSLATION
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;
            }
        }

        public float ROTATION
        {
            get 
            { 
                return rotation; 
            }
            set 
            { 
                rotation = value; 
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
                //if less than, set to minimum, otherwise set value
                scale = (value < SCALE_MINIMUM) ? SCALE_MINIMUM  : value;
            }
        }

        //called by SpriteManager::Draw() in spriteBatch.Begin()
        public Matrix MATRIX
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-translation.X, -translation.Y, 0)) *
                             Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(scale, scale, 1)) *
                                    Matrix.CreateTranslation(new Vector3(viewPort.Width/2, viewPort.Height/2, 0));

            }
        }
        //use these methods to move the camera either in XY or in X and Y independently
        public Vector2 MOVE
        {
            set
            {
               translation += value;
            }
        }
        public float MOVEX
        {
            set
            {
                translation.X += value;
            }
        }
        public float MOVEY
        {
            set
            {
                translation.Y += value;
            }
        }

        #endregion

        public Camera2D(Main game, Vector2 translation, float rotation, float scale)
            : base(game)
        {
            this.game = game;

            //we need access to viewport to get dimensions of screen
            //we cant use windowDimensions because we may want to create more than one viewport (i.e. for splitscreen)
            this.viewPort = game.GraphicsDevice.Viewport;

            //sets the position of the camera
            this.translation = translation;
       
            //sets any rotation around Z (i.e. coming out of the screen)
            this.rotation = rotation;

            //sets the zoom level (i.e. if > 1 zoom in, if < 1 zoom out, bounded at minimum value)
            //call property and not this.scale to ensure scale not set to less than minimum
            SCALE = scale;

            //stored for reset
            this.originalTranslation = translation;
            this.originaRotation = rotation;
            this.originalScale = scale;


            //Calc this properly, with game data etc.
            this.minY = 300;
            this.maxY = GameData.LEVEL_HEIGHT-300;
            this.minX = 400;
            this.maxX = GameData.LEVEL_WIDTH - 400;


            this.cameraElasticity = 0.1f;
            this.difference = Vector2.Zero;
        }

        public void reset()
        {
            this.translation = originalTranslation;
            this.rotation = originaRotation;
            //call property and not this.scale to ensure scale not set to less than minimum
            SCALE = originalScale;
        }


        //Dont think you need game time as it is just following the play.
        //the player is obeying gameTime
        private void followPlayer()
        {
            //difference in the Y from the player.
            difference = game.PLAYER.POSITIONINFO.TRANSLATION - this.translation;

            this.translation += difference * cameraElasticity;

            
            if (translation.Y > maxY)
                translation.Y = maxY;
            else if (translation.Y < minY)
                translation.Y = minY;


            if (translation.X > maxX)
                translation.X = maxX;
            else if (translation.X < minX)
                translation.X = minX;
            //need some stuff for the X axis too now.
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(translation.X + ", " + translation.Y);
            //might want to add keyboard control to the camera
            //disable or redefine movement based on your requirements
            followPlayer();
            //handleInput(gameTime);
         
            base.Update(gameTime);
        }
    }
}
