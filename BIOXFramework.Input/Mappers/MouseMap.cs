using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    public enum MouseButtons
    {
        Left,
        Rigth,
        Middle,
        X1,
        X2
    }

    public sealed class MouseMap
    {
        public string Name { get; set; }
        public MouseButtons? Button { get; set; }
        public DateTime PressedTime { get; set; }
    }
}