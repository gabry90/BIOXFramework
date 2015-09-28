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
        public TextAlignments TextAlignement = TextAlignments.Center;
        public int SpacingFromBorder = 5;
        public int MaxLenght = 255;
        public string Text
        {
            get { return inputTextProcessor.CurrentText; }
            set { inputTextProcessor.CurrentText = value; }
        }

        private InputTextProcessor inputTextProcessor;
        private string lastText;

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

        public override void Update(GameTime gameTime)
        {
            if (lastText == null)
                lastText = Text;

            if (!string.Equals(lastText, Text))
            {
                TextChangedEventDispatcher(new TextChangedEventArgs(lastText, Text));
                lastText = Text;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = Vector2.Zero;
            Rectangle rect = GetRectangle();
            string visibleText = Text;

            switch (TextAlignement)
            {
                case TextAlignments.Center:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, Text, 0);
                    position = TextHelper.GetPositionAligned(TextAlignement, rect, Font, visibleText, 0);
                    break;
                case TextAlignments.Left:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, visibleText, SpacingFromBorder);
                    position = TextHelper.GetPositionAligned(TextAlignement, rect, Font, Text, SpacingFromBorder);
                    break;
                case TextAlignments.Right:
                    visibleText = TextHelper.GetVisibleText(rect.Width, Font, visibleText, SpacingFromBorder);
                    position = TextHelper.GetPositionAligned(TextAlignement, rect, Font, Text, SpacingFromBorder);
                    break;
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, visibleText, position, TextColor);
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