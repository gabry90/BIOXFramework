using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.GUI.Components;

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
            switch (e.Key)
            {
                case Keys.Left:
                    sceneManager.Load<UtilityTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<AudioTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            base.OnKeyPressing(sender, e);
        }

        protected override void OnKeyReleased(object sender, KeyboardReleasedEventArgs e)
        {
            base.OnKeyReleased(sender, e);
        }

        protected override void OnMousePressed(object sender, MousePressedEventArgs e)
        {
            base.OnMousePressed(sender, e);
        }

        protected override void OnMousePressing(object sender, MousePressingEventArgs e)
        {
            base.OnMousePressing(sender, e);
        }

        protected override void OnMouseReleased(object sender, MouseReleasedEventArgs e)
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

        protected override void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
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