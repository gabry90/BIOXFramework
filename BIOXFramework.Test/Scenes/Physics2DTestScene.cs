using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;
using BIOXFramework.Physics2D;

namespace BIOXFramework.Test.Scenes
{
    public class Physics2DTestScene : BIOXScene
    {
        public enum PlayerMovements
        {
            Up,
            Down,
            Left,
            right
        }

        private class Player : DrawableGameComponent, I2DCollidableComponent
        {
            public Player(Game game, string name)
                : base(game)
            {
                Name = name;
            }

            public string Name { get; set; }
            public Texture2D Texture { get { return AnimatedTexture.Texture; } }
            public Rectangle Rectangle { get { return AnimatedTexture.Rectangle; } }
            public bool EnableCollisionDetection { get; set; }

            public AnimatedTexture AnimatedTexture;

            public void Move(PlayerMovements movement)
            {
                switch (movement)
                {
                    case PlayerMovements.Up:
                        AnimatedTexture.SetRegion("up");
                        AnimatedTexture.IncrementFrame();
                        AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y - 10);
                        break;
                    case PlayerMovements.Down:
                        AnimatedTexture.SetRegion("down");
                        AnimatedTexture.IncrementFrame();
                        AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X, AnimatedTexture.Position.Y + 10);
                        break;
                    case PlayerMovements.Left:
                        AnimatedTexture.SetRegion("left");
                        AnimatedTexture.IncrementFrame();
                        AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X - 10, AnimatedTexture.Position.Y);
                        break;
                    case PlayerMovements.right:
                        AnimatedTexture.SetRegion("right");
                        AnimatedTexture.IncrementFrame();
                        AnimatedTexture.Position = new Vector2(AnimatedTexture.Position.X + 10, AnimatedTexture.Position.Y);
                        break;
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

        private Player player1;
        private Player player2;

        public Physics2DTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Physics 2D Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    player1.Move(PlayerMovements.Up);
                    break;
                case Keys.S:
                    player1.Move(PlayerMovements.Down);
                    break;
                case Keys.A:
                    player1.Move(PlayerMovements.Left);
                    break;
                case Keys.D:
                    player1.Move(PlayerMovements.right);
                    break;
                case Keys.I:
                    player2.Move(PlayerMovements.Up);
                    break;
                case Keys.K:
                    player2.Move(PlayerMovements.Down);
                    break;
                case Keys.J:
                    player2.Move(PlayerMovements.Left);
                    break;
                case Keys.L:
                    player2.Move(PlayerMovements.right);
                    break;
                case Keys.Left:
                    sceneManager.Load<GuiTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<Physics3DTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    player1.Move(PlayerMovements.Up);
                    break;
                case Keys.S:
                    player1.Move(PlayerMovements.Down);
                    break;
                case Keys.A:
                    player1.Move(PlayerMovements.Left);
                    break;
                case Keys.D:
                    player1.Move(PlayerMovements.right);
                    break;
                case Keys.I:
                    player2.Move(PlayerMovements.Up);
                    break;
                case Keys.K:
                    player2.Move(PlayerMovements.Down);
                    break;
                case Keys.J:
                    player2.Move(PlayerMovements.Left);
                    break;
                case Keys.L:
                    player2.Move(PlayerMovements.right);
                    break;
            }
            base.OnKeyPressing(sender, e);
        }

        protected override void On2DObjectCollide(object sender, Collide2DEventArgs e)
        {

            base.On2DObjectCollide(sender, e);
        }

        public override void Initialize()
        {
            keyboardManager.PressingDelay = 100;

            player1 = new Player(game, "player1");
            player2 = new Player(game, "player2");
            player1.EnableCollisionDetection = true;
            player2.EnableCollisionDetection = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            List<AnimatedTextureRegion> textureRegions = new List<AnimatedTextureRegion>
            {
                new AnimatedTextureRegion("down", 1, 4, 70, 124),
                new AnimatedTextureRegion("left", 2, 4, 70, 124),
                new AnimatedTextureRegion("right", 3, 4, 70, 124),
                new AnimatedTextureRegion("up", 4, 4, 70, 124),
            };
            AnimatedTexture texture1 = new AnimatedTexture(game, SceneContent.Load<Texture2D>("UI image/texture_atlas_example"), textureRegions);
            AnimatedTexture texture2 = new AnimatedTexture(game, SceneContent.Load<Texture2D>("UI image/texture_atlas_example"), textureRegions);
            texture1.Position = new Vector2(0, 0);
            texture1.AnimationSpeed = 500;
            texture2.Position = new Vector2(100, 100);
            texture2.AnimationSpeed = 500;

            player1.AnimatedTexture = texture1;
            player2.AnimatedTexture = texture2;

            AddDrawableGameComponent(player1);
            AddDrawableGameComponent(player2);

            collision2DManager.EnableCollisionDetection = true;

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Purple);
            base.Draw(gameTime);
        }
    }
}