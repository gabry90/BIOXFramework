using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class Picture : AnimatedGuiBase
    {
        #region constructors

        public Picture(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, animatedTexture, animations, position)
        { }

        #endregion
    }
}