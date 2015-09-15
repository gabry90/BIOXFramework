using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.GUI;
using BIOXFramework.GUI.Components;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Test.Scenes
{
    public class GuiTestScene : BIOXScene
    {
        public GuiTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "GUI Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.A:
                    IsCursorVisible = false;
                    break;
                case Keys.B:
                    IsCursorVisible = true;
                    break;
                case Keys.Escape:
                    game.Exit();
                    break;
                case Keys.Left:
                    sceneManager.Load<AudioTestScene>(game);
                    break;
                case Keys.Right:
                    sceneManager.Load<Physics2DTestScene>(game);
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime);
        }
    }
}