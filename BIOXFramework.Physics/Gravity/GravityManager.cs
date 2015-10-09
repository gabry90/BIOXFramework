using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Physics.Collision;

namespace BIOXFramework.Physics.Gravity
{
    public enum GravityDirections
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

        public EventHandler<GravityEventArgs> Falling;

        public bool EnableGravity { get; set; }
        public GravityDirections GravityDirectionAngle { get; set; }
        public double GravityAcceleration
        {
            get { return gravityAcceleration; }
            set { gravityAcceleration = value < 0 ? 0 : gravityAcceleration; }
        }

        private double gravityAcceleration;
        private List<GameComponent> components;
        private Collision2DManager collision2DManager;

        #endregion

        #region constructors

        public GravityManager(Game game)
            : base(game)
        {
            collision2DManager = game.Services.GetService<Collision2DManager>();
            if (collision2DManager == null)
                throw new GravityException("Gravity manager required collision 2d manager game service!");

            collision2DManager.Collide += On2DCollided;
            collision2DManager.InCollision += On2DCollisionPersist;
            collision2DManager.OutCollision += On2DOutOfCollision;

            components = new List<GameComponent>();
            GravityDirectionAngle = GravityDirections._0;
            EnableGravity = false; //disabled for default
        }

        #endregion

        #region public methods

        public void AddComponent(GameComponent component)
        {
            if (component == null)
                return;

            lock (components)
            {
                int componentInListIndex = components.IndexOf(component);
                if (componentInListIndex != -1)
                    components[componentInListIndex] = component;
                else
                    components.Add(component);
            }
        }

        public void RemoveComponent(GameComponent component)
        {
            if (component == null)
                return;

            lock (components)
            {
                if (components.Contains(component))
                    components.Remove(component);
            }
        }

        public void ClearComponents()
        {
            lock (components)
            {
                components.Clear();
            }
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            if (!EnableGravity)
                return;

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is IImmovableComponent)
                    continue; //ignoring immovable components

                IGravitableComponent component = components[i] as IGravitableComponent;
                if (component == null || component.IgnoreGravity)
                    continue;

                if (components[i] is I2DGravitableComponent)
                {
                    //2d gravitable component
                    I2DGravitableComponent component2D = (I2DGravitableComponent)components[i];
                    if (component2D.Texture == null)
                        continue;

                    Vector2 newPosition = GravityHelper.Recalculate2DPosition(component2D, GravityDirectionAngle, gravityAcceleration);      
                    
                    if (components[i] is I2DCollidableComponent)
                    {
                        List<GameComponent> collidedComponents = collision2DManager.GetCollidedComponents((I2DCollidableComponent)components[i]);
                        foreach (GameComponent collidedCom in collidedComponents)
                        {
                            //to-do: recalculate new position if collided with other components
                        }
                    }

                    component2D.Position = newPosition;

                    //to-do: dispatch on falling event here if obiect is falling on gravity direction
                }
                else
                {
                    //3d gravitable component
                    I3DGravitableComponent component3D = (I3DGravitableComponent)components[i];
                    if (component3D.Model == null)
                        continue;

                    Vector3 newPosition = GravityHelper.Recalculate3DPosition(component3D, GravityDirectionAngle, gravityAcceleration);
                    //to do: use collision3DManager
                    component3D.Position = newPosition;
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region collision events

        private void On2DCollided(object sender, Collide2DEventArgs e)
        {

        }

        private void On2DCollisionPersist(object sender, Collide2DEventArgs e)
        {

        }

        private void On2DOutOfCollision(object sender, Collide2DEventArgs e)
        {

        }

        #endregion

        #region dispatchers

        private void FallingEventHandler(GravityEventArgs e)
        {
            var h = Falling;
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
                    collision2DManager.Collide -= On2DCollided;
                    collision2DManager.InCollision -= On2DCollisionPersist;
                    collision2DManager.OutCollision -= On2DOutOfCollision;

                    if (Falling != null) Falling = null;
                    lock (components) { components.Clear(); }
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