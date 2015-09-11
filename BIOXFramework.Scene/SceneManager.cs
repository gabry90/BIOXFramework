using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Scene
{
    public static class SceneManager
    {
        #region vars

        public static event EventHandler<SceneLoadedEventArgs> Loaded;
        public static event EventHandler<SceneUnloadedEventArgs> Unloaded;
        public static event EventHandler<SceneVisibilityChangedEventArgs> VisibilityChanged;
        public static event EventHandler<SceneEnabledChangedEventArgs> EnabledChanged;
        public static event EventHandler<ScenePausedEventArgs> Paused;
        public static event EventHandler<SceneResumedEventArgs> Resumed;

        private static List<Type> _scenes = new List<Type>();
        private static BIOXScene _currentScene = null;

        #endregion

        #region public methods

        public static void Register<T>() where T : BIOXScene
        {
            if (_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is already registered!", typeof(T).FullName));

            lock (_scenes) { _scenes.Add(typeof(T)); }
        }

        public static void Unregister<T>(Game game) where T : BIOXScene
        {
            if (!_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                if (_currentScene != null && _currentScene.GetType() == typeof(T))
                    Unload(game); //unload current scene before unregistering it

                _scenes.Remove(typeof(T));
            }
        }

        public static void Load<T>(Game game) where T : BIOXScene
        {
            if (!_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                if (_currentScene != null)
                {
                    if (_currentScene.GetType() == typeof(T))
                        throw new SceneManagerException(string.Format("the scene \"{0}\" is already loaded!", typeof(T).FullName));
                    else
                        Unload(game); //unload old scene
                }

                //create new scene and add to it's to game components
                _currentScene = (T)Activator.CreateInstance(typeof(T), game);
                game.Components.Add(_currentScene);
            }
        }

        public static void Unload(Game game)
        {
            if (_currentScene == null)
                throw new SceneManagerException("the current scene is not loaded!");

            lock (_scenes)
            {
                _currentScene.Dispose();
                game.Components.Remove(_currentScene);
                _currentScene = null;
            }
        }

        public static BIOXScene GetCurrentScene()
        {
            return _currentScene;
        }

        public static void Clear(Game game)
        {
            Unload(game);
            lock (_scenes) { _scenes.Clear(); }
        }

        #endregion

        #region dispatchers

        internal static void SceneLoadedEventDispatcher(SceneLoadedEventArgs e)
        {
            var h = Loaded;
            if (h != null)
                h(null, e);
        }

        internal static void SceneUnloadedEventDispatcher(SceneUnloadedEventArgs e, Game game)
        {
            var h = Unloaded;
            if (h != null)
                h(null, e);
        }

        internal static void SceneVisibilityChangedEventDispatcher(SceneVisibilityChangedEventArgs e)
        {
            var h = VisibilityChanged;
            if (h != null)
                h(null, e);
        }

        internal static void SceneEnabledChangedEventDispatcher(SceneEnabledChangedEventArgs e)
        {
            var h = EnabledChanged;
            if (h != null)
                h(null, e);
        }

        internal static void ScenePausedEventDispatcher(ScenePausedEventArgs e)
        {
            var h = Paused;
            if (h != null)
                h(null, e);
        }

        internal static void SceneResumedEventDispatcher(SceneResumedEventArgs e)
        {
            var h = Resumed;
            if (h != null)
                h(null, e);
        }

        #endregion
    }
}