using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Physics.Collision;
using BIOXFramework.Physics;
using BIOXFramework.Physics.Gravity;
using BIOXFramework.GUI.Components;
using BIOXFramework.Utility.Extensions;

namespace BIOXFramework.Scene
{
    public class BIOXScene : DrawableGameComponent
    {
        #region vars

        public bool IsCursorVisible
        {
            get { return CurrentCursor == null ? false : CurrentCursor.Visible; }
            set { if (CurrentCursor != null) CurrentCursor.Visible = value; }
        }
        public Cursor CurrentCursor 
        {
            get { return currentCursor; }
        }
        public bool EnableGui = true;

        protected static SceneManager sceneManager;
        protected static SongManager songManager;
        protected static SoundManager soundManager;
        protected static KeyboardManager keyboardManager;
        protected static MouseManager mouseManager;
        protected static Collision2DManager collision2DManager;
        protected static GravityManager gravityManager;

        protected ContentManager SceneContent;
        protected Game game;
        protected bool isPaused = false;
        protected bool exitGameRequest = false;

        private Cursor currentCursor;
        private ContentManager cursorContent;
        private List<GameComponent> gameComponents;
        private List<DrawableGameComponent> drawableGameComponents;
        private List<GuiBase> guiComponents;

        #endregion

        #region constructors

        public BIOXScene(Game game)
            : base(game)
        {
            this.game = game;
            Visible = true;
            Enabled = true;
            gameComponents = new List<GameComponent>();
            drawableGameComponents = new List<DrawableGameComponent>();
            guiComponents = new List<GuiBase>();
            InitializeServices();
        }

        #endregion

        #region public methods

        public void SetCursor(string path, bool lastValidPosition = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            lock (guiComponents)
            {
                if (currentCursor != null && !currentCursor.IsDisposed)
                {
                    CurrentCursor.Dispose();
                    cursorContent.Unload();
                }

                try
                {
                    currentCursor = new Cursor(game, cursorContent.Load<Texture2D>(path), Vector2.Zero);
                    if (lastValidPosition)
                        currentCursor.Position = new Vector2(mouseManager.MousePosition.X, mouseManager.MousePosition.Y);
                    else
                    {
                        currentCursor.Position = new Vector2(
                            (game.GraphicsDevice.Viewport.Bounds.Size.X / 2) + (currentCursor.Rectangle.Width / 2),
                            (game.GraphicsDevice.Viewport.Bounds.Size.Y / 2) + (currentCursor.Rectangle.Height / 2));
                    }
                }
                catch (Exception ex)
                {
                    throw new SceneManagerException(ex.Message);
                }
            }
        }

        public virtual void Pause()
        {
            if (isPaused) return;
            isPaused = true;
            sceneManager.ScenePausedEventDispatcher(new SceneEventArgs(this.GetType()));
        }

        public virtual void Resume()
        {
            if (!isPaused) return;
            isPaused = false;
            sceneManager.SceneResumedEventDispatcher(new SceneEventArgs(this.GetType()));
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

            content.AppendFormat(@"
GUI Components: {0}
------------------------------------------------", guiComponents.Count);

            for (int i = 0; i < guiComponents.Count; i++)
            {
                content.AppendFormat(@"
Type:       {0}
HashCode:   {1}
Interfaces: {2}     
Enabled:    {3}
------------------------------------------------",
                guiComponents[i].GetType().FullName,
                string.Join(", ", guiComponents[i].GetType().GetInterfaces().Select(x => x.Name)),
                guiComponents[i].GetHashCode(),
                guiComponents[i].Enabled);
            }

            content.AppendFormat(@"
Game Components: {0}
------------------------------------------------", gameComponents.Count);

            for (int i = 0; i < gameComponents.Count; i++)
            {
                content.AppendFormat(@"
Type:       {0}
HashCode:   {1}
Interfaces: {2}     
Enabled:    {3}
------------------------------------------------",
                gameComponents[i].GetType().FullName,
                string.Join(", ", gameComponents[i].GetType().GetInterfaces().Select(x => x.Name)),
                gameComponents[i].GetHashCode(),
                gameComponents[i].Enabled);
            }

            content.AppendFormat(@"
Drawable Game Components: {0}
------------------------------------------------", drawableGameComponents.Count);

            for (int i = 0; i < drawableGameComponents.Count; i++)
            {
                content.AppendFormat(@"
Type:       {0}
HashCode:   {1}
Interfaces: {2}     
Enabled:    {3}
Visibile:   {4}
------------------------------------------------",
                drawableGameComponents[i].GetType().FullName,
                drawableGameComponents[i].GetHashCode(),
                string.Join(", ", drawableGameComponents[i].GetType().GetInterfaces().Select(x => x.Name)),
                drawableGameComponents[i].Enabled,
                drawableGameComponents[i].Visible);
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
                cursorContent = new ContentManager(content.ServiceProvider, content.RootDirectory);
            }

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

            if (collision2DManager == null)
            {
                collision2DManager = game.Services.GetService<Collision2DManager>();
                if (collision2DManager == null)
                    collision2DManager = new Collision2DManager(game);

                collision2DManager.EnableCollisionDetection = false; //disable for default
            }

            if (gravityManager == null)
            {
                gravityManager = game.Services.GetService<GravityManager>();
                if (gravityManager == null)
                    gravityManager = new GravityManager(game);
            }
        }

        protected void AddGameComponent(GameComponent component)
        {
            lock (gameComponents)
            {
                if (component != null && !(component is GuiBase) && !gameComponents.Contains(component))
                {
                    component.Initialize();
                    gameComponents.Add(component);
                    collision2DManager.AddComponent(component);
                }
            }
        }

        protected void RemoveGameComponent(GameComponent component)
        {
            lock (gameComponents)
            {
                if (component != null && !(component is GuiBase) && gameComponents.Contains(component))
                {
                    collision2DManager.RemoveComponent(component);
                    gameComponents.Remove(component);
                    if (!(component is IPersistentComponent))
                        component.Dispose();
                }
            }
        }

        protected void AddDrawableGameComponent(DrawableGameComponent component)
        {
            lock (drawableGameComponents)
            {
                if (component != null && !(component is GuiBase) && !drawableGameComponents.Contains(component))
                {
                    component.Initialize();
                    component.CallMethod("LoadContent", null);
                    collision2DManager.AddComponent(component);
                    drawableGameComponents.Add(component);
                }
            }
        }

        protected void RemoveDrawableGameComponent(DrawableGameComponent component)
        {
            lock (drawableGameComponents)
            {
                if (component != null && !(component is GuiBase) && drawableGameComponents.Contains(component))
                {
                    collision2DManager.RemoveComponent(component);
                    drawableGameComponents.Remove(component);
                    if (!(component is IPersistentComponent))
                    {
                        component.CallMethod("UnloadContent", null);
                        component.Dispose();
                    }
                }
            }
        }

        protected void AddGuiComponent(GuiBase gui)
        {
            if (gui != null && !(gui is Cursor) && !guiComponents.Exists(x => string.Equals(x.Name, gui.Name)))
            {
                gui.Initialize();
                gui.CallMethod("LoadContent", null);
                guiComponents.Add(gui);
            }
        }

        protected void RemoveGuiComponent(GuiBase gui)
        {
            if (gui != null && !(gui is Cursor) && guiComponents.Exists(x => string.Equals(x.Name, gui.Name)))
            {
                guiComponents.Remove(gui);
                gui.CallMethod("UnloadContent", null);
                gui.Dispose();
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

            //attach physics events
            collision2DManager.Collide += On2DObjectCollide;
            collision2DManager.InCollision += On2DObjectInCollision;
            collision2DManager.OutCollision += On2DObjectOutCollision;
            gravityManager.Falling += OnFallingComponent;
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

            //detach physics events
            collision2DManager.Collide -= On2DObjectCollide;
            collision2DManager.InCollision -= On2DObjectInCollision;
            collision2DManager.OutCollision -= On2DObjectOutCollision;
            gravityManager.Falling -= OnFallingComponent;
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

        #region physics events

        protected virtual void On2DObjectCollide(object sender, Collide2DEventArgs e)
        {

        }

        protected virtual void On2DObjectInCollision(object sender, Collide2DEventArgs e)
        {

        }

        protected virtual void On2DObjectOutCollision(object sender, Collide2DEventArgs e)
        {

        }

        protected virtual void OnFallingComponent(object sender, GravityEventArgs e)
        {

        }

        #endregion

        #region component implementations

        public override void Initialize()
        {
            //attach scene event handlers
            AttachSceneEventHandlers();

            //add game component to scene
            gameComponents.Add(soundManager);
            gameComponents.Add(keyboardManager);
            gameComponents.Add(mouseManager);
            gameComponents.Add(collision2DManager);

            //dispatch scene initialized event
            sceneManager.SceneInitializedEventDispatcher(new SceneEventArgs(this.GetType()));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //dispatch scene loaded event
            sceneManager.SceneLoadedEventDispatcher(new SceneEventArgs(this.GetType()));

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            //unload current scene contents
            cursorContent.Unload();
            SceneContent.Unload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentCursor != null && !currentCursor.IsDisposed && IsCursorVisible)
                currentCursor.Update(gameTime); //make sure update cursor before all components

            //update all game component
            for (int i = 0; i < gameComponents.Count; i ++)
            {
                if (gameComponents[i].Enabled && (!isPaused || (isPaused && gameComponents[i] is INonPausableComponent)))
                    gameComponents[i].Update(gameTime);
            }

            //update all drawable component
            for (int i = 0; i < drawableGameComponents.Count; i++)
            {
                if (drawableGameComponents[i].Enabled && (!isPaused || (isPaused && drawableGameComponents[i] is INonPausableComponent)))
                    drawableGameComponents[i].Update(gameTime);
            }

            if (EnableGui)
            {
                //update all GuiBase component
                for (int i = 0; i < guiComponents.Count; i++)
                {
                    if (guiComponents[i].Enabled && (!isPaused || (isPaused && guiComponents[i] is INonPausableComponent)))
                        guiComponents[i].Update(gameTime);
                }
            }

            //dispatch scene updated event
            sceneManager.SceneUpdatedEventDispatcher(new SceneEventArgs(this.GetType()));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //draw all drawable game component
            for (int i = 0; i < drawableGameComponents.Count; i++)
            {
                if (drawableGameComponents[i].Enabled && (!isPaused || (isPaused && drawableGameComponents[i] is INonPausableComponent)))
                    drawableGameComponents[i].Draw(gameTime);
            }

            if (EnableGui)
            {
                //draw all GuiBase component
                for (int i = 0; i < guiComponents.Count; i++)
                {
                    if (guiComponents[i].Enabled && (!isPaused || (isPaused && guiComponents[i] is INonPausableComponent)))
                        guiComponents[i].Draw(gameTime);
                }
            }

            if (currentCursor != null && !currentCursor.IsDisposed && IsCursorVisible)
                currentCursor.Draw(gameTime);    //make sure draw cursor at top of all components

            //dispatch scene drawed event
            sceneManager.SceneDrawedEventDispatcher(new SceneEventArgs(this.GetType()));

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
                    collision2DManager.ClearComponents();

                    //disposing all game component
                    lock (gameComponents)
                    {
                        for (int i = 0; i < gameComponents.Count; i++)
                        {
                            if (exitGameRequest || !(gameComponents[i] is IPersistentComponent))
                                gameComponents[i].Dispose();
                        }
                        gameComponents.Clear();
                    }

                    //disposing all drawable game component
                    lock (drawableGameComponents)
                    {
                        for (int i = 0; i < drawableGameComponents.Count; i++)
                        {
                            if (exitGameRequest || !(drawableGameComponents[i] is IPersistentComponent))
                                drawableGameComponents[i].Dispose();
                        }
                        drawableGameComponents.Clear();
                    }

                    //disposing all GuiBase component
                    lock (guiComponents)
                    {
                        for (int i = 0; i < guiComponents.Count; i++)
                        {
                            if (exitGameRequest || !(guiComponents[i] is IPersistentComponent))
                                guiComponents[i].Dispose();
                        }
                        guiComponents.Clear();
                    }

                    if (currentCursor != null && !currentCursor.IsDisposed && IsCursorVisible)
                        currentCursor.Dispose();    //dispose cursor

                    //dispatch unloaded event
                    sceneManager.SceneUnloadedEventDispatcher(new SceneEventArgs(this.GetType()));
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