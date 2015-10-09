using System;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    public sealed class KeyboardMap
    {
        public string Name { get; set; }
        public Keys? Key { get; set; }
        internal DateTime PressedTime { get; set; }
    }
}