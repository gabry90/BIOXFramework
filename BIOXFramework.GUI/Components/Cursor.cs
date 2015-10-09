using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input.Events;

namespace BIOXFramework.GUI.Components
{
    public class Cursor : StaticGuiBase, IPersistentComponent
    {
        #region constructors

        public Cursor(Game game, Texture2D texture, Vector2 position)
            : base(game, null, texture, position)
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
    }
}