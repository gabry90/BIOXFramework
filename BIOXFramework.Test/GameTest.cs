﻿using System;
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
     *      <- UtilityTestScene -> ...(InputTestScene)
     */

    public class GameTest : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private SceneManager sceneManager;
        private KeyboardManager keyboardManager;

        public GameTest()
        {        
            graphics = new GraphicsDeviceManager(this);

            IsFixedTimeStep = true;
            IsMouseVisible = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(24);
        }

        private void OnSceneInitialized(object sender, SceneEventArgs e)
        {
            sceneManager.GetCurrentScene().SetCursor("UI image/cursor");
            Console.WriteLine("SCENE INITIALIZED:   " + e.Type.FullName);
        }

        private void OnSceneLoaded(object sender, SceneEventArgs e)
        {
            Console.WriteLine("SCENE LOADED:        " + e.Type.FullName);
        }

        private void OnSceneUnloaded(object sender, SceneEventArgs e)
        {
            Console.WriteLine("SCENE UNLOADED:      " + e.Type.FullName);
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
            sceneManager.Initialized += OnSceneInitialized;
            sceneManager.Loaded += OnSceneLoaded;
            sceneManager.Unloaded += OnSceneUnloaded;

            keyboardManager = this.Services.GetService<KeyboardManager>();
            keyboardManager.Pressed += OnKeyPressed;

            //add spriteBatch to services
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);

            //load input scene with first scene
            sceneManager.Load<PhysicsTestScene>();

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    sceneManager.Initialized -= OnSceneInitialized;
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
