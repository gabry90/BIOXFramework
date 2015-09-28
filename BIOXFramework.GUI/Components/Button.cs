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

        public Button(Game game, string name, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, name, animatedTexture, animations, position)
        { }

        #endregion
    }
}