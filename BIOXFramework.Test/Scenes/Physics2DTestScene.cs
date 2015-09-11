using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;

namespace BIOXFramework.Test.Scenes
{
    public class Physics2DTestScene : BIOXScene
    {
        public Physics2DTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Physics 2D Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    game.Exit();
                    break;
                case Keys.Left:
                    sceneManager.Load<GuiTestScene>(game);
                    break;
                case Keys.Right:
                    sceneManager.Load<Physics3DTestScene>(game);
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Purple);
            base.Draw(gameTime);
        }
    }
}