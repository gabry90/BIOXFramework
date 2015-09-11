using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using System.Text;

namespace BIOXFramework.Scene
{
    public class BIOXScene : DrawableGameComponent
    {
        #region vars

        protected SceneManager sceneManager;
        protected SongManager songManager;
        protected SoundManager soundManager;
        protected KeyboardManager keyboardManager;
        protected MouseManager mouseManager;
        protected Game game;

        private List<GameComponent> _components;

        #endregion

        #region constructors

        public BIOXScene(Game game)
            : base(game)
        {
            //init vars
            this.game = game;
            Visible = true;
            _components = new List<GameComponent>();
            sceneManager = ServiceManager.Get<SceneManager>();
            songManager = ServiceManager.Get<SongManager>();
            soundManager = ServiceManager.Get<SoundManager>();
            keyboardManager = ServiceManager.Get<KeyboardManager>();
            mouseManager = ServiceManager.Get<MouseManager>();
        }

        #endregion

        #region scene methods

        protected void AddComponent(GameComponent component)
        {
            lock (_components)
            {
                if (component != null && !_components.Contains(component))
                    _components.Add(component);
            }
        }

        protected void RemoveComponent(GameComponent component)
        {
            lock (_components)
            {
                if (component != null && _components.Contains(component))
                    _components.Remove(component);
            }
        }

        public virtual void Pause()
        {
            sceneManager.ScenePausedEventDispatcher(new ScenePausedEventArgs(this.GetType()));
        }

        public virtual void Resume()
        {
            sceneManager.SceneResumedEventDispatcher(new SceneResumedEventArgs(this.GetType()));
        }

        protected void AttachSceneEventHandlers()
        {
            //attach audio events
            songManager.Played += OnSongPlayed;
            songManager.Paused += OnSongPaused;
            songManager.Resumed += OnSongResumed;
            songManager.Stopped += OnSongStopped;
            soundManager.Played += OnSoundPlayed;
            soundManager.Paused += OnSoundPaused;
            soundManager.Resumed += OnSoundResumed;
            soundManager.Stopped += OnSoundStopped;

            //attach input events
            keyboardManager.Pressed += OnKeyPressed;
            keyboardManager.Pressing += OnKeyPressing;
            keyboardManager.Released += OnKeyReleased;
            mouseManager.Pressed += OnMousePressed;
            mouseManager.Pressing += OnMousePressing;
            mouseManager.Released += OnMouseReleased;
            mouseManager.WhellUp += OnMouseWhellUp;
            mouseManager.WhellDown += OnMouseWhellDown;
            mouseManager.PositionChanged += OnMousePositionChanged;
        }

        protected void DetachSceneEventHandlers()
        {
            //detach audio events
            songManager.Played -= OnSongPlayed;
            songManager.Paused -= OnSongPaused;
            songManager.Resumed -= OnSongResumed;
            songManager.Stopped -= OnSongStopped;
            soundManager.Played -= OnSoundPlayed;
            soundManager.Paused -= OnSoundPaused;
            soundManager.Resumed -= OnSoundResumed;
            soundManager.Stopped -= OnSoundStopped;

            //detach input events
            keyboardManager.Pressed -= OnKeyPressed;
            keyboardManager.Pressing -= OnKeyPressing;
            keyboardManager.Released -= OnKeyReleased;
            mouseManager.Pressed -= OnMousePressed;
            mouseManager.Pressing -= OnMousePressing;
            mouseManager.Released -= OnMouseReleased;
            mouseManager.WhellUp -= OnMouseWhellUp;
            mouseManager.WhellDown -= OnMouseWhellDown;
            mouseManager.PositionChanged -= OnMousePositionChanged;
        }

        #endregion

        #region audio events

        protected virtual void OnSongPlayed(object sender, SongPlayedEventArgs e)
        {

        }

        protected virtual void OnSongPaused(object sender, SongPausedEventArgs e)
        {

        }

        protected virtual void OnSongResumed(object sender, SongResumedEventArgs e)
        {

        }

        protected virtual void OnSongStopped(object sender, SongStoppedEventArgs e)
        {

        }

        protected virtual void OnSoundPlayed(object sender, SoundPlayedEventArgs e)
        {

        }

        protected virtual void OnSoundPaused(object sender, SoundPausedEventArgs e)
        {

        }

        protected virtual void OnSoundResumed(object sender, SoundResumedEventArgs e)
        {

        }

        protected virtual void OnSoundStopped(object sender, SoundStoppedEventArgs e)
        {

        }

        #endregion

        #region input events

        protected virtual void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {

        }

        protected virtual void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {

        }

        protected virtual void OnKeyReleased(object sender, KeyboardReleasedEventArgs e)
        {

        }

        protected virtual void OnMousePressed(object sender, MousePressedEventArgs e)
        {

        }

        protected virtual void OnMousePressing(object sender, MousePressingEventArgs e)
        {

        }

        protected virtual void OnMouseReleased(object sender, MouseReleasedEventArgs e)
        {

        }

        protected virtual void OnMouseWhellUp(object sender, EventArgs e)
        {

        }

        protected virtual void OnMouseWhellDown(object sender, EventArgs e)
        {

        }

        protected virtual void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {

        }

        #endregion

        #region game implementations

        public override void Initialize()
        {
            //attach scene event handlers
            AttachSceneEventHandlers();

            lock (_components)
            {
                //add components to scene
                _components.Add(soundManager);
                _components.Add(songManager);
                _components.Add(keyboardManager);
                _components.Add(mouseManager);
            }

            base.Initialize();

            sceneManager.SceneLoadedEventDispatcher(new SceneLoadedEventArgs(this.GetType()));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                lock (_components)
                {
                    for (int i = 0; i < _components.Count; i ++)
                    {
                        if (_components[i].Enabled)
                            _components[i].Update(gameTime);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                lock (_components)
                {
                    for (int i = 0; i < _components.Count; i++)
                    {
                        if (_components[i] is DrawableGameComponent)
                            ((DrawableGameComponent)_components[i]).Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("Type: {0}", this.GetType().FullName);
            content.AppendFormat("Hash code: {0}", this.GetHashCode());
            return content.ToString();
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    //dispose events
                    DetachSceneEventHandlers();

                    Enabled = false;
                    Visible = false;

                    lock (_components)
                    {
                        //remove scene components
                        _components.Remove(soundManager);
                        _components.Remove(songManager);
                        _components.Remove(keyboardManager);
                        _components.Remove(mouseManager);
                    }

                    //dispatch unloaded event
                    sceneManager.SceneUnloadedEventDispatcher(new SceneUnloadedEventArgs(this.GetType()), game);
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