using System;
using BIOXFramework.Input.Mappers;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Input.Events
{
    public class MousePressedEventArgs : EventArgs
    {
        public MousePressedEventArgs(string name, MouseButtons button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons Button { get; private set; }
    }

    public class MousePressingEventArgs : EventArgs
    {
        public MousePressingEventArgs(string name, MouseButtons button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons Button { get; private set; }
    }

    public class MouseReleasedEventArgs : EventArgs
    {
        public MouseReleasedEventArgs(string name, MouseButtons button)
        {
            Name = name;
            Button = button;
        }

        public string Name { get; private set; }
        public MouseButtons Button { get; private set; }
    }

    public class MousePositionChangedEventArgs : EventArgs
    {
        public MousePositionChangedEventArgs(Point position) { Position = position; }
        public Point Position { get; private set; }
    }
}