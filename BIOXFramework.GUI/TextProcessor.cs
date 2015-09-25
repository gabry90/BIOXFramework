using System;
using Microsoft.Xna.Framework;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;

namespace BIOXFramework.GUI
{
    internal class TextProcessor : IDisposable
    {
        #region vars

        public string CurrentText { get; private set; }

        private KeyboardManager manager;

        #endregion

        #region constructors

        public TextProcessor(Game game) 
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

        #region events

        private void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {

        }

        private void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {

        }

        private void OnKeyRelease(object sender, KeyboardReleasedEventArgs e)
        {

        }

        #endregion

        #region dispose

        public void Dispose()
        {
            manager.Pressed -= OnKeyPressed;
            manager.Pressing -= OnKeyPressing;
            manager.Released -= OnKeyRelease;
            manager.Dispose();
        }

        #endregion
    }
}