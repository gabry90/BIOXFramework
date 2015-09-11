using System;
using BIOXFramework.Scene;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Test.Scenes
{
    public class InputTestScene : BIOXScene
    {
        public InputTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Input Test Scene";
        }

        protected override void OnKeyPressed(object sender, Input.Events.KeyboardPressedEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Right)
                SceneManager.Load<AudioTestScene>(game);

            base.OnKeyPressed(sender, e);
        }

        protected override void OnKeyPressing(object sender, Input.Events.KeyboardPressingEventArgs e)
        {
            base.OnKeyPressing(sender, e);
        }

        protected override void OnKeyReleased(object sender, Input.Events.KeyboardReleasedEventArgs e)
        {
            base.OnKeyReleased(sender, e);
        }

        protected override void OnMousePressed(object sender, Input.Events.MousePressedEventArgs e)
        {
            base.OnMousePressed(sender, e);
        }

        protected override void OnMousePressing(object sender, Input.Events.MousePressingEventArgs e)
        {
            base.OnMousePressing(sender, e);
        }

        protected override void OnMouseReleased(object sender, Input.Events.MouseReleasedEventArgs e)
        {
            base.OnMouseReleased(sender, e);
        }

        protected override void OnMouseWhellUp(object sender, EventArgs e)
        {
            base.OnMouseWhellUp(sender, e);
        }

        protected override void OnMouseWhellDown(object sender, EventArgs e)
        {
            base.OnMouseWhellDown(sender, e);
        }

        protected override void OnMousePositionChanged(object sender, Input.Events.MousePositionChangedEventArgs e)
        {
            base.OnMousePositionChanged(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}