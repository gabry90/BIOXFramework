using System;

namespace BIOXFramework.Scene
{
    public class SceneEventArgs : EventArgs
    {
        public SceneEventArgs(Type type) { Type = type; }
        public Type Type { get; private set; }
    } 
}