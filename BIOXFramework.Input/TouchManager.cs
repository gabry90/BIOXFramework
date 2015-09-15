using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BIOXFramework.Input
{
    public sealed class TouchManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        public TouchManager(Game game)
            : base(game)
        {

        }

        #region interface implementations

        public bool ForcePausableStatus { get; set; }
        public bool ForceDisposing { get; set; }

        #endregion
    }
}