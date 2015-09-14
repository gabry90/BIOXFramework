using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using System.Text;
using BIOXFramework.GUI;
using BIOXFramework.GUI.Components;

namespace BIOXFramework.Scene
{
    public class BIOXScene : DrawableGameComponent
    {
        #region vars

        protected static SceneManager sceneManager;
        protected static SongManager songManager;
        protected static SoundManager soundManager;
        protected static KeyboardManager keyboardManager;
        protected static MouseManager mouseManager;
        protected Game game;
        protected bool isPaused = false;

        private List<GameComponent> _components;

        #endregion

        #region constructors

        public BIOXScene(Game game)
            : base(game)
        {
            this.game = game;
            Visible = true;
            _components = new List<GameComponent>();
            InitializeServices();
        }

        #endregion

        #region public methods

        public virtual void Pause()
        {
            if (isPaused) return;
            isPaused = true;
            sceneManager.ScenePausedEventDispatcher(new ScenePausedEventArgs(this.GetType()));
        }

        public virtual void Resume()
        {
            if (!isPaused) return;
            isPaused = false;
            sceneManager.SceneResumedEventDispatcher(new SceneResumedEventArgs(this.GetType()));
        }

        #endregion

        #region private / protected methods

        protected virtual void InitializeServices()
        {
            if (sceneManager == null)
            {
                sceneManager = game.Services.GetService<SceneManager>();
                if (sceneManager == null)
                    throw new SceneManagerException("the BIOXScene required SceneManager game service!");
            }

            if (songManager == null)
            {
                songManager = game.Services.GetService<SongManager>();
                if (songManager == null)
                    songManager = new SongManager(game);
            }
                
            if (soundManager == null)
            {
                soundManager = game.Services.GetService<SoundManager>();
                if (soundManager == null)
                    soundManager = new SoundManager(game);
            }

            if (keyboardManager == null)
            {
                keyboardManager = game.Services.GetService<KeyboardManager>();
                if (keyboardManager == null)
                    keyboardManager = new KeyboardManager(game);
            }

            if (mouseManager == null)
            {
                mouseManager = game.Services.GetService<MouseManager>();
                if (mouseManager == null)
                    mouseManager = new MouseManager(game);
            }
        }

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

        protected virtual void AttachSceneEventHandlers()
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

        protected virtual void DetachSceneEventHandlers()
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
            //add common logic here...
        }

        protected virtual void OnSongPaused(object sender, SongPausedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSongResumed(object sender, SongResumedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSongStopped(object sender, SongStoppedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSoundPlayed(object sender, SoundPlayedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSoundPaused(object sender, SoundPausedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSoundResumed(object sender, SoundResumedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnSoundStopped(object sender, SoundStoppedEventArgs e)
        {
            //add common logic here...
        }

        #endregion

        #region input events

        protected virtual void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnKeyPressing(object sender, KeyboardPressingEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnKeyReleased(object sender, KeyboardReleasedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMousePressed(object sender, MousePressedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMousePressing(object sender, MousePressingEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMouseReleased(object sender, MouseReleasedEventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMouseWhellUp(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMouseWhellDown(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {
            //add common logic here...
        }

        #endregion

        #region component implementations

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
            //add common logic here...
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            //add common logic here...
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            lock (_components)
            {
                for (int i = 0; i < _components.Count; i ++)
                {
                    if (!isPaused || (isPaused && 
                        _components[i] is MouseManager ||
                        _components[i] is KeyboardManager ||
                        _components[i] is SoundManager ||
                        _components[i] is SongManager ||
                        _components[i] is GuiBase))
                    {   //excluding managers from paused status
                        if (_components[i].Enabled)
                            _components[i].Update(gameTime);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            lock (_components)
            {
                for (int i = 0; i < _components.Count; i++)
                {
                    //excluding managers from paused status
                    if ((!isPaused || (isPaused &&
                        _components[i] is MouseManager ||
                        _components[i] is KeyboardManager ||
                        _components[i] is SoundManager ||
                        _components[i] is SongManager ||
                        _components[i] is GuiBase)) &&
                        _components[i] is DrawableGameComponent)
                        ((DrawableGameComponent)_components[i]).Draw(gameTime);
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