using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.GUI.Components
{
    public class Label : StaticGuiBase
    {
        #region vars

        public string Text = "";
        public Color Color = Color.White;
        public SpriteFont Font;

        #endregion

        #region constructors

        public Label(Game game, string name, SpriteFont font, string text, Vector2 position)
            : base(game, name, null, position)
        {
            Text = text;
            Font = font;
        }

        #endregion

        #region component implementations

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, Text, Position, Color);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}