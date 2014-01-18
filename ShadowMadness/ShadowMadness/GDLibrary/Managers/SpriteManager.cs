using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMadness;

namespace GDLibrary.Managers
{
    public class SpriteManager : DrawableGameComponent
    {
        private static Main game;

        public List<Sprite> list;
        protected bool bPause = false;

        #region PROPERTIES
        public static Main GAME
        {
            get
            {
                return game;
            }
        }
        public bool PAUSE
        {
            get
            {
                return bPause;
            }
            set
            {
                bPause = value;
            }
        }
        public Sprite this[int index]
        {
            get
            {
                return list[index];
            }
        }
        #endregion

        public SpriteManager(Main theGame)
            : base(theGame)
        {
            this.list = new List<Sprite>();

            //store static pointer to game
            game = theGame;
        }
        public void Add(Sprite sprite)
        {
            list.Add(sprite);
        }
        public bool Remove(Sprite sprite)
        {
            return list.Remove(sprite);
        }
        public int Size()
        {
            return list.Count;
        }


        public bool Remove(string name)
        {
            for (int i = 0; i < list.Count; i++)
            { 
                Sprite sprite = list[i];
                if (sprite.NAME.Equals(name))
                {
                    return list.Remove(sprite);
                }
            }
            return false;
        }

        public Sprite Get(string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Sprite sprite = list[i];
                if (sprite.NAME.Equals(name))
                {
                    return sprite;
                }
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            if (!bPause)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Update(gameTime);
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {

            game.SPRITEBATCH.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp,
                    DepthStencilState.Default, RasterizerState.CullNone,
                    null, game.CAMERA2D.MATRIX);

            //fix - nmcg
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Draw(gameTime);
            }

            //draw more stuff here.
            List<List<Vector2>> shadows = game.shadows;

            for(int i = 0; i < shadows.Count; i++)
            {
                game.draw2D.TriangleStrip(shadows[i], game.shadowCastManager.shadowColor);
            }
            
            
            game.SPRITEBATCH.End();
            
            //GAME.basicEffect.Projection = GAME.CAMERA2D.MATRIX;
            //GAME.basicEffect.CurrentTechnique.Passes[0].Apply();
            //GAME.graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, GAME.lineVerts, 0, 1);
        }


    }
}






