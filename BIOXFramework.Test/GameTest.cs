using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Mappers;
using BIOXFramework.Scene;
using BIOXFramework.Test.Scenes;
using BIOXFramework.GUI;
using BIOXFramework.GUI.Components;
using BIOXFramework.Input.Events;

namespace BIOXFramework.Test
{
    /*
     * SCENE TEST CYCLE 
     * (UtilityTestScene) ... <- InputTestScene ->
     *      <- AudioTestScene ->
     *      <- GuiTestScene ->
     *      <- Physics2DTestScene ->
     *      <- Physics3DTestScene ->
     *      <- UtilityTestScene -> ...(InputTestScene)
     */

    public class GameTest : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private SceneManager sceneManager;
        private GuiManager guiManager;
        private KeyboardManager keyboardManager;

        public GameTest()
        {        
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;
            IsMouseVisible = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(24);
        }

        private void OnSceneLoaded(object sender, SceneLoadedEventArgs e)
        {
            Console.WriteLine("SCENE LOADED:   " + e.Type.FullName);
        }

        private void OnSceneUnloaded(object sender, SceneUnloadedEventArgs e)
        {
            Console.WriteLine("SCENE UNLOADED: " + e.Type.FullName);
        }

        private void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            if (e.Key == Keys.Escape)
                Exit();
        }

        protected override void Initialize()
        {
            sceneManager = this.Services.GetService<SceneManager>();
            sceneManager.GameObj = this;
            sceneManager.Loaded += OnSceneLoaded;
            sceneManager.Unloaded += OnSceneUnloaded;

            keyboardManager = this.Services.GetService<KeyboardManager>();
            keyboardManager.Pressed += OnKeyPressed;

            //add spriteBatch to services
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);

            //set cursor for all scenes
            guiManager = this.Services.GetService<GuiManager>();
            guiManager.CurrentCursor = new Cursor(this, Content.Load<Texture2D>("UI image/cursor"));

            //load input scene with first scene
            sceneManager.Load<InputTestScene>();

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
                    keyboardManager.Pressed -= OnKeyPressed;

                    sceneManager.Dispose();
                    spriteBatch.Dispose();
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
