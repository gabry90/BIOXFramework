using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;
using BIOXFramework.Input.Utility;
using BIOXFramework.GUI.Utility;
using System.Text;

namespace BIOXFramework.GUI.Components
{
    public class TextBox : AnimatedGuiBase
    {
        #region vars

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public Color TextColor = Color.White;
        public SpriteFont Font;
        public TextAlignments TextAlignement = TextAlignments.Center;
        public int SpacingFromBorder = 5;
        public bool IsPassword = false;
        public int MaxLenght = 255;
        public string Text
        {
            get { return inputTextProcessor.CurrentText; }
            set { inputTextProcessor.CurrentText = value; }
        }

        private InputTextProcessor inputTextProcessor;
        private string lastText;
        private string visibleText = "";
        private Vector2 textPosition = Vector2.Zero;

        #endregion

        #region constructors

        public TextBox(Game game, string name, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position, SpriteFont font, string text = "")
            : base(game, name, animatedTexture, animations, position)
        {
            inputTextProcessor = new InputTextProcessor(game);
            Text = text;
            Font = font;
        }

        #endregion

        #region base implementation

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

        public override void Update(GameTime gameTime)
        {
            if (Text.Length > MaxLenght)
                Text = Text.Remove(Text.Length - 1);

            if (lastText == null)
                lastText = Text;

            if (!string.Equals(lastText, Text))
            {
                TextChangedEventDispatcher(new TextChangedEventArgs(lastText, Text));
                lastText = Text;
            }

            Rectangle rect = GetRectangle();

            if (SpacingFromBorder > 0)
            {
                rect.Width = rect.Width - (SpacingFromBorder * 2);
                if (rect.Width < 0) rect.Width = 0;
                rect.X = rect.X + SpacingFromBorder;
            }

            string finalText = "";

            if (IsPassword)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < Text.Length; i++)
                    builder.Append("*");
                finalText = builder.ToString();
            }
            else
                finalText = Text;

            switch (TextAlignement)
            {
                case TextAlignments.Center:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, finalText);
                    textPosition = TextHelper.GetPositionAligned(TextAlignement, rect, Font, visibleText);
                    break;
                case TextAlignments.Left:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, finalText);
                    textPosition = TextHelper.GetPositionAligned(TextAlignement, rect, Font, visibleText);
                    break;
                case TextAlignments.Right:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, finalText);
                    textPosition = TextHelper.GetPositionAligned(TextAlignement, rect, Font, visibleText);
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, visibleText, textPosition, TextColor);
            spriteBatch.End();
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