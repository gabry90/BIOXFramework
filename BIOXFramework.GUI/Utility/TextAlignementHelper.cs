using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.GUI.Utility
{
    public static class TextAlignementHelper
    {
        public static Vector2 AlignCenter(Rectangle rect, SpriteFont font, string text)
        {
            Vector2 textDimens = font.MeasureString(text);

            int midRectWidth = (rect.X + rect.Width) / 2;
            int midRectHeight = (rect.Y + rect.Height) / 2;
            int midTextWidth = (int)textDimens.X / 2;
            int midTextHeight = (int)textDimens.Y / 2;

            return new Vector2(midRectWidth - midTextWidth, midRectHeight - midTextHeight);
        }

        public static Vector2 AlignLeft(Rectangle rect, SpriteFont font, string text, int spacingFromLeft)
        {
            Vector2 textDimens = font.MeasureString(text);

            int midRectHeight = (rect.Y + rect.Height) / 2;
            int midTextHeight = (int)textDimens.Y / 2;

            return new Vector2(rect.X + spacingFromLeft, midRectHeight - midTextHeight);
        }

        public static Vector2 AlignRight(Rectangle rect, SpriteFont font, string text, int spacingFromRight)
        {
            Vector2 textDimens = font.MeasureString(text);

            int midRectHeight = (rect.Y + rect.Height) / 2;
            int midTextHeight = (int)textDimens.Y / 2;

            return new Vector2((rect.X + rect.Width) - textDimens.X - spacingFromRight, midRectHeight - midTextHeight);
        }
    }
}