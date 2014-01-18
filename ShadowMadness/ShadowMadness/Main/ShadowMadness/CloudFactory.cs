using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GDLibrary.Managers;
using GDLibrary;

namespace ShadowMadness
{
    public class CloudFactory : GameComponent
    {
        private float minSpeed;
        private int maxClouds;
        private Rectangle boundary;
        private List<Cloud> currentClouds;
        private List<Rectangle> cloudSources;
        private Random random;
        private float backgroundDepth;

        public CloudFactory(Main game, List<Rectangle> cloudSources, float minSpeed, int maxClouds, Rectangle boundary) : 
            base(game)
        {
            this.minSpeed = minSpeed;
            this.maxClouds = maxClouds;
            this.boundary = boundary;   //level boundary
            this.currentClouds = new List<Cloud>();
            this.cloudSources = cloudSources;
            this.random = new Random();
            this.backgroundDepth = 0.4f;
        }

        public override void Update(GameTime gameTime)
        {

            if (currentClouds.Count < maxClouds)
            {
                addNewCloud();
            }

            checkCloudsLeavingScreen();

            base.Update(gameTime);
        }

        private void addNewCloud()
        {
            float speed;
            int startHeight;
            int randomSourceIndex;
            Vector2 startPos;

            startHeight = random.Next(boundary.X, (int)(boundary.Height * .75f)); //0 - 2400 ~

            speed =  0.05f + ((float)random.NextDouble()) * minSpeed; //just using minspeed for now.

            randomSourceIndex = random.Next(0, cloudSources.Count);

            //start off the screen to the left
            startPos.X = 0 - (cloudSources[randomSourceIndex].Width / 2);
            startPos.Y = startHeight;

            Cloud newCloud = new Cloud(speed, "cloud" + random.Next(999),
                                                SpriteManager.GAME.TEXTUREMANAGER.Get("cloudsheet"),
                                                new SpritePresentationInfo(cloudSources[randomSourceIndex], backgroundDepth),
                                                new SpritePositionInfo(startPos, cloudSources[randomSourceIndex].Width, cloudSources[randomSourceIndex].Height));
            this.currentClouds.Add(newCloud);
            SpriteManager.GAME.SPRITEMANAGER.Add(newCloud);
        }

        private void checkCloudsLeavingScreen()
        {
            for (int i = 0; i < currentClouds.Count; i++)
            {
                if (checkCloudOutOfBoundary(currentClouds[i].POSITIONINFO.BOUNDS))
                {
                    //remove
                    SpriteManager.GAME.SPRITEMANAGER.Remove(currentClouds[i]);
                    currentClouds.RemoveAt(i);
                }
            }
        }

        private bool checkCloudOutOfBoundary(Rectangle cloudRect)
        {
            if (Collision.Intersects(cloudRect, boundary))
                return false;
            return true;
        }
    }
}
