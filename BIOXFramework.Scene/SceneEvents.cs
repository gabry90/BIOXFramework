using System;

namespace BIOXFramework.Scene
{
    public class SceneLoadedEventArgs : EventArgs
    {
        public SceneLoadedEventArgs(Type type) { Type = type; }
        public Type Type { get; private set; }
    }

    public class SceneUnloadedEventArgs : EventArgs
    {
        public SceneUnloadedEventArgs(Type type) { Type = type; }
        public Type Type { get; private set; }
    }

    public class ScenePausedEventArgs : EventArgs
    {
        public ScenePausedEventArgs(Type type) { Type = type; }
        public Type Type { get; private set; }
    }

    public class SceneResumedEventArgs : EventArgs
    {
        public SceneResumedEventArgs(Type type) { Type = type; }
        public Type Type { get; private set; }
    }
}