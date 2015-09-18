using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Test.Scenes
{
    public class UtilityTestScene : BIOXScene
    {
        private Timer timer;
        private AnimatedTexture animatedTexture;

        public UtilityTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Utility Test Scene";
            keyboardManager.PressingDelay = 100;
        }

        protected override void AttachSceneEventHandlers()
        {
            timer.Tick += OnTimerTick;
            timer.Stopped += OnTimerStopped;
            base.AttachSceneEventHandlers();
        }

        protected override void DetachSceneEventHandlers()
        {
            timer.Tick -= OnTimerTick;
            timer.Stopped -= OnTimerStopped;
            base.DetachSceneEventHandlers();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Console.WriteLine("TIMER: Tick");
        }

        private void OnTimerStopped(object sender, EventArgs e)
        {
            Console.WriteLine("TIMER: Stopped");
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    animatedTexture.SetRegion("up");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X, animatedTexture.Position.Y - 10);
                    break;
                case Keys.S:
                    animatedTexture.SetRegion("down");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X, animatedTexture.Position.Y + 10);
                    break;
                case Keys.A:
                    animatedTexture.SetRegion("left");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X - 10, animatedTexture.Position.Y);
                    break;
                case Keys.D:
                    animatedTexture.SetRegion("right");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X + 10, animatedTexture.Position.Y);
                    break;
                case Keys.Z:
                    timer.Start();
                    animatedTexture.AutoAnimated = true;
                    break;
                case Keys.X:
                    timer.Stop();
                    animatedTexture.AutoAnimated = false;
                    break;
                case Keys.Left:
                    sceneManager.Load<Physics3DTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<InputTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    animatedTexture.SetRegion("up");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X, animatedTexture.Position.Y - 10);
                    break;
                case Keys.S:
                    animatedTexture.SetRegion("down");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X, animatedTexture.Position.Y + 10);
                    break;
                case Keys.A:
                    animatedTexture.SetRegion("left");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X - 10, animatedTexture.Position.Y);
                    break;
                case Keys.D:
                    animatedTexture.SetRegion("right");
                    animatedTexture.IncrementFrame();
                    animatedTexture.Position = new Vector2(animatedTexture.Position.X + 10, animatedTexture.Position.Y);
                    break;
            }
            base.OnKeyPressing(sender, e);
        }

        public override void Initialize()
        {
            timer = new Timer(game);
            timer.Interval = 1000;
            AddComponent(timer);

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
            animatedTexture = new AnimatedTexture(game, SceneContent.Load<Texture2D>("UI image/texture_atlas_example"), textureRegions);
            animatedTexture.Position = new Vector2(0, 0);
            animatedTexture.AnimationSpeed = 500;
            AddComponent(animatedTexture);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}