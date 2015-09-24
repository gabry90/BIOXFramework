using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Physics.Gravity
{
    public enum GravityDirection
    {
        _0,
        _45,
        _90,
        _180,
        _270,
        _360
    }

    public sealed class GravityManager : GameComponent
    {
        #region vars

        public bool EnableGravity { get; set; }
        public double GravityAcceleration
        {
            get { return gravityAcceleration; }
            set { gravityAcceleration = value < 0 ? 0 : gravityAcceleration; }
        }

        private double gravityAcceleration;
        private List<GameComponent> gravitableComponents;

        #endregion

        #region constructors

        public GravityManager(Game game)
            : base(game)
        {
            gravitableComponents = new List<GameComponent>();
            EnableGravity = false; //disabled for default
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {

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