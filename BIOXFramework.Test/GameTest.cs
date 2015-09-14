using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private void OnSceneLoaded(object sender, SceneLoadedEventArgs e)
        {
            Console.WriteLine("SCENE LOADED: " + e.Type.FullName);
        }

        private void OnSceneUnloaded(object sender, SceneUnloadedEventArgs e)
        {
            Console.WriteLine("SCENE UNLOADED: " + e.Type.FullName);
        }

        protected override void Initialize()
        {
            sceneManager = this.Services.GetService<SceneManager>();
            sceneManager.Loaded += OnSceneLoaded;
            sceneManager.Unloaded += OnSceneUnloaded;

            //load input scene with first scene
            sceneManager.Load<InputTestScene>(this);

            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    sceneManager.Loaded -= OnSceneLoaded;
                    sceneManager.Unloaded -= OnSceneUnloaded;
                    sceneManager.Clear(this);
                    graphics.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
