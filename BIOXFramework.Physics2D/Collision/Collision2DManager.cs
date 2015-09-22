using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Physics2D.Collision
{
    public sealed class Collision2DManager : GameComponent
    {
        #region vars

        public event EventHandler<Collide2DEventArgs> Collide;
        public event EventHandler<Collide2DEventArgs> InCollision;
        public event EventHandler<Collide2DEventArgs> OutCollision;
        public List<GameComponent> Components;  
        public bool EnableCollisionDetection = true;

        private List<Tuple<GameComponent, GameComponent>> collidedComponents;

        #endregion

        #region constructors

        public Collision2DManager(Game game)
            : base(game)
        {
            Components = new List<GameComponent>();
            collidedComponents = new List<Tuple<GameComponent,GameComponent>>();
        }

        #endregion

        #region public methods

        public List<GameComponent> GetCollidedComponents(GameComponent gc, params GameComponent[] exclusionList)
        {
            List<GameComponent> componentsCollided = new List<GameComponent>();

            I2DCollidableComponent component1 = gc as I2DCollidableComponent;
            if (component1 == null
                || !component1.EnableCollisionDetection
                || component1.Rectangle == Rectangle.Empty
                || component1.Texture.Bounds == Rectangle.Empty)
                return componentsCollided;

            Rectangle rect1 = component1.Rectangle;
            Nullable<Rectangle> innerRect1 = component1.InnerRectangle;

            for (int x = 0; x < Components.Count; x++)
            {
                if (Components[x] == null || Components[x] == gc || exclusionList.Contains(Components[x]))
                    continue;

                I2DCollidableComponent component2 = Components[x] as I2DCollidableComponent;
                if (component2 == null
                    || !component2.EnableCollisionDetection
                    || component2.Rectangle == Rectangle.Empty
                    || component2.Texture.Bounds == Rectangle.Empty)
                {
                    continue;
                }

                Rectangle rect2 = component2.Rectangle; 
                Nullable<Rectangle> innerRect2 = component2.InnerRectangle;

                if (DetectFullCollision(ref rect1, ref rect2, ref innerRect1, ref innerRect2, component1.Texture, component2.Texture))
                    componentsCollided.Add(Components[x]);
            }

            return componentsCollided;
        }

        public bool DetectFullCollision(ref Rectangle rectangleA, 
            ref Rectangle rectangleB,
            ref Nullable<Rectangle> innerRectA,
            ref Nullable<Rectangle> innerRectB,
            Texture2D textureA, 
            Texture2D textureB)
        {
            if (!DetectRectangleCollision(ref rectangleA, ref rectangleB))
                return false;

            Color[] colorA = innerRectA.HasValue ?
                new Color[innerRectA.Value.Width * innerRectA.Value.Height] :
                new Color[textureA.Width * textureA.Height];

            Color[] colorB = innerRectB.HasValue ?
                new Color[innerRectB.Value.Width * innerRectB.Value.Height] :
                new Color[textureB.Width * textureB.Height];

            if (innerRectA.HasValue)
                textureA.GetData(0, innerRectA, colorA, 0, colorA.Length);
            else
                textureA.GetData(colorA);

            if (innerRectB.HasValue)
                textureB.GetData(0, innerRectB, colorB, 0, colorB.Length);
            else
                textureB.GetData(colorB);

            return DetectPixelCollision(ref rectangleA, ref rectangleB, ref colorA, ref colorB);
        }

        public bool DetectRectangleCollision(
            ref Rectangle rectangleA, 
            ref Rectangle rectangleB)
        {
            return rectangleA.Intersects(rectangleB);
        }

        public bool DetectPixelCollision(ref Rectangle rectangleA, ref Rectangle rectangleB, ref Color[] dataA, ref Color[] dataB)
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
            {
                if (collidedComponents.Count > 0) collidedComponents.Clear();
                return;
            }

            List<GameComponent> processedComponents = new List<GameComponent>();
             
            for (int x = 0; x < Components.Count; x++)
            {
                if (Components[x] == null || processedComponents.Contains(Components[x]))
                    continue;

                I2DCollidableComponent component1 = Components[x] as I2DCollidableComponent;
                if (component1 == null
                    || !component1.EnableCollisionDetection
                    || component1.Rectangle == Rectangle.Empty
                    || component1.Texture.Bounds == Rectangle.Empty)
                {
                    processedComponents.Add(Components[x]);
                    continue;
                }

                processedComponents.Add(Components[x]);

                for (int y = 0; y < Components.Count; y++)
                {
                    if (Components[y] == null || processedComponents.Contains(Components[y]))
                        continue;

                    I2DCollidableComponent component2 = Components[y] as I2DCollidableComponent;
                    if (component2 == null
                        || !component2.EnableCollisionDetection
                        || component2.Rectangle == Rectangle.Empty
                        || component2.Texture.Bounds == Rectangle.Empty)
                    {
                        processedComponents.Add(Components[y]);
                        continue;
                    }

                    processedComponents.Add(Components[y]);

                    Rectangle rect1 = component1.Rectangle;
                    Rectangle rect2 = component2.Rectangle;
                    Nullable<Rectangle> innerRect1 = component1.InnerRectangle;
                    Nullable<Rectangle> innerRect2 = component2.InnerRectangle;

                    bool collided = DetectFullCollision(ref rect1, ref rect2, ref innerRect1, ref innerRect2, component1.Texture, component2.Texture);
                    var inCollision = collidedComponents.FirstOrDefault(z => z.Item1 == Components[x] && z.Item2 == Components[y]);

                    if (collided && inCollision != null)        //collision persistent
                        InCollisionEventDispatcher(new Collide2DEventArgs(Components[x], Components[y]));
                    else if (collided && inCollision == null)   //collision for first time
                    {
                        collidedComponents.Add(new Tuple<GameComponent, GameComponent>(Components[x], Components[y]));
                        CollideEventDispatcher(new Collide2DEventArgs(Components[x], Components[y]));
                    }
                    else if (!collided && inCollision != null)  //out of collision (only after collide)
                    {
                        collidedComponents.Remove(inCollision);
                        OutCollisionEventDispatcher(new Collide2DEventArgs(Components[x], Components[y]));
                    }
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

        private void InCollisionEventDispatcher(Collide2DEventArgs e)
        {
            var h = InCollision;
            if (h != null)
                h(this, e);
        }

        private void OutCollisionEventDispatcher(Collide2DEventArgs e)
        {
            var h = OutCollision;
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
                    if (InCollision != null) InCollision = null;
                    if (OutCollision != null) OutCollision = null;
                    lock (Components) { Components.Clear(); }
                    lock (collidedComponents) { collidedComponents.Clear(); }
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