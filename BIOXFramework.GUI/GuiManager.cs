﻿using System;
using BIOXFramework.GUI.Components;
using Microsoft.Xna.Framework;

namespace BIOXFramework.GUI
{
    public sealed class GuiManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        #region vars

        public Cursor CurrentCursor 
        {
            get { return _cursor; }
            set
            {
                if (value == null)
                    throw new GuiException("cursor cannot be null!");
                _cursor = value;
            }
        }

        private Cursor _cursor;

        #endregion

        public GuiManager(Game game)
            : base(game)
        {

        }
    }
}