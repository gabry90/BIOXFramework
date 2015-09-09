using System;
using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;

namespace BIOXFramework.Test
{
#if WINDOWS || LINUX
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
            using (var game = new Game1())
            {
                InitService(game);
                game.Run();
            }
        }

        private static void InitService(Game1 game)
        {
            //register audio services
            ServiceManager.Register<SongManager>(new SongManager(game));
            ServiceManager.Register<SoundManager>(new SoundManager(game));

            //register input services
            ServiceManager.Register<KeyboardManager>(new KeyboardManager(game));
            ServiceManager.Register<MouseManager>(new MouseManager(game));
            ServiceManager.Register<GamepadManager>(new GamepadManager(game));
        }
    }
#endif
}
