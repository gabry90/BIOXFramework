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

            string visibleText = text;

            try
            {
                while ((font.MeasureString(visibleText).X) + spacingFromBorder >= rectWidth)
                {
                    if (visibleText.Length == 1)
                    {
                        visibleText = "";
                        break;
                    }
                    visibleText = visibleText.Remove(0, 1);
                }
            }
            catch { visibleText = text; }
            return visibleText;
        }
    }
}