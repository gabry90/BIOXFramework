using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    public sealed class KeyboardMap
    {
        public string Name { get; set; }
        public Keys? Key { get; set; }
        public DateTime PressedTime { get; set; }
    }
}