using System;
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
        private TextureAtlas textureAtlas;

        public UtilityTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Utility Test Scene";
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
                case Keys.A:
                    textureAtlas.CurrentFrame--;
                    break;
                case Keys.D:
                    textureAtlas.CurrentFrame++;
                    break;
                case Keys.Z:
                    timer.Start();
                    textureAtlas.AutoAnimated = true;
                    break;
                case Keys.X:
                    timer.Stop();
                    textureAtlas.AutoAnimated = false;
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

        public override void Initialize()
        {
            timer = new Timer(game);
            timer.Interval = 1000;
            AddComponent(timer);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            textureAtlas = new TextureAtlas(game, SceneContent.Load<Texture2D>("UI image/texture_atlas_example"), 4, 4);
            textureAtlas.Position = new Vector2(0, 0);
            textureAtlas.AnimationSpeed = 100;
            AddComponent(textureAtlas);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}