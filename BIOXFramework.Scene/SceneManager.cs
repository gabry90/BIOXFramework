using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using BIOXFramework.Services;

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

        private static ConcurrentDictionary<Type, BIOXScene> _scenes = new ConcurrentDictionary<Type, BIOXScene>();

        #endregion

        #region public methods

        public static void Register<T>() where T : BIOXScene
        {
            if (_scenes.ContainsKey(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is already registered!", typeof(T).FullName));

            lock (_scenes) { _scenes.TryAdd(typeof(T), null); }
        }

        public static void Unregister<T>() where T : BIOXScene
        {
            if (!_scenes.ContainsKey(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                BIOXScene scene = _scenes[typeof(T)];
                scene.Dispose();
                _scenes.TryRemove(typeof(T), out scene);
            }
        }

        public static T Get<T>() where T : BIOXScene
        {
            if (!_scenes.ContainsKey(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            return (T)_scenes[typeof(T)];
        }

        public static void Load<T>(Game game) where T : BIOXScene
        {
            if (!_scenes.ContainsKey(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                T scene = _scenes[typeof(T)] as T;
                if (scene != null)
                    throw new SceneManagerException(string.Format("the scene \"{0}\" is already loaded!", typeof(T).FullName));

                scene = (T)Activator.CreateInstance(typeof(T), game);
                _scenes[typeof(T)] = scene;
                game.Components.Add(scene);
            }
        }

        public static void Unload<T>(Game game) where T : BIOXScene
        {
            if (!_scenes.ContainsKey(typeof(T)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(T).FullName));

            lock (_scenes)
            {
                T scene = _scenes[typeof(T)] as T;
                if (scene == null)
                    throw new SceneManagerException(string.Format("the scene \"{0}\" is not loaded!", typeof(T).FullName));

                scene.Dispose();
            }
        }

        public static void Switch<CURRENT, NEW>(Game game) 
            where CURRENT : BIOXScene
            where NEW : BIOXScene
        {
            if (!_scenes.ContainsKey(typeof(CURRENT)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(CURRENT).FullName));

            if (!_scenes.ContainsKey(typeof(NEW)))
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not registered!", typeof(NEW).FullName));

            CURRENT currentScene = _scenes[typeof(CURRENT)] as CURRENT;
            if (currentScene == null)
                throw new SceneManagerException(string.Format("the scene \"{0}\" is not loaded!", typeof(CURRENT).FullName));

            //unload current scene
            Unload<CURRENT>(game);

            NEW newScene = _scenes[typeof(NEW)] as NEW;
            if (newScene != null)
                newScene.Visible = true;    //scene is already loaded
            else
                Load<NEW>(game);            //scene is not loaded
        }

        public static void Clear(Game game)
        {
            lock (_scenes)
            {
                foreach (KeyValuePair<Type, BIOXScene> x in _scenes)
                {
                    if (x.Value != null)
                        x.Value.Dispose();
                }
                _scenes.Clear();
            }
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
            //remove scene from collection when it's unloaded
            lock (_scenes)
            {
                IGameComponent scene = _scenes[e.Type];
                if (scene != null)
                {
                    game.Components.Remove(scene);
                    _scenes[e.GetType()] = null;
                }
            }

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