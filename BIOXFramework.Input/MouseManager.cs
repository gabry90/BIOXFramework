using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input
{
    public sealed class MouseManager : GameComponent
    {
        #region vars

        public EventHandler<MousePressedEventArgs> Pressed;
        public EventHandler<MousePressingEventArgs> Pressing;
        public EventHandler<MouseReleasedEventArgs> Released;

        private List<MouseMap> _maps;

        #endregion

        #region constructors

        public MouseManager(Game game)
            : base(game)
        {
            _maps = new List<MouseMap>();
        }

        #endregion

        #region public methods

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void MousePressedEventDispatcher(MousePressedEventArgs e)
        {
            var h = Pressed;
            if (h != null)
                h(this, e);
        }

        private void MousePressingEventDispatcher(MousePressingEventArgs e)
        {
            var h = Pressing;
            if (h != null)
                h(this, e);
        }

        private void MouseReleasedEventDispatcher(MouseReleasedEventArgs e)
        {
            var h = Released;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _maps.Clear();
                    if (Pressed != null) Pressed = null;
                    if (Pressing != null) Pressing = null;
                    if (Released != null) Released = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}