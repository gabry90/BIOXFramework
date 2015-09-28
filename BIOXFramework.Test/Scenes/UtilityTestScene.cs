using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Physics.Collision;
using BIOXFramework.GUI.Components;

namespace BIOXFramework.Test.Scenes
{
    public class UtilityTestScene : BIOXScene
    {
        private Timer timer;
        private AnimatedTexture explosion;

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
                case Keys.Z:
                    timer.Start();
                    explosion.AutoAnimated = true;
                    break;
                case Keys.X:
                    timer.Stop();
                    explosion.AutoAnimated = false;
                    break;
                case Keys.Left:
                    sceneManager.Load<PhysicsTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<InputTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        public override void Initialize()
        {
            timer = new Timer(game);
            timer.Interval = 1000;
            AddGameComponent(timer);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            List<AnimatedTextureRegion> textureRegions = new List<AnimatedTextureRegion>
            {
                new AnimatedTextureRegion("exp1", 1, 8, 67, 67),
                new AnimatedTextureRegion("exp2", 2, 8, 67, 67),
                new AnimatedTextureRegion("exp3", 3, 8, 67, 67),
                new AnimatedTextureRegion("exp4", 4, 8, 67, 67)
            };
            explosion = new AnimatedTexture(game, SceneContent.Load<Texture2D>("UI image/explosion"), textureRegions);
            explosion.Position = new Vector2(200, 200);
            explosion.AnimationSpeed = 0;
            explosion.AnimateAllRegions = true;
            AddDrawableGameComponent(explosion);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}