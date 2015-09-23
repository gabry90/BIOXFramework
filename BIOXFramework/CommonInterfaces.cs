using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework
{
    //implement this interface for avoid disposing component when it's called from BIOXScene
    public interface IPersistentComponent { }

    //implement this interface for ignore paused status
    public interface INonPausableComponent { }

    //implement this interface for enable collision detector for 2D object
    public interface I2DCollidableComponent 
    {
        Rectangle Rectangle { get; }
        Nullable<Rectangle> InnerRectangle { get; }
        Texture2D Texture { get; }
        bool EnableCollisionDetection { get; set; }
    }

    //implement this interface for disabling all movement (including gravity)
    public interface IImmovableComponent { }

    //implement this interface for enable gravity on component
    public interface IGravitableComponent 
    {
        float Mass { get; }
        bool IgnoreGravity { get; }
    }


    //implement this interface for enable collision detector for 3D object
    public interface I3DCollidableComponent 
    {
        Model Model { get; }
        bool EnableCollisionDetection { get; set; }
    }
}