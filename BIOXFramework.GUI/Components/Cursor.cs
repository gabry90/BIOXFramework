using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input.Events;

namespace BIOXFramework.GUI.Components
{
    public class Cursor : GuiBase
    {
        public Cursor(Game game, Texture2D texture)
            : base(game, texture)
        { }

        public Cursor(Game game, Texture2D texture, int columns, int rows)
            : base(game, texture, columns, rows)
        { }

        protected override void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {
            if (Visible)
                Position = new Vector2(e.Position.X, e.Position.Y);

            base.OnMousePositionChanged(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible || Texture == null || (isTextureAtlas && textureAtlas != null))
                return;

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}