using System;
using BIOXFramework.Scene;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Test.Scenes
{
    public class GuiTestScene : BIOXScene
    {
        public GuiTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "GUI Test Scene";
        }

        protected override void OnKeyPressed(object sender, Input.Events.KeyboardPressedEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Left)
                sceneManager.Load<AudioTestScene>(game);

            base.OnKeyPressed(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime);
        }
    }
}