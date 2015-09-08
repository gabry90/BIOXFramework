using System;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Input.Events
{
    public class MousePressedEventArgs : EventArgs
    {
        public MousePressedEventArgs(string name, MouseButtons? button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons? Button { get; private set; }
    }

    public class MousePressingEventArgs : EventArgs
    {
        public MousePressingEventArgs(string name, MouseButtons? button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons? Button { get; private set; }
    }

    public class MouseReleasedEventArgs : EventArgs
    {
        public MouseReleasedEventArgs(string name, MouseButtons? button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons? Button { get; private set; }
    }
}