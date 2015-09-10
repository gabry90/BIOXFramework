using System;
using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Test.Scenes;

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
            //register audio services
            ServiceManager.Register<SongManager>(new SongManager(game));
            ServiceManager.Register<SoundManager>(new SoundManager(game));

            //register input services
            ServiceManager.Register<KeyboardManager>(new KeyboardManager(game));
            ServiceManager.Register<MouseManager>(new MouseManager(game));
            ServiceManager.Register<GamepadManager>(new GamepadManager(game));
        }

        private static void InitScenes(GameTest game)
        {
            //register scenes
            SceneManager.Register<InputTestScene>();
            SceneManager.Register<AudioTestScene>();
            SceneManager.Register<GuiTestScene>();
            SceneManager.Register<Physics2DTestScene>();
            SceneManager.Register<Physics3DTestScene>();
        }
    }
#endif
}
