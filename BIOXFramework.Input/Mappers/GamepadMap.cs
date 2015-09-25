using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Input.Mappers
{
    public sealed class GamepadMap
    {
        public string Name { get; set; }
        public Buttons? Button { get; set; }
        internal DateTime PressedTime { get; set; }
        public PlayerIndex Player { get; set; }
    }
}
