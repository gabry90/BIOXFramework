namespace BIOXFramework
{
    //implement this interface for avoid disposing component when it's called
    public interface IPersistentComponent { }

    //implement this interface (only for derived class by GameComponent) for ignore paused status
    public interface INonPausableComponent { }
}