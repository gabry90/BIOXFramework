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
    /*
     * SCENE TEST CYCLE 
     * (Physics3DTestScene) ... <- InputTestScene ->
     *      <- AudioTestScene ->
     *      <- GuiTestScene ->
     *      <- Physics2DTestScene ->
     *      <- Physics3DTestScene -> ...(InputTestScene)
     */

    public class GameTest : Game
    {
        public GraphicsDeviceManager graphics;

        private SceneManager sceneManager;

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
            sceneManager = ServiceManager.Get<SceneManager>();

            //load input scene with first scene
            sceneManager.Load<InputTestScene>(this);

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
                    sceneManager.Clear(this);
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
