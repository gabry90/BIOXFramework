using System;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Events
{
    public class KeyboardPressedEventArgs : EventArgs
    {
        public KeyboardPressedEventArgs(Keys key) { Key = key; }
        public Keys Key { get; private set; }
    }

    public class KeyboardPressingEventArgs : EventArgs
    {
        public KeyboardPressingEventArgs(Keys key) { Key = key; }
        public Keys Key { get; private set; }
    }

    public class KeyboardReleaseEventArgs : EventArgs
    {
        public KeyboardReleaseEventArgs(Keys key) { Key = key; }
        public Keys Key { get; private set; }
    }
}