using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GDLibrary
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyboardManager : Microsoft.Xna.Framework.GameComponent
    {
        protected KeyboardState newState, oldState;
        public KeyboardManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //store the old keyboard state for later comparison
            oldState = newState; 
            //get the current state in THIS update
            newState = Keyboard.GetState();
            base.Update(gameTime);
        }
        /// <summary>
        /// Detects first press of a user-defined key
        /// </summary>
        /// <param name="key">Test this key for first press</param>
        /// <returns>true if first press, otherwise false</returns>
        public bool isFirstKeyPress(Keys key)
        {
            //is this the first press for this key????
            if (newState.IsKeyDown(key) && oldState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        public bool isKeyDown(Keys key)
        {
            return newState.IsKeyDown(key);
        }
        public bool isStateChanged()
        {
            return (newState.Equals(oldState)) ? false : true;
            //return (age >= 21) ? "adult" : "not adult";
        }
    }
}
