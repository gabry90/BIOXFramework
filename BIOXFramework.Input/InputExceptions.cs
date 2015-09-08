using System;

namespace BIOXFramework.Input
{
    public class KeyboardManagerException : Exception
    {
        public KeyboardManagerException(string message)
            : base(string.Format("[BIOXFramework.Input.KeyboardManager Exception]: {0}", message))
        {

        }
    }

    public class GamepadManagerException : Exception
    {
        public GamepadManagerException(string message)
            : base(string.Format("[BIOXFramework.Input.GamepadManager Exception]: {0}", message))
        {

        }
    }

    public class MouseManagerException : Exception
    {
        public MouseManagerException(string message)
            : base(string.Format("[BIOXFramework.Input.MouseManager Exception]: {0}", message))
        {

        }
    }

    public class TouchManagerException : Exception
    {
        public TouchManagerException(string message)
            : base(string.Format("[BIOXFramework.Input.TouchManager Exception]: {0}", message))
        {

        }
    }
}