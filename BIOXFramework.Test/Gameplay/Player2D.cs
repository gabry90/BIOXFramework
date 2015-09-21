using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.Test.Gameplay
{
    public enum PlayerMovements
    {
        Up,
        Down,
        Left,
        right
    }

    public class Player2D : DrawableGameComponent, I2DCollidableComponent
    {
        public string Name { get; set; }
        public AnimatedTexture AnimatedTexture;
        public Texture2D Texture { get { return AnimatedTexture.Texture; } }
        public Rectangle Rectangle { get { return AnimatedTexture.Rectangle; } }
        public Rectangle InnerRectangle { get { return AnimatedTexture.InnerRectangle; } }
        public bool EnableCollisionDetection { get; set; }

        public bool EnableMovement { get; set; }
        public bool EnableAnimation { get; set; }

        public Player2D(Game game, string name)
            : base(game)
        {
            Name = name;
            EnableCollisionDetection = true;
            EnableMovement = true;
            EnableAnimation = true;
        }

        public void Move(PlayerMovements movement)
        {
            switch (movement)
            {
                case PlayerMovements.Up:
                {
                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("up");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y - 10);
                    break;
                }
                case PlayerMovements.Down:
                {
                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("down");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y + 10);
                    break;
                }
                case PlayerMovements.Left:
                {
                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("left");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X - 10, AnimatedTexture.Position.Y);
                    break;
                }
                case PlayerMovements.right:
                {
                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("right");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X + 10, AnimatedTexture.Position.Y);
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            AnimatedTexture.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            AnimatedTexture.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}