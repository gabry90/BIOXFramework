using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.GUI;
using BIOXFramework.GUI.Components;
using BIOXFramework.Utility;

namespace BIOXFramework.Test.Scenes
{
    public class GuiTestScene : BIOXScene
    {
        private Label label;
        private TextBox textbox;

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
                    if (!textbox.IsFocused)
                        sceneManager.Load<AudioTestScene>();
                    break;
                case Keys.Right:
                    if (!textbox.IsFocused)
                        sceneManager.Load<PhysicsTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void LoadContent()
        {
            List<AnimatedTextureRegion> regions = new List<AnimatedTextureRegion>
            {
                new AnimatedTextureRegion("non_focused", 1, 1, 311, 55),
                new AnimatedTextureRegion("focused", 2, 1, 311, 55)
            };
            List<AnimatedGuiAnimations> animations = new List<AnimatedGuiAnimations>()
            {
                new AnimatedGuiAnimations(AnimatedGuiEvents.OnFocused, "focused"),
                new AnimatedGuiAnimations(AnimatedGuiEvents.OnLostFocus, "non_focused")
            };
            AnimatedTexture textBoxTexture = new AnimatedTexture(game, SceneContent.Load<Texture2D>("UI image/textbox_example"), regions);

            label = new Label(game, "label1", SceneContent.Load<SpriteFont>("Fonts/curier_new"), "prova", Vector2.Zero);
            textbox = new TextBox(game, "textbox1", textBoxTexture, animations, new Vector2(200, 200), SceneContent.Load<SpriteFont>("Fonts/curier_new"));
            AddGuiComponent(label);
            AddGuiComponent(textbox);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime);
        }
    }
}