using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class TextBox : AnimatedGuiBase
    {
        #region vars

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public Color TextColor = Color.White;
        public SpriteFont Font;
        public TextAlignements TextAlignement = TextAlignements.Center;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private string text;
        private TextProcessor textpProcessor;

        #endregion

        #region constructors

        public TextBox(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position, SpriteFont font, string text = "")
            : base(game, animatedTexture, animations, position)
        {
            textpProcessor = new TextProcessor(game);
            Text = text;
            Font = font;
        }

        #endregion

        #region gui base implementation

        protected override void OnFocused(object sender, EventArgs e)
        {
            textpProcessor.Process();
            base.OnFocused(sender, e);
        }

        protected override void OnLostFocus(object sender, EventArgs e)
        {
            textpProcessor.Unprocess();
            base.OnLostFocus(sender, e);
        }

        #endregion

        #region component implementation

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion

        #region dispatchers

        private void TextChangedEventDispatcher(TextChangedEventArgs e)
        {
            var h = TextChanged;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (TextChanged != null) TextChanged = null;
                    textpProcessor.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}