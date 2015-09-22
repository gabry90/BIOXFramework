using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class Button : AnimatedGuiBase
    {
        #region constructors

        public Button(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, animatedTexture, animations, position)
        { }

        #endregion
    }
}