using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class TextBox : AnimatedGuiBase
    {
        #region constructors

        public TextBox(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, animatedTexture, animations, position)
        { }

        #endregion
    }
}