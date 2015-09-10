using System;

namespace BIOXFramework.Scene
{
    public class SceneVisibilityChangedEventArgs : EventArgs
    {
        public SceneVisibilityChangedEventArgs(Type type, bool visible)
        {
            Type = type; 
            Visible = visible;
        }
        public Type Type { get; private set; }
        public bool Visible { get; private set; }
    }

    public class SceneEnabledChangedEventArgs : EventArgs
    {
        public SceneEnabledChangedEventArgs(Type type, bool enabled)
        {
            Type = type; 
            Enabled = enabled;
        }
        public Type Type { get; private set; }
        public bool Enabled { get; private set; }
    }

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