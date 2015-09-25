using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;
using BIOXFramework.Input.Utility;
using BIOXFramework.GUI.Utility;

namespace BIOXFramework.GUI.Components
{
    public class TextBox : AnimatedGuiBase
    {
        #region vars

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public Color TextColor = Color.White;
        public SpriteFont Font;
        public TextAlignements TextAlignement = TextAlignements.Center;
        public int SpacingFromBorder = 5;
        public string Text
        {
            get { return inputTextProcessor.CurrentText; }
            set { inputTextProcessor.CurrentText = value; }
        }

        private InputTextProcessor inputTextProcessor;

        #endregion

        #region constructors

        public TextBox(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position, SpriteFont font, string text = "")
            : base(game, animatedTexture, animations, position)
        {
            inputTextProcessor = new InputTextProcessor(game);
            Text = text;
            Font = font;
        }

        #endregion

        #region gui base implementation

        protected override void OnFocused(object sender, EventArgs e)
        {
            inputTextProcessor.Process();
            base.OnFocused(sender, e);
        }

        protected override void OnLostFocus(object sender, EventArgs e)
        {
            inputTextProcessor.Unprocess();
            base.OnLostFocus(sender, e);
        }

        #endregion

        #region component implementation

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = Vector2.Zero;
            Rectangle rect = GetRectangle();

            switch (TextAlignement)
            {
                case TextAlignements.Center:
                    position = TextAlignementHelper.AlignCenter(rect, Font, Text);
                    break;
                case TextAlignements.Left:
                    position = TextAlignementHelper.AlignLeft(rect, Font, Text, SpacingFromBorder);
                    break;
                case TextAlignements.Right:
                    position = TextAlignementHelper.AlignRight(rect, Font, Text, SpacingFromBorder);
                    break;
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, Text, position, TextColor);
            spriteBatch.End();

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
                    inputTextProcessor.Dispose();
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