using System;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Settings;
using BIOXFramework.Test.Scenes;
using BIOXFramework.GUI;
using BIOXFramework.GUI.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BIOXFramework.Physics2D.Collision;

namespace BIOXFramework.Test
{
#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameTest())
            {
                InitService(game);
                InitScenes(game);
                game.Run();
            }
        }

        private static void InitService(GameTest game)
        {
            //register services
            game.Content.RootDirectory = "Content";
            game.Services.AddService<ContentManager>(game.Content);
            game.Services.AddService<SettingsManager>(new SettingsManager(game));
            game.Services.AddService<SceneManager>(new SceneManager(game));
            game.Services.AddService<SongManager>(new SongManager(game));
            game.Services.AddService<SoundManager>(new SoundManager(game));
            game.Services.AddService<KeyboardManager>(new KeyboardManager(game));
            game.Services.AddService<MouseManager>(new MouseManager(game));
            game.Services.AddService<GamepadManager>(new GamepadManager(game));
            game.Services.AddService<GuiManager>(new GuiManager(game));
            game.Services.AddService<Collision2DManager>(new Collision2DManager(game));
        }

        private static void InitScenes(GameTest game)
        {
            //register scenes
            SceneManager manager = game.Services.GetService<SceneManager>();
            manager.Register<InputTestScene>();
            manager.Register<AudioTestScene>();
            manager.Register<GuiTestScene>();
            manager.Register<PhysicsTestScene>();
            manager.Register<UtilityTestScene>();
        }
    }
#endif
}
