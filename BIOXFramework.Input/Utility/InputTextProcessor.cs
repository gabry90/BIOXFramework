using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;
using System.Globalization;

namespace BIOXFramework.Input.Utility
{
    public class InputTextProcessor : IDisposable
    {
        #region vars

        public string CurrentText
        {
            get { return currentText; }
            set 
            {
                currentText = value;
                UpdateCursorPosition(null);
            }
        }
            
        public bool IsMaiuscActive 
        { 
            get 
            { 
                bool shifted = keys.ContainsOneOrMore(Keys.LeftShift, Keys.RightShift);
                return ((Console.CapsLock && !shifted) || (!Console.CapsLock && shifted)); 
            } 
        }

        public bool IsNumber
        {
            get 
            {
                int num;
                return Int32.TryParse(CurrentText, out num);
            }
        }

        public float CursorPosition
        {
            get { return cursorPosition; }
            set 
            {
                UpdateCursorPosition(value);
                FixCursorPosition();
            }
        }

        private ExtendedList<Keys> keys;
        private string currentText;
        private float cursorPosition = -0.5f;
        private KeyboardManager manager;
        private Game game;

        #endregion

        #region constructors

        public InputTextProcessor(Game game) 
        {
            keys = new ExtendedList<Keys>();
            keys.EnableRaisingEvents = false;
            manager = new KeyboardManager(game);
            manager.PressingDelay = 100;
            manager.Pressed += OnKeyPressed;
            manager.Pressing += OnKeyPressing;
            manager.Released += OnKeyRelease;
            manager.EnableCapture = false;
            game.Components.Add(manager);
            this.game = game;
        }

        #endregion

        #region public methods

        public void Process()
        {
            manager.EnableCapture = true;
        }

        public void Unprocess()
        {
            manager.EnableCapture = false;
        }

        #endregion

        #region private methods

        private int GetDecimalPart(float number)
        {
            return Int32.Parse(number.ToString("0.0######", CultureInfo.InvariantCulture).Split('.')[1]);
        }

        private int GetIntPart(float number)
        {
            return Int32.Parse(number.ToString("0.0", CultureInfo.InvariantCulture).Split('.')[0]);
        }

        private Tuple<int, int> GetIndexesFromCursorPosition()
        {
            int intPart = GetIntPart(cursorPosition);
            return new Tuple<int, int>(cursorPosition < 0f ? 0 : intPart, cursorPosition < 0f ? 0 : intPart + 1);
        }

        private void FixCursorPosition()
        {
            int decimalPart = GetDecimalPart(cursorPosition);
            if (decimalPart != 5)
                cursorPosition = Convert.ToSingle(string.Concat(GetIntPart(cursorPosition), decimalPart));
        }

        //update cursor position from new or current position
        private void UpdateCursorPosition(float? pos = null)
        {
            float newPosition = pos.HasValue ? pos.Value : cursorPosition;

            if (newPosition < -0.5f)
                cursorPosition = -0.5f;
            else if (newPosition > ((float)currentText.Length - 0.5f))
                cursorPosition = (float)currentText.Length - 0.5f;
            else
                cursorPosition = newPosition;
        }

        //update cursor position by key
        private void UpdateCursorPosition(Keys key)
        {
            float currentPosition = cursorPosition;
            switch (key)
            {
                case Keys.Left:
                    currentPosition--;
                    break;
                case Keys.Right:
                    currentPosition++;
                    break;
            }
            UpdateCursorPosition(currentPosition);
        }

        private void UpdateText(Keys key)
        {
            Tuple<int, int> indexes = GetIndexesFromCursorPosition();

            if (key == Keys.Back && currentText.Length > 0 && cursorPosition > -0.5f)
            {
                currentText = currentText.Remove(indexes.Item1, 1);
                UpdateCursorPosition();
                return;
            }

            if (key == Keys.Delete && currentText.Length > 0 && cursorPosition < (float)currentText.Length - 1f)
            {
                currentText = currentText.Remove(indexes.Item2, 1);
                UpdateCursorPosition();
                return;
            }

            Char? text = KeyboardHelper.ConvertKeyToChar(key, IsMaiuscActive);
            if (!text.HasValue)
                return;

            if (currentText == null || currentText.Length == 0)
                currentText = text.Value.ToString();
            else
            {
                if (cursorPosition < (float)currentText.Length - 1f) 
                    currentText = currentText.Insert(indexes.Item1, text.Value.ToString());
                else 
                    currentText = string.Concat(currentText, text.Value);
            }

            UpdateCursorPosition(cursorPosition + 1f);
        }

        #endregion

        #region events

        private void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            keys.AddExclusive(e.Key);
            UpdateCursorPosition(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            keys.AddExclusive(e.Key);
            UpdateCursorPosition(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyRelease(object sender, KeyboardReleasedEventArgs e)
        {
            keys.Remove(e.Key);
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            manager.Pressed -= OnKeyPressed;
            manager.Pressing -= OnKeyPressing;
            manager.Released -= OnKeyRelease;
            manager.Dispose();
            game.Components.Remove(manager);
            keys.Dispose();
        }

        #endregion
    }
}