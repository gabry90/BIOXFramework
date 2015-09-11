using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;

namespace BIOXFramework.Test.Scenes
{
    public class Physics3DTestScene : BIOXScene
    {
        public Physics3DTestScene(GameTest game)
            :base(game)
        {
            game.Window.Title = "Physics 3D Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    game.Exit();
                    break;
                case Keys.Left:
                    sceneManager.Load<Physics2DTestScene>(game);
                    break;
                case Keys.Right:
                    sceneManager.Load<InputTestScene>(game);
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Yellow);
            base.Draw(gameTime);
        }
    }
}