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

        public bool IsCursorVisible
        {
            get { return guiManager.CurrentCursor.Visible; }
            set { guiManager.CurrentCursor.Visible = value; }
        }

        protected static SceneManager sceneManager;
        protected static GuiManager guiManager;
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
            Enabled = true;
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

            if (guiManager == null)
            {
                guiManager = game.Services.GetService<GuiManager>();
                if (guiManager == null)
                    throw new SceneManagerException("the BIOXScene required GuiManager game service!");
            }

            if (songManager == null)
            {
                songManager = game.Services.GetService<SongManager>();
                if (songManager == null)
                    songManager = new SongManager();
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
            //detach game events
            game.Exiting += OnGameExiting;

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
            //detach game events
            game.Exiting -= OnGameExiting;

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

        public virtual void OnGameExiting(object sender, EventArgs e)
        {
            lock (_components)
            {
                _components.ForEach(x => x.Dispose());
                _components.Clear();
            }
        }

        public override void Initialize()
        {
            //attach scene event handlers
            AttachSceneEventHandlers();

            lock (_components)
            {
                //add components to scene
                _components.Add(soundManager);
                _components.Add(keyboardManager);
                _components.Add(mouseManager);
                _components.Add(guiManager.CurrentCursor);
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

            //update all scene components
            for (int i = 0; i < _components.Count; i ++)
            {
                if (_components[i].Enabled && (!isPaused || (isPaused && _components[i].GetType().GetInterfaces().Contains(typeof(INonPausableComponent)))))
                    _components[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            //draw all scene components
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is DrawableGameComponent && _components[i].Enabled && (!isPaused || (isPaused && _components[i].GetType().GetInterfaces().Contains(typeof(INonPausableComponent)))))
                    ((DrawableGameComponent)_components[i]).Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat(@"
------------------------------------------------
                INSTANCE INFO
------------------------------------------------
Type: {0}
Hash code: {1}           
IsCursorVisible: {2}
IsPaused: {3}
------------------------------------------------
                COMPONENTS INFO
------------------------------------------------",
            this.GetType().FullName,
            this.GetHashCode(),
            this.IsCursorVisible,
            this.isPaused);

            content.AppendLine();

            for (int i = 0; i < _components.Count; i++)
            {
                content.AppendFormat(@"Type: {0}     
Enabled: {1}    
Visibile: {2}
------------------------------------------------",
                _components[i].GetType().FullName,
                _components[i].Enabled,
                _components[i] is DrawableGameComponent ? ((DrawableGameComponent)_components[i]).Visible.ToString() : "this object is not a DrawableGameComponent!");
                content.AppendLine();
            }

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
                        //dispose all components
                        for (int i = 0; i < _components.Count; i++)
                        {
                            if (!_components[i].GetType().GetInterfaces().Contains(typeof(IPersistentComponent)))
                                _components[i].Dispose();
                        }

                        //remove scene components
                        _components.Clear();
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