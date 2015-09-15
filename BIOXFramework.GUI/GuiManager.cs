using System;
using BIOXFramework.GUI.Components;
using Microsoft.Xna.Framework;

namespace BIOXFramework.GUI
{
    public sealed class GuiManager: IPersistenceComponent
    {
        #region vars

        public Cursor CurrentCursor 
        {
            get 
            { 
                if (_cursor == null)
                    throw new GuiException("cursor cannot be null!");
                return _cursor; 
            }
            set
            {
                if (value == null)
                    throw new GuiException("cursor cannot be null!");
                _cursor = value;
            }
        }

        private Cursor _cursor;

        #endregion
    }
}