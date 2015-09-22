using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;
using BIOXFramework.Physics2D;
using BIOXFramework.Test.Gameplay;

namespace BIOXFramework.Test.Scenes
{
    public class PhysicsTestScene : BIOXScene
    {
        private Player2D player1;
        private Player2D player2;

        public PhysicsTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Physics Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Up), player1))
                        player1.Move(PlayerDirections.Up);
                    break;
                case Keys.S:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Down), player1))
                        player1.Move(PlayerDirections.Down);
                    break;
                case Keys.A:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Left), player1))
                        player1.Move(PlayerDirections.Left);
                    break;
                case Keys.D:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Right), player1))
                        player1.Move(PlayerDirections.Right);
                    break;
                case Keys.I:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Up), player2))
                        player2.Move(PlayerDirections.Up);
                    break;
                case Keys.K:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Down), player2))
                        player2.Move(PlayerDirections.Down);
                    break;
                case Keys.J:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Left), player2))
                        player2.Move(PlayerDirections.Left);
                    break;
                case Keys.L:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Right), player2))
                        player2.Move(PlayerDirections.Right);
                    break;
                case Keys.Left:
                    sceneManager.Load<GuiTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<UtilityTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {

            switch (e.Key)
            {
                case Keys.W:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Up), player1))
                        player1.Move(PlayerDirections.Up);
                    break;
                case Keys.S:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Down), player1))
                        player1.Move(PlayerDirections.Down);
                    break;
                case Keys.A:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Left), player1))
                        player1.Move(PlayerDirections.Left);
                    break;
                case Keys.D:
                    if (!collision2DManager.IsColliding(player1.MoveEmulate(PlayerDirections.Right), player1))
                        player1.Move(PlayerDirections.Right);
                    break;
                case Keys.I:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Up), player2))
                        player2.Move(PlayerDirections.Up);
                    break;
                case Keys.K:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Down), player2))
                        player2.Move(PlayerDirections.Down);
                    break;
                case Keys.J:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Left), player2))
                        player2.Move(PlayerDirections.Left);
                    break;
                case Keys.L:
                    if (!collision2DManager.IsColliding(player2.MoveEmulate(PlayerDirections.Right), player2))
                        player2.Move(PlayerDirections.Right);
                    break;
                case Keys.Left:
                    sceneManager.Load<GuiTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<UtilityTestScene>();
                    break;
            }
            base.OnKeyPressing(sender, e);
        }

        protected override void On2DObjectCollide(object sender, Collide2DEventArgs e)
        {
            Console.WriteLine(string.Format("COLLIDED: {0} with {1}", e.Component1 == null ? "null" : e.Component1.GetType().Name, e.Component2 == null ? "null" : e.Component2.GetType().Name));
            base.On2DObjectCollide(sender, e);
        }

        protected override void On2DObjectInCollision(object sender, Collide2DEventArgs e)
        {
            Console.WriteLine(string.Format("PERSISTENT COLLISION: {0} with {1}", e.Component1 == null ? "null" : e.Component1.GetType().Name, e.Component2 == null ? "null" : e.Component2.GetType().Name));
            base.On2DObjectInCollision(sender, e);
        }

        protected override void On2DObjectOutCollision(object sender, Collide2DEventArgs e)
        {
            Console.WriteLine(string.Format("OUT OF COLLISION: {0} with {1}", e.Component1 == null ? "null" : e.Component1.GetType().Name, e.Component2 == null ? "null" : e.Component2.GetType().Name));
            base.On2DObjectOutCollision(sender, e);
        }

        public void CheckDirectionCollision(Player2D player, bool enableAll = false)
        {
            if (enableAll)
            {
                player.SetAvailableDirections(PlayerDirections.Down, PlayerDirections.Up, PlayerDirections.Left, PlayerDirections.Right);
                return;
            }

            switch (player.PlayerDirection)
            {
                case PlayerDirections.Up:
                    player.SetAvailableDirections(PlayerDirections.Down, PlayerDirections.Left, PlayerDirections.Right);
                    break;
                case PlayerDirections.Down:
                    player.SetAvailableDirections(PlayerDirections.Up, PlayerDirections.Left, PlayerDirections.Right);
                    break;
                case PlayerDirections.Left:
                    player.SetAvailableDirections(PlayerDirections.Down, PlayerDirections.Up, PlayerDirections.Right);
                    break;
                case PlayerDirections.Right:
                    player.SetAvailableDirections(PlayerDirections.Down, PlayerDirections.Up, PlayerDirections.Left);
                    break;
            }
        }

        public override void Initialize()
        {
            keyboardManager.PressingDelay = 100;

            player1 = new Player2D(game, "player1");
            player2 = new Player2D(game, "player2");

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