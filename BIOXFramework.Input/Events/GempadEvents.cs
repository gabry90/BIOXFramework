using System;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Events
{
    public class GamepadPressedEventArgs : EventArgs
    {
        public GamepadPressedEventArgs(string name, Buttons button) 
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public Buttons Button { get; private set; }
    }

    public class GamepadPressingEventArgs : EventArgs
    {
        public GamepadPressingEventArgs(string name, Buttons button) 
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public Buttons Button { get; private set; }
    }

    public class GamepadReleasedEventArgs : EventArgs
    {
        public GamepadReleasedEventArgs(string name, Buttons button) 
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public Buttons Button { get; private set; }
    }
}