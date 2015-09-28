using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class Picture : AnimatedGuiBase
    {
        #region constructors

        public Picture(Game game, string name, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, name, animatedTexture, animations, position)
        { }

        #endregion
    }
}