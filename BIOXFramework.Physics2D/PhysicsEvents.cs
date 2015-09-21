using System;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Physics2D
{
    public class Collide2DEventArgs : EventArgs
    {
        public Collide2DEventArgs(GameComponent component1, GameComponent component2)
        {
            Component1 = component1;
            Component2 = component2;
        }

        public GameComponent Component1 { get; private set; }
        public GameComponent Component2 { get; private set; }
    }
}