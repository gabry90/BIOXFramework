using System;

namespace BIOXFramework.Physics
{
    public class CollisionException: Exception
    {
        public CollisionException(string message)
            : base(string.Format("[BIOXFramework.Physics.Collision Exception]: {0}", message))
        {

        }
    }

    public class GravityException: Exception
    {
        public GravityException(string message)
            : base(string.Format("[BIOXFramework.Physics.Gravity Exception]: {0}", message))
        {

        }
    }
}