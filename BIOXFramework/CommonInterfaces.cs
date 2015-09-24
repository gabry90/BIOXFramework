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
        Rectangle? InnerRectangle { get; }
        Texture2D Texture { get; }
        bool EnableCollisionDetection { get; set; }
    }

    //implement this interface for disabling all movement (including gravity)
    public interface IImmovableComponent { }

    //implement this interface for enable gravity on component
    public interface IGravitableComponent 
    {
        double Mass { get; }
        Texture2D Texture { get; }
        Rectangle Rectangle { get; set; }
        Rectangle? InnerRectangle { get; }
        double OrizzontalForce { get; set; }
        double VerticalForce { get; set; }
        bool IsOrizzontalForcePersistence { get; set; }
        bool IsVerticalForcePersistence { get; set; }
        bool IgnoreGravity { get; }
    }

    //implement this interface for enable collision detector for 3D object
    public interface I3DCollidableComponent 
    {
        Model Model { get; }
        bool EnableCollisionDetection { get; set; }
    }
}