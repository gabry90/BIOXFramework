using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Scene
{
    public sealed class SceneManager : IPersistenceComponent
    {
        #region vars

        public event EventHandler<SceneLoadedEventArgs> Loaded;
        public event EventHandler<SceneUnloadedEventArgs> Unloaded;
        public event EventHandler<ScenePausedEventArgs> Paused;
        public event EventHandler<SceneResumedEventArgs> Resumed;

        private List<Type> _scenes = new List<Type>();
        private BIOXScene _currentScene = null;

        #endregion

        #region public methods

        public void Register<T>() where T : BIOXScene
        {
            if (_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is already registered!", typeof(T).FullName));

            lock (_scenes) { _scenes.Add(typeof(T)); }
        }

        public void Unregister<T>(Game game) where T : BIOXScene
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

        public void Load<T>(Game game) where T : BIOXScene
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

        public void Unload(Game game)
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

        public BIOXScene GetCurrentScene()
        {
            return _currentScene;
        }

        public void Clear(Game game)
        {
            try
            {
                Unload(game);
            }
            finally
            {
                lock (_scenes) { _scenes.Clear(); }
            }
        }

        #endregion

        #region dispatchers

        internal void SceneLoadedEventDispatcher(SceneLoadedEventArgs e)
        {
            var h = Loaded;
            if (h != null)
                h(null, e);
        }

        internal void SceneUnloadedEventDispatcher(SceneUnloadedEventArgs e, Game game)
        {
            var h = Unloaded;
            if (h != null)
                h(null, e);
        }

        internal void ScenePausedEventDispatcher(ScenePausedEventArgs e)
        {
            var h = Paused;
            if (h != null)
                h(null, e);
        }

        internal void SceneResumedEventDispatcher(SceneResumedEventArgs e)
        {
            var h = Resumed;
            if (h != null)
                h(null, e);
        }

        #endregion

        #region dispose

        public void Dispose()
        {

        }

        #endregion
    }
}