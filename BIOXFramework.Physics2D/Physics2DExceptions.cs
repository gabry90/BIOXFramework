using System;

namespace BIOXFramework.Physics2D
{
    public class CollisionException: Exception
    {
        public CollisionException(string message)
            : base(string.Format("[BIOXFramework.Physics2D.Collision Exception]: {0}", message))
        {

        }
    }
}