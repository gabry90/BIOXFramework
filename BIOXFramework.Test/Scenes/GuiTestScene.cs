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
            game.IsMouseVisible = false;
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
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

        protected override void LoadContent()
        {
            AddComponent(new Cursor(game, game.Content.Load<Texture2D>("UI image/cursor")));
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime);
        }
    }
}