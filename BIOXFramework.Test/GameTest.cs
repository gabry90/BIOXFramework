using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Mappers;
using BIOXFramework.Scene;
using BIOXFramework.Test.Scenes;

namespace BIOXFramework.Test
{
    public class GameTest : Game
    {
        public GraphicsDeviceManager graphics;

        public GameTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(24);
        }

        protected override void Initialize()
        {
            //load input scene with first scene
            SceneManager.Load<InputTestScene>(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    SceneManager.Clear(this);
                    ServiceManager.Clear();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
