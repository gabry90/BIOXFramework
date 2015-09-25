using System;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Physics.Gravity
{
    internal static class GravityHelper
    {
        public static Vector2 Recalculate2DPosition(I2DGravitableComponent component, GravityDirections direction, double acceleration)
        {
            //to do: implement 2d logic here...
            return Vector2.Zero;
        }

        public static Vector3 Recalculate3DPosition(I3DGravitableComponent component, GravityDirections direction, double acceleration)
        {
            //to do: implement 3d logic here...
            return Vector3.Zero;
        }
    }
}