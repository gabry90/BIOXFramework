using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework
{
    //implement this interface for avoid disposing component when it's called from BIOXScene
    public interface IPersistentComponent { }

    //implement this interface for ignore paused status
    public interface INonPausableComponent { }

    //don't implement this interface
    public interface ICollidableComponent
    {
        bool EnableCollisionDetection { get; set; }
    }

    //implement this interface for enable collision detector for 2D object
    public interface I2DCollidableComponent : ICollidableComponent
    {
        Rectangle Rectangle { get; }
        Rectangle? InnerRectangle { get; }
        Texture2D Texture { get; }
    }

    //implement this interface for enable collision detector for 3D object
    public interface I3DCollidableComponent : ICollidableComponent
    {
        Model Model { get; }
    }

    //implement this interface for disabling all movement (including gravity)
    public interface IImmovableComponent { }

    //don't implement this interface
    public interface IGravitableComponent 
    {
        double Mass { get; }
        double OrizzontalForce { get; set; }
        double VerticalForce { get; set; }
        bool IsOrizzontalForcePersistence { get; set; }
        bool IsVerticalForcePersistence { get; set; }
        bool IgnoreGravity { get; }
    }

    //implement this interface for enable gravity on 2D component
    public interface I2DGravitableComponent : IGravitableComponent
    {
        Texture2D Texture { get; }
        Rectangle Rectangle { get; set; }
        Rectangle? InnerRectangle { get; }
        Vector2 Position { get; set; }
    }

    //implement this interface for enable gravity on 3D component
    public interface I3DGravitableComponent : IGravitableComponent
    {
        Model Model { get; }
        Vector3 Position { get; set; }
    }
}