using System;

namespace BIOXFramework.Scene
{
    public class SceneManagerException : Exception
    {
        public SceneManagerException(string message)
            : base(string.Format("[BIOXFramework.Scene.SceneManager Exception]: {0}", message))
        {

        }
    }
}