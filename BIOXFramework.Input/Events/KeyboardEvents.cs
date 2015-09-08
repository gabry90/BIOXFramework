using System;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Input.Events
{
    public class KeyboardPressedEventArgs : EventArgs
    {
        public KeyboardPressedEventArgs(string name, Keys key) 
        {
            Name = name;
            Key = key;
        }

        public string Name { get; private set; }
        public Keys Key { get; private set; }
    }

    public class KeyboardPressingEventArgs : EventArgs
    {
        public KeyboardPressingEventArgs(string name, Keys key) 
        {
            Name = name;
            Key = key;
        }

        public string Name { get; private set; }
        public Keys Key { get; private set; }
    }

    public class KeyboardReleasedEventArgs : EventArgs
    {
        public KeyboardReleasedEventArgs(string name, Keys key) 
        {
            Name = name;
            Key = key;
        }

        public string Name { get; private set; }
        public Keys Key { get; private set; }
    }
}