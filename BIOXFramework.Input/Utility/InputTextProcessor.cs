using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using System.Runtime.InteropServices;

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
                return keys.Count(x => keys.Contains(x)) == keys.Count();
            }

            public static bool ContainsOneOrMore(params Keys[] keys)
            {
                return keys.Count(x => keys.Contains(x)) > 0;
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

        public string CurrentText { get; set; }
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

        private KeyboardManager manager;

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

        private void UpdateText(Keys key)
        {
            Char? text = KeyboardHelper.ConvertKeyToChar(key, IsMaiuscActive);
            if (text.HasValue)
                CurrentText = string.Concat(CurrentText == null ? "" : CurrentText, text);
        }

        #endregion

        #region events

        private void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            KeyPressedEnumerator.Add(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            KeyPressedEnumerator.Add(e.Key);
            UpdateText(e.Key);
        }

        private void OnKeyRelease(object sender, KeyboardReleasedEventArgs e)
        {
            KeyPressedEnumerator.Remove(e.Key);
            UpdateText(e.Key);
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            manager.Pressed -= OnKeyPressed;
            manager.Pressing -= OnKeyPressing;
            manager.Released -= OnKeyRelease;
            manager.Dispose();
            KeyPressedEnumerator.Clear();
        }

        #endregion
    }
}