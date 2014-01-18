using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ShadowMadness;

namespace GDLibrary
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MouseManager : Microsoft.Xna.Framework.GameComponent
    {
        public MouseState newState, oldState;

        //a simple 1x1 bounding rectangle for the mouse pointer
        public Rectangle bounds;

        private Main game;
        private bool isVisible;

        public MouseManager(Main game, bool isVisible)
            : base(game)
        {
            //allows us to set mouse visibility
            this.isVisible = isVisible;
            game.IsMouseVisible = isVisible;


            this.game = game;
            this.bounds = new Rectangle(0,0,1,1);
            this.game.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void setOldState()
        {
            this.oldState = newState;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            setOldState();

            this.newState = Mouse.GetState();

            //nmcg - mouse - 4 - reset the 1x1 pixel bounds rectangle everytime the mouse moves
            this.bounds.X = newState.X;
            this.bounds.Y = newState.Y;

            base.Update(gameTime);


        }
    }
}