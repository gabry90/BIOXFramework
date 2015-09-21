using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using System.Text;
using BIOXFramework.GUI;
using Microsoft.Xna.Framework.Content;
using BIOXFramework.Physics2D.Collision;
using BIOXFramework.Physics2D;

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
        protected static Collision2DManager collision2DManager;

        protected ContentManager SceneContent;
        protected Game game;
        protected bool isPaused = false;
        protected bool exitGameRequest = false;

        private List<GameComponent> _gameComponents;
        private List<DrawableGameComponent> _drawableGameComponents;

        #endregion

        #region constructors

        public BIOXScene(Game game)
            : base(game)
        {
            this.game = game;
            Visible = true;
            Enabled = true;
            _gameComponents = new List<GameComponent>();
            _drawableGameComponents = new List<DrawableGameComponent>();
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
                BIOXOject INFO
------------------------------------------------",
            this.GetType().FullName,
            this.GetHashCode(),
            this.IsCursorVisible,
            this.isPaused);

            content.AppendLine();

            for (int i = 0; i < _drawableGameComponents.Count; i++)
            {
                content.AppendFormat(@"Type: {0}     
Enabled: {1}    
Visibile: {2}
------------------------------------------------",
                _drawableGameComponents[i].GetType().FullName,
                _drawableGameComponents[i].Enabled,
                _drawableGameComponents[i].Visible);
                content.AppendLine();
            }

            return content.ToString();
        }

        #endregion

        #region private / protected methods

        private void InitializeServices()
        {
            if (SceneContent == null)
            {
                ContentManager content = game.Services.GetService<ContentManager>();
                if (content == null)
                    throw new SceneManagerException("the BIOXScene required ContentManager game service!");
                SceneContent = new ContentManager(content.ServiceProvider, content.RootDirectory);
            }

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

            if (collision2DManager == null)
            {
                collision2DManager = game.Services.GetService<Collision2DManager>();
                if (collision2DManager == null)
                    collision2DManager = new Collision2DManager(game);

                collision2DManager.EnableCollisionDetection = false; //disable for default
            }
        }

        protected void AddGameComponent(GameComponent component)
        {
            lock (_gameComponents)
            {
                if (component != null && !_gameComponents.Contains(component))
                {
                    _gameComponents.Add(component);
                    collision2DManager.Components.Add(component);
                }
            }
        }

        protected void RemoveGameComponent(GameComponent component)
        {
            lock (_gameComponents)
            {
                if (component != null && _gameComponents.Contains(component))
                {
                    collision2DManager.Components.Remove(component);
                    if (!component.GetType().GetInterfaces().Contains(typeof(IPersistentComponent)))
                        component.Dispose();
                    _gameComponents.Remove(component);
                }
            }
        }

        protected void AddDrawableGameComponent(DrawableGameComponent component)
        {
            lock (_drawableGameComponents)
            {
                if (component != null && !_drawableGameComponents.Contains(component))
                {
                    collision2DManager.Components.Add(component);
                    _drawableGameComponents.Add(component);
                }
            }
        }

        protected void RemoveDrawableGameComponent(DrawableGameComponent component)
        {
            lock (_drawableGameComponents)
            {
                if (component != null && _drawableGameComponents.Contains(component))
                {
                    collision2DManager.Components.Remove(component);
                    if (!component.GetType().GetInterfaces().Contains(typeof(IPersistentComponent)))
                        component.Dispose();

                    _drawableGameComponents.Remove(component);
                }
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

            //attach physics 2D events
            collision2DManager.Collide += On2DObjectCollide;
            collision2DManager.InCollision += On2DObjectInCollision;
            collision2DManager.OutCollision += On2DObjectOutCollision;
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

            //detach physics 2D events
            collision2DManager.Collide -= On2DObjectCollide;
            collision2DManager.InCollision -= On2DObjectInCollision;
            collision2DManager.OutCollision -= On2DObjectOutCollision;
        }

        protected virtual void OnGameExiting(object sender, EventArgs e)
        {
            exitGameRequest = true;
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

        #region physics 2D events

        protected virtual void On2DObjectCollide(object sender, Collide2DEventArgs e)
        {

        }

        protected virtual void On2DObjectInCollision(object sender, Collide2DEventArgs e)
        {

        }

        protected virtual void On2DObjectOutCollision(object sender, Collide2DEventArgs e)
        {

        }

        #endregion

        #region component implementations

        public override void Initialize()
        {
            //attach scene event handlers
            AttachSceneEventHandlers();

            //add game component to scene
            _gameComponents.Add(soundManager);
            _gameComponents.Add(keyboardManager);
            _gameComponents.Add(mouseManager);
            _gameComponents.Add(collision2DManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //add common logic here...
            base.LoadContent();

            //add cursor over all drawable game component
            _drawableGameComponents.Add(guiManager.CurrentCursor);

            //dispatch scene loaded event
            sceneManager.SceneLoadedEventDispatcher(new SceneLoadedEventArgs(this.GetType()));
        }

        protected override void UnloadContent()
        {
            //unload current scene content
            SceneContent.Unload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //update all game component
            for (int i = 0; i < _gameComponents.Count; i ++)
            {
                if (_gameComponents[i].Enabled && (!isPaused || (isPaused && _gameComponents[i].GetType().GetInterfaces().Contains(typeof(INonPausableComponent)))))
                    _gameComponents[i].Update(gameTime);
            }

            //update all drawable component
            for (int i = 0; i < _drawableGameComponents.Count; i++)
            {
                if (_drawableGameComponents[i].Enabled && (!isPaused || (isPaused && _drawableGameComponents[i].GetType().GetInterfaces().Contains(typeof(INonPausableComponent)))))
                    _drawableGameComponents[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //draw all drawable game component
            for (int i = 0; i < _drawableGameComponents.Count; i++)
            {
                if ( _drawableGameComponents[i].Enabled && (!isPaused || (isPaused && _drawableGameComponents[i].GetType().GetInterfaces().Contains(typeof(INonPausableComponent)))))
                    _drawableGameComponents[i].Draw(gameTime);
            }

            base.Draw(gameTime);
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

                    //clear collission components reference for this scene
                    collision2DManager.Components.Clear();

                    //disposing all game component
                    lock (_gameComponents)
                    {
                        for (int i = 0; i < _gameComponents.Count; i++)
                        {
                            if (exitGameRequest || !_gameComponents[i].GetType().GetInterfaces().Contains(typeof(IPersistentComponent)))
                                _gameComponents[i].Dispose();
                        }
                        _gameComponents.Clear();
                    }

                    //disposing all drawable game component
                    lock (_drawableGameComponents)
                    {
                        for (int i = 0; i < _drawableGameComponents.Count; i++)
                        {
                            if (exitGameRequest || !_drawableGameComponents[i].GetType().GetInterfaces().Contains(typeof(IPersistentComponent)))
                                _drawableGameComponents[i].Dispose();
                        }
                        _drawableGameComponents.Clear();
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