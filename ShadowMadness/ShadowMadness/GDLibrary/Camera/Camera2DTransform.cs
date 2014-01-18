using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Camera2DTransform
    {
        //target position of camera (i.e. what position camera is looking at on-screen)
        protected Vector2 translation;

        //rotation and scale for camera
        protected float rotation, scale;

        //perform some transformation after N milliseconds
        protected double timeToPerformTransformMS;

        //minimum amount by which we can scale the Viewport (i.e. zoom out)
        private const float SCALE_MINIMUM = 0.05f;

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
        //used by Camera2DPathManager::lerpVector2()
        public float TRANSLATIONX
        {
            get
            {
                return translation.X;
            }
            set
            {
                translation.X = value;
            }
        }
        //used by Camera2DPathManager::lerpVector2()
        public float TRANSLATIONY
        {
            get
            {
                return translation.Y;
            }
            set
            {
                translation.Y = value;
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
                scale = (value < SCALE_MINIMUM) ? SCALE_MINIMUM : value;
            }
        }

        public double TIME
        {
            get
            {
                return timeToPerformTransformMS;
            }
            set
            {
                //if less than, set to 0
                timeToPerformTransformMS = (value > 0) ? value : 0;
            }
        }


        #endregion

        public Camera2DTransform(Vector2 translation, float rotation, float scale, double timeToPerformTransformMS)
        {
            //sets the position of the camera
            this.translation = translation;

            //sets any rotation around Z (i.e. coming out of the screen)
            this.rotation = rotation;

            //sets the zoom level (i.e. if > 1 zoom in, if < 1 zoom out, bounded at minimum value)
            this.scale = scale;

            TIME = timeToPerformTransformMS;
        }
    }
}
