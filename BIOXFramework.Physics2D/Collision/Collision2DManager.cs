using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Physics2D.Collision
{
    public sealed class Collision2DManager : GameComponent
    {
        #region vars

        public event EventHandler<Collide2DEventArgs> Collide;
        public List<GameComponent> Components;  
        public bool EnableCollisionDetection = true;

        #endregion

        #region constructors

        public Collision2DManager(Game game)
            : base(game)
        {
            Components = new List<GameComponent>();
        }

        #endregion

        #region public methods

        public bool Detect(Rectangle rectangleA, Rectangle rectangleB, Texture2D textureA, Texture2D textureB)
        {
            if (rectangleA.Intersects(rectangleB))
            {
                Color[] colorA = new Color[textureA.Width * textureA.Height];
                Color[] colorB = new Color[textureB.Width * textureB.Height];
                textureA.GetData(colorA);
                textureB.GetData(colorB);
                return xPixel(rectangleA, rectangleB, ref colorA, ref colorB);
            }
            else
                return false;
        }

        #endregion

        #region private methods

        private bool xPixel(Rectangle rectangleA, Rectangle rectangleB, ref Color[] dataA, ref Color[] dataB)
        {
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
                    if (colorA.A != 0 && colorB.A != 0)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            if (!EnableCollisionDetection)
                return;

            List<GameComponent> processedComponents = new List<GameComponent>();

            for (int x = 0; x < Components.Count; x++)
            {
                if (processedComponents.Contains(Components[x]))
                    continue;

                I2DCollidableComponent component1 = Components[x] as I2DCollidableComponent;
                if (component1 == null
                    || !component1.EnableCollisionDetection
                    || component1.Rectangle == null
                    || component1.Rectangle == Rectangle.Empty
                    || component1.Texture == null
                    || component1.Texture.Bounds == Rectangle.Empty)
                {
                    processedComponents.Add(Components[x]);
                    continue;
                }

                processedComponents.Add(Components[x]);

                for (int y = 0; y < Components.Count; y++)
                {
                    if (processedComponents.Contains(Components[y]))
                        continue;

                    I2DCollidableComponent component2 = Components[y] as I2DCollidableComponent;
                    if (component2 == null
                        || !component2.EnableCollisionDetection
                        || component2.Rectangle == null
                        || component2.Rectangle == Rectangle.Empty
                        || component2.Texture == null
                        || component2.Texture.Bounds == Rectangle.Empty)
                    {
                        processedComponents.Add(Components[y]);
                        continue;
                    }

                    processedComponents.Add(Components[y]);

                    if (Detect(component1.Rectangle, component2.Rectangle, component1.Texture, component2.Texture))
                        CollideEventDispatcher(new Collide2DEventArgs(Components[x], Components[y]));
                }
            }

            processedComponents.Clear();

            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void CollideEventDispatcher(Collide2DEventArgs e)
        {
            var h = Collide;
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
                    if (Collide != null) Collide = null;
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