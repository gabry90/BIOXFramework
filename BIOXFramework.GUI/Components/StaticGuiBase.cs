using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.GUI.Components
{
    public class StaticGuiBase : GuiBase
    {
        #region constructors

        public StaticGuiBase(Game game, Texture2D texture, Vector2 position)
            : base(game, texture, position)
        { }

        #endregion

        #region component implementations

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, GetRectangle(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}