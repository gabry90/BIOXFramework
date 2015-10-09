using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;

namespace BIOXFramework.GUI.Components
{
    public class GuiBase : DrawableGameComponent
    {
        #region vars

        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Focused;
        public event EventHandler LostFocus;
        public string Name;
        public Texture2D Texture;
        public Vector2 Position = Vector2.Zero;
        public Rectangle Rectangle { get { return GetRectangle(); } }
        public bool IsFocused { get { return focused; } }
        public bool IsDisposed { get; private set; }
        public Cursor CurrentCursor { protected get; set; }

        protected static MouseManager mouseManager;
        protected SpriteBatch spriteBatch;
        protected Game game;

        private bool mouseInner = false;
        private bool focused = false;

        #endregion

        #region constructors

        public GuiBase(Game game, string name)
            : base(game)
        {
            InitGuiBase(game, name);
        }

        public GuiBase(Game game, string name, Texture2D texture, Vector2 position)
            : base(game)
        {
            Texture = texture;
            Position = position;
            InitGuiBase(game, name);
        }

        #endregion

        #region public methods

        public void Focus()
        {
            if (focused)
                return;

            focused = true;
            FocusedEventDispatcher(EventArgs.Empty);
        }

        #endregion

        #region private / protected methods

        private void InitGuiBase(Game game, string name)
        {
            this.game = game;
            Name = name;
            IsDisposed = false;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            InitializeServices();
            AttachGUIEventsHandlers();
        }

        private void InitializeServices()
        {
            if (mouseManager == null)
            {
                mouseManager = game.Services.GetService<MouseManager>();
                if (mouseManager == null)
                    mouseManager = new MouseManager(game);
            }
        }

        protected virtual void AttachGUIEventsHandlers()
        {
            Click += OnClick;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            Focused += OnFocused;
            LostFocus += OnLostFocus;

            mouseManager.Pressed += OnMousePressed;
            mouseManager.PositionChanged += OnMousePositionChanged;
        }

        protected virtual void DetachGUIEventsHandlers()
        {
            Click -= OnClick;
            MouseEnter -= OnMouseEnter;
            MouseLeave -= OnMouseLeave;
            Focused -= OnFocused;
            LostFocus -= OnLostFocus;

            mouseManager.Pressed -= OnMousePressed;
            mouseManager.PositionChanged -= OnMousePositionChanged;
        }

        protected virtual Rectangle GetRectangle()
        {
            if (Texture == null)
                throw new GuiException("texture is null!");

            if (Position == null)
                throw new GuiException("position is null!");

            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        #endregion

        #region events

        protected virtual void OnMousePressed(object sender, MousePressedEventArgs e)
        {
            if (!Enabled || Texture == null)
                return; //avoid null value

            if (mouseInner)
            {
                //gui component is clicked
                ClickEventDispatcher(EventArgs.Empty);
                if (!focused)
                {
                    focused = true;     //gui component is focused
                    FocusedEventDispatcher(EventArgs.Empty);
                }
            }
            else
            {
                if (focused)
                {
                    focused = false;    //gui component lost focus
                    LostFocusEventDispatcher(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {
            if (!Enabled || Texture == null || this is Cursor)
                return; //avoid null value and cursor rendoundant event

            Rectangle textureRect = this.GetRectangle();
            Rectangle cursorRect = CurrentCursor == null ? new Rectangle(e.Position.X, e.Position.Y, 16, 16) : new Rectangle(e.Position.X, e.Position.Y, CurrentCursor.Rectangle.Width, CurrentCursor.Rectangle.Height);

            if (cursorRect.Intersects(textureRect))
            {
                if (!mouseInner)
                {
                    mouseInner = true;  //enter mouse inner gui component
                    MouseEnterEventDispatcher(EventArgs.Empty);
                }
            }
            else
            {
                if (mouseInner)
                {
                    mouseInner = false; //leave mouse outer gui component
                    MouseLeaveEventDispatcher(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnFocused(object sender, EventArgs e)
        {
            //add common logic here...
        }

        protected virtual void OnLostFocus(object sender, EventArgs e)
        {
            //add common logic here...
        }

        #endregion

        #region dispatchers

        protected void ClickEventDispatcher(EventArgs e)
        {
            var h = Click;
            if (h != null)
                h(this, e);
        }

        protected void MouseEnterEventDispatcher(EventArgs e)
        {
            var h = MouseEnter;
            if (h != null)
                h(this, e);
        }

        protected void MouseLeaveEventDispatcher(EventArgs e)
        {
            var h = MouseLeave;
            if (h != null)
                h(this, e);
        }

        protected void FocusedEventDispatcher(EventArgs e)
        {
            var h = Focused;
            if (h != null)
                h(this, e);
        }

        protected void LostFocusEventDispatcher(EventArgs e)
        {
            var h = LostFocus;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    DetachGUIEventsHandlers();

                    if (Texture != null)
                        Texture.Dispose();

                    if (Click != null) Click = null;
                    if (MouseEnter != null) MouseEnter = null;
                    if (MouseLeave != null) MouseLeave = null;
                    if (Focused != null) Focused = null;
                    if (LostFocus != null) LostFocus = null;

                    IsDisposed = true;
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