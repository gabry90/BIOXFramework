using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class Cursor : GuiBase, INonPausableComponent, IPersistentComponent
    {
        #region constructors

        public Cursor(Game game, Texture2D texture)
            : base(game, texture)
        { }

        public Cursor(Game game, Texture2D texture, List<AnimatedTextureRegion> regions)
            : base(game, texture, regions)
        { }

        #endregion

        #region overridden base methods

        protected override void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {
            if (Visible)
                Position = new Vector2(e.Position.X, e.Position.Y);

            base.OnMousePositionChanged(sender, e);
        }

        #endregion

        #region game implementations

        public override void Draw(GameTime gameTime)
        {
            if (!Visible || Texture == null || (isTextureAtlas && animatedTexture != null))
                return;

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}