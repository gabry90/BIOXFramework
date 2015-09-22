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
        private Label label;

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
                case Keys.C:
                    Console.WriteLine(this);
                    break;
                case Keys.Left:
                    sceneManager.Load<AudioTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<PhysicsTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void LoadContent()
        {
            label = new Label(game, SceneContent.Load<SpriteFont>("Fonts/curier_new"), "prova", Vector2.Zero);
            AddDrawableGameComponent(label);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime);
        }
    }
}