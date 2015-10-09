using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;

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
                if (string.IsNullOrEmpty(value))
                {
                    currentText = "";
                    cursorPosition = 0;
                }
                else
                {
                    if (value.Length > currentText.Length)
                        cursorPosition = value.Length;
                    else
                    {
                        if (value.Length < cursorPosition)
                            cursorPosition = value.Length;
                    }
                    currentText = value;
                }
            }
        }
            
        public bool IsMaiuscActive 
        { 
            get { return keys.ContainsOneOrMore(Keys.LeftShift, Keys.RightShift) || Console.CapsLock; } 
        }
        public bool IsNumber
        {
            get 
            {
                int num;
                return Int32.TryParse(CurrentText, out num);
            }
        }
        public int CursorPosition
        {
            get { return cursorPosition; }
            set
            {
                if (value < 0)
                    cursorPosition = 0;
                else if (value > CurrentText.Length)
                    cursorPosition = CurrentText.Length;
                else
                    cursorPosition = value;
            }
        }

        private ExtendedList<Keys> keys;
        private string currentText;
        private int cursorPosition = 0;
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

        private void UpdateTextPosition(Keys key)
        {
            //update input text index position
            switch (key)
            {
                case Keys.Left:
                {
                    cursorPosition--;
                    if (cursorPosition < 0)
                        cursorPosition = 0;
                    break;
                }
                case Keys.Right:
                {
                    cursorPosition++;
                    if (cursorPosition > CurrentText.Length)
                        cursorPosition = CurrentText.Length;
                    break;
                }
            }
        }

        private void UpdateText(Keys key)
        {
            if (key == Keys.Back && cursorPosition > 0)
            {
                currentText = currentText.Length == 0 ? "" : currentText.Remove(cursorPosition - 1, 1);
                if (cursorPosition >= currentText.Length) cursorPosition--;
                return;
            }

            if (key == Keys.Delete && cursorPosition < CurrentText.Length)
            {
                currentText = currentText.Length == 0 ? "" : currentText.Remove(cursorPosition, 1);
                return;
            }

            Char? text = KeyboardHelper.ConvertKeyToChar(key, IsMaiuscActive);
            if (text.HasValue)
            {
                if (currentText == null)
                    currentText = "";
                else
                {
                    if (cursorPosition < currentText.Length)
                        currentText = currentText.Insert(cursorPosition, text.Value.ToString());
                    else
                    {
                        currentText = string.Concat(currentText, text.Value);
                        cursorPosition++;
                    }
                }
            }
        }

        #endregion

        #region events

        private void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            keys.AddExclusive(e.Key);
            UpdateTextPosition(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            keys.AddExclusive(e.Key);
            UpdateTextPosition(e.Key);
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