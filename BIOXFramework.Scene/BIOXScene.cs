using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;

namespace BIOXFramework.Scene
{
    public class BIOXScene : DrawableGameComponent
    {
        #region vars

        protected SongManager songManager;
        protected SoundManager soundManager;
        protected KeyboardManager keyboardManager;
        protected MouseManager mouseManager;

        protected Game game;
        protected List<GameComponent> Components;

        #endregion

        #region constructors

        public BIOXScene(Game game)
            : base(game)
        {
            //init vars
            this.game = game;
            Visible = true;
            Components = new List<GameComponent>();
            songManager = ServiceManager.Get<SongManager>();
            soundManager = ServiceManager.Get<SoundManager>();
            keyboardManager = ServiceManager.Get<KeyboardManager>();
            mouseManager = ServiceManager.Get<MouseManager>();

            //init game base events
            this.EnabledChanged += (o, e) => { SceneManager.SceneEnabledChangedEventDispatcher(new SceneEnabledChangedEventArgs(this.GetType(), this.Enabled)); };
            this.VisibleChanged += (o, e) => { SceneManager.SceneVisibilityChangedEventDispatcher(new SceneVisibilityChangedEventArgs(this.GetType(), this.Visible)); };
        }

        #endregion

        #region game implementations

        public override void Initialize()
        {
            //attach scene event handlers
            AttachSceneEventHandlers();

            //add components to scene
            Components.Add(soundManager);
            Components.Add(songManager);
            Components.Add(keyboardManager);
            Components.Add(mouseManager);

            base.Initialize();

            SceneManager.SceneLoadedEventDispatcher(new SceneLoadedEventArgs(this.GetType()));
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
                Parallel.ForEach(Components, x =>
                {
                    GameComponent component = (GameComponent)x;
                    if (component.Enabled)
                        component.Update(gameTime);
                });
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                Parallel.ForEach(Components, x =>
                {
                    GameComponent component = (GameComponent)x;
                    if (component is DrawableGameComponent && component.Enabled)
                        ((DrawableGameComponent)component).Draw(gameTime);
                });
            }
            base.Draw(gameTime);
        }

        #endregion

        #region scene methods

        public virtual void Pause()
        {
            SceneManager.ScenePausedEventDispatcher(new ScenePausedEventArgs(this.GetType()));
        }

        public virtual void Resume()
        {
            SceneManager.SceneResumedEventDispatcher(new SceneResumedEventArgs(this.GetType()));
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

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    //dispose events
                    DetachSceneEventHandlers();

                    //remove scene components
                    Components.Remove(soundManager);
                    Components.Remove(songManager);
                    Components.Remove(keyboardManager);
                    Components.Remove(mouseManager);

                    //dispatch unloaded event
                    SceneManager.SceneUnloadedEventDispatcher(new SceneUnloadedEventArgs(this.GetType()), game);
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