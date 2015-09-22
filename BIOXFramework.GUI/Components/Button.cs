using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class Button : GuiBase
    {
        #region constructors

        public Button(Game game)
            : base(game)
        { }

        public Button(Game game, Texture2D texture)
            : base(game, texture)
        { }

        public Button(Game game, Texture2D texture, List<AnimatedTextureRegion> regions)
            : base(game, texture, regions)
        { }

        #endregion
    }
}