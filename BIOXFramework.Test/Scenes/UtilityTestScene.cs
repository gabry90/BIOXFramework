using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;

namespace BIOXFramework.Test.Scenes
{
    public class UtilityTestScene : BIOXScene
    {
        private Timer timer;

        public UtilityTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Utility Test Scene";
            timer = new Timer(game);
            timer.Interval = 1000;
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

        public override void Initialize()
        {
            base.Initialize();
            AddComponent(timer);
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
                    timer.Start();
                    break;
                case Keys.B:
                    timer.Stop();
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

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}