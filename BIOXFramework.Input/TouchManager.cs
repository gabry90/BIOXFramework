using Microsoft.Xna.Framework;

namespace BIOXFramework.Input
{
    public sealed class TouchManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        public TouchManager(Game game)
            : base(game)
        {

        }
    }
}