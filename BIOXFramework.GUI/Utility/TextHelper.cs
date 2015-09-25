using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.GUI.Utility
{
    public enum TextAlignments
    {
        Center,
        Left,
        Right
    }

    public static class TextHelper
    {
        public static Vector2 GetPositionAligned(TextAlignments alignement, Rectangle rect, SpriteFont font, string text, int spacingFromBorder)
        {
            Vector2 textDimens = font.MeasureString(text);

            int midRectWidth = (rect.X + rect.Width) / 2;
            int midRectHeight = (rect.Y + rect.Height) / 2;
            int midTextWidth = (int)textDimens.X / 2;
            int midTextHeight = (int)textDimens.Y / 2;

            switch (alignement)
            {
                case TextAlignments.Center: return new Vector2(midRectWidth - midTextWidth, midRectHeight - midTextHeight);
                case TextAlignments.Left: return new Vector2(rect.X + spacingFromBorder, midRectHeight - midTextHeight);
                case TextAlignments.Right: return new Vector2((rect.X + rect.Width) - textDimens.X - spacingFromBorder, midRectHeight - midTextHeight);
                default : return Vector2.Zero;
            }
        }

        public static string GetVisibleText(int rectWidth, SpriteFont font, string text, int spacingFromBorder)
        {
            if (rectWidth <= (0 + spacingFromBorder))
                return "";

            while ((font.MeasureString(text).X) + spacingFromBorder >= rectWidth)
            {
                if (text.Length == 1)
                {
                    text = "";
                    break;
                }
                text = text.Remove(0, 1);
            }
            
            return text;
        }
    }
}