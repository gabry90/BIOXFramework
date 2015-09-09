using System;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    public sealed class GamepadMap
    {
        public string Name { get; set; }
        public Buttons? Button { get; set; }
        public DateTime PressedTime { get; set; }
    }
}
