using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;

namespace BIOXFramework.Input.Utility
{
    public class InputTextProcessor : IDisposable
    {
        #region keys pressed enumerator

        private static class KeyPressedEnumerator
        {
            private static List<Keys> keys = new List<Keys>();

            public static void Add(Keys key)
            {
                if (!keys.Contains(key))
                    keys.Add(key);
            }

            public static void Remove(Keys key)
            {
                if (keys.Contains(key))
                    keys.Remove(key);
            }

            public static bool ContainsAll(params Keys[] keys)
            {
                return KeyPressedEnumerator.keys.Count(x => keys.Contains(x)) == keys.Count();
            }

            public static bool ContainsOneOrMore(params Keys[] keys)
            {
                return KeyPressedEnumerator.keys.Count(x => keys.Contains(x)) > 0;
            }

            public static Keys? Get(int index)
            {
                return (index >= keys.Count || index < 0) ? null : (Keys?)keys[index];
            }

            public static List<Keys> Get()
            {
                return keys.Select(x => x).ToList();
            }

            public static void Clear()
            {
                keys.Clear();
            }
        }

        #endregion

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
            get { return KeyPressedEnumerator.ContainsOneOrMore(Keys.LeftShift, Keys.RightShift) || Console.CapsLock; } 
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

        private string currentText;
        private int cursorPosition = 0;
        private KeyboardManager manager;
        private Game game;

        #endregion

        #region constructors

        public InputTextProcessor(Game game) 
        {
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
            KeyPressedEnumerator.Add(e.Key);
            UpdateTextPosition(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            KeyPressedEnumerator.Add(e.Key);
            UpdateTextPosition(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyRelease(object sender, KeyboardReleasedEventArgs e)
        {
            KeyPressedEnumerator.Remove(e.Key);
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
            KeyPressedEnumerator.Clear();
        }

        #endregion
    }
}