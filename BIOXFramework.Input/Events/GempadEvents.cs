using System;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Events
{
    public class GamepadPressedEventArgs : EventArgs
    {
        public GamepadPressedEventArgs(Buttons button) { Button = button; }
        public Buttons Button { get; private set; }
    }

    public class GamepadPressingEventArgs : EventArgs
    {
        public GamepadPressingEventArgs(Buttons button) { Button = button; }
        public Buttons Button { get; private set; }
    }

    public class GamepadReleaseEventArgs : EventArgs
    {
        public GamepadReleaseEventArgs(Buttons button) { Button = button; }
        public Buttons Button { get; private set; }
    }
}