using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.Test.Gameplay
{
    public enum PlayerDirections
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Player2D : DrawableGameComponent, I2DCollidableComponent
    {
        public string Name { get; set; }
        public AnimatedTexture AnimatedTexture;
        public Texture2D Texture { get { return AnimatedTexture.Texture; } }
        public Rectangle Rectangle { get { return AnimatedTexture.Rectangle; } }
        public Nullable<Rectangle> InnerRectangle { get { return AnimatedTexture.InnerRectangle; } }
        public bool EnableCollisionDetection { get; set; }
        public PlayerDirections PlayerDirection { get { return direction; } }
        public bool EnableMovement { get; set; }
        public bool EnableAnimation { get; set; }

        private PlayerDirections direction;
        private List<PlayerDirections> directionsAvailable;

        public Player2D(Game game, string name)
            : base(game)
        {
            Name = name;
            EnableCollisionDetection = true;
            EnableMovement = true;
            EnableAnimation = true;
            directionsAvailable = new List<PlayerDirections>
            {
                PlayerDirections.Up,
                PlayerDirections.Down,
                PlayerDirections.Left,
                PlayerDirections.Right
            };
        }

        private Player2D(Player2D player)
           : base(player.Game)
        {
            Name = player.Name;
            EnableCollisionDetection = player.EnableCollisionDetection;
            EnableMovement = player.EnableMovement;
            EnableAnimation = player.EnableAnimation;
            directionsAvailable = player.directionsAvailable;
            AnimatedTexture = new AnimatedTexture(player.AnimatedTexture);
        }

        public void SetAvailableDirections(params PlayerDirections[] directions)
        {
            directionsAvailable.Clear();
            directionsAvailable.AddRange(directions);
        }

        public void SetAvailableDirection(PlayerDirections direction)
        {
            if (!directionsAvailable.Contains(direction))
                directionsAvailable.Add(direction);
        }

        public Player2D MoveEmulate(PlayerDirections direction)
        {
            Player2D t = new Player2D(this);
            t.Move(direction);
            t.AnimatedTexture.Update(null);
            return t;
        }

        public void Move(PlayerDirections direction)
        {
            this.direction = direction;

            switch (direction)
            {
                case PlayerDirections.Up:
                {
                    if (!directionsAvailable.Contains(PlayerDirections.Up))
                        break;

                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("up");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y - 10);
                    break;
                }
                case PlayerDirections.Down:
                {
                    if (!directionsAvailable.Contains(PlayerDirections.Down))
                        break;

                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("down");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y + 10);
                    break;
                }
                case PlayerDirections.Left:
                {
                    if (!directionsAvailable.Contains(PlayerDirections.Left))
                        break;

                    if (EnableAnimation)
                    {
                        AnimatedTexture.SetRegion("left");
                        AnimatedTexture.IncrementFrame();
                    }
                    if (EnableMovement) AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X - 10, AnimatedTexture.Position.Y);
                    break;
                }
                case PlayerDirections.Right:
                {
                    if (!directionsAvailable.Contains(PlayerDirections.Right))
                        break;

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