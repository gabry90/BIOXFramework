using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility.Extensions;

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
        public static Vector2 GetPositionAligned(TextAlignments alignment, Rectangle rect, SpriteFont font, string text)
        {
            if (string.IsNullOrEmpty(text))
                return Vector2.Zero;

            Vector2 textDimens = font.MeasureString(text);

            int midRectPosX = rect.X + (rect.Width / 2);
            int midRectPosY = rect.Y + (rect.Height / 2);
            int midTextWidth = (int)textDimens.X / 2;
            int midTextHeight = (int)textDimens.Y / 2;
            
            switch (alignment)
            {
                case TextAlignments.Center: return new Vector2(midRectPosX - midTextWidth, midRectPosY - midTextHeight);
                case TextAlignments.Left: return new Vector2(rect.X, midRectPosY - midTextHeight);
                case TextAlignments.Right: return new Vector2((rect.X + rect.Width) - textDimens.X, midRectPosY - midTextHeight);
                default : return Vector2.Zero;
            }
        }

        public static string GetVisibleText(int rectWidth, SpriteFont font, string text)
        {
            if (rectWidth <= 0)
                return "";

            if (string.IsNullOrEmpty(text))
                return text;

            string visibleText = text;

            try
            {
                while (font.MeasureString(visibleText).X >= rectWidth)
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

        public static Vector2 GetCursorPosition(float cursorIndex, Vector2 textPosition, SpriteFont font, string text)
        {
            int cursorIndexFixed = Convert.ToInt32(cursorIndex - 0.5f);
            string textUntilCursor = cursorIndexFixed < 0 ? "" : text.Substring(0, cursorIndexFixed + 1);
            return new Vector2(font.MeasureString(textUntilCursor).X + (font.Spacing / 2f), textPosition.Y);
        }
    }
}