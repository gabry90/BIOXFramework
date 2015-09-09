using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using BIOXFramework.Services;

namespace BIOXFramework.Input
{
    public sealed class TouchManager : GameComponent, IBIOXFrameworkService
    {
        public TouchManager(Game game)
            : base(game)
        {

        }
    }
}