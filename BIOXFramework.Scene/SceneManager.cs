using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Scene
{
    public sealed class SceneManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        #region vars

        public event EventHandler<SceneLoadedEventArgs> Loaded;
        public event EventHandler<SceneUnloadedEventArgs> Unloaded;
        public event EventHandler<ScenePausedEventArgs> Paused;
        public event EventHandler<SceneResumedEventArgs> Resumed;

        public Game GameObj = null;

        private List<Type> _scenes = new List<Type>();
        private BIOXScene _currentScene = null;

        #endregion

        #region constructors

        public SceneManager(Game game)
            : base(game)
        {

        }

        #endregion

        #region public methods

        public void Register<T>() where T : BIOXScene
        {
            if (_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is already registered!", typeof(T).FullName));

            lock (_scenes) { _scenes.Add(typeof(T)); }
        }

        public void Unregister<T>() where T : BIOXScene
        {
            if (!_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                if (_currentScene != null && _currentScene.GetType() == typeof(T))
                    Unload(); //unload current scene before unregistering it

                _scenes.Remove(typeof(T));
            }
        }

        public void Load<T>() where T : BIOXScene
        {
            if (!_scenes.Contains(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            if (GameObj == null)
                throw new SceneManagerException("Game object not setted!");

            lock (_scenes)
            {
                if (_currentScene != null)
                {
                    if (_currentScene.GetType() == typeof(T))
                        throw new SceneManagerException(string.Format("the scene \"{0}\" is already loaded!", typeof(T).FullName));
                    else
                        Unload(); //unload old scene
                }

                //create new scene and add to it's to game components
                _currentScene = (T)Activator.CreateInstance(typeof(T), GameObj);
                GameObj.Components.Add(_currentScene);
            }
        }

        public void Unload()
        {
            if (_currentScene == null)
                throw new SceneManagerException("the current scene is not loaded!");

            if (GameObj == null)
                throw new SceneManagerException("Game object not setted!");

            lock (_scenes)
            {
                _currentScene.Dispose();
                GameObj.Components.Remove(_currentScene);
                _currentScene = null;
            }
        }

        public BIOXScene GetCurrentScene()
        {
            return _currentScene;
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Unload();
                    if (Loaded != null) Loaded = null;
                    if (Unloaded != null) Unloaded = null;
                    if (Paused != null) Paused = null;
                    lock (_scenes) { _scenes.Clear(); }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}