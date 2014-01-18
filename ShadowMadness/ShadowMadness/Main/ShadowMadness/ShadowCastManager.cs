using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GDLibrary.Managers;
using Microsoft.Xna.Framework.Input;
using GDLibrary;

namespace ShadowMadness
{
    public class ShadowCastManager : GameComponent
    {
        public Vector2 shadowCastingPosition;
        private bool paused;
        public Color shadowColor;

        private Color light;
        private Color dark;

        //when there is a pause we fill this with shit.
        //if it's still paused we leave it // if still the same casting point.
        //else we clear.

        //needs access to all the collidables from main.
        public List<Line> contactLines;

        //local copy
        Main game;

        public ShadowCastManager(Main game) : base(game)
        {
            this.game = game;
            this.shadowCastingPosition = Vector2.Zero;
            this.paused = false;
            this.contactLines = new List<Line>();
            this.light = new Color(0, 0, 0, 60);
            this.dark = new Color(0, 0, 0, 120);
            this.shadowColor = light;
        }

        public override void Update(GameTime gameTime)
        {
            //if the left mouse is not pressed, update the position.
            if (!game.MOUSEMANAGER.newState.LeftButton.Equals(ButtonState.Pressed))
            {
                this.shadowColor = light;
                paused = false;
                contactLines.Clear();

                shadowCastingPosition.X = game.MOUSEMANAGER.newState.X;
                shadowCastingPosition.Y = game.MOUSEMANAGER.newState.Y;

                shadowCastingPosition += game.CAMERA2D.TRANSLATION;
                shadowCastingPosition.X -= game.graphics.PreferredBackBufferWidth / 2;
                shadowCastingPosition.Y -= game.graphics.PreferredBackBufferHeight / 2;
            }
            else
            {
                if (!paused)
                {
                    this.shadowColor = dark;
                    //do all the setup stuff
                    //fill the line List with contact lines. -> collidables from main.
                    List<Rectangle> shadowCasters = game.SHADOWCASTERS;

                    for (int i = 0; i < shadowCasters.Count; i++)
                    {
                        contactLines.AddRange(Util.contactLineFactory(shadowCasters[i], this.shadowCastingPosition, GameData.WINDOW_SIZE));
                    }
                    paused = true;
                }
            }
        }
    }
}
