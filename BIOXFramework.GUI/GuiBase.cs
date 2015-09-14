using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;

namespace BIOXFramework.GUI
{
    public class GuiBase : DrawableGameComponent
    {
        #region vars

        public EventHandler Click;
        public EventHandler MouseEnter;
        public EventHandler MouseLeave;

        public Texture2D Texture;
        public Vector2 Position;

        internal static MouseManager mouseManager;

        protected SpriteBatch spriteBatch;
        protected Rectangle rectangle;
        protected SpriteFont font;
        protected Game game;

        #endregion

        #region constructors

        public GuiBase(Game game)
            : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            InitializeServices();
        }

        #endregion

        #region public methods



        #endregion

        #region private / protected methods

        protected virtual void InitializeServices()
        {
            if (mouseManager == null)
            {
                mouseManager = game.Services.GetService<MouseManager>();
                if (mouseManager == null)
                    mouseManager = new MouseManager(game);
            }
        }

        protected virtual void AttachEvents()
        {
            Click += OnClick;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;

            mouseManager.Pressed += OnMousePressed;
            mouseManager.PositionChanged += OnMousePositionChanged;
        }

        protected virtual void DetachEvents()
        {
            Click -= OnClick;
            MouseEnter -= OnMouseEnter;
            MouseLeave -= OnMouseLeave;

            mouseManager.Pressed -= OnMousePressed;
            mouseManager.PositionChanged -= OnMousePositionChanged;
        }

        protected Rectangle GetRectangle()
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

        }

        protected virtual void OnMousePositionChanged(object sender, MousePositionChangedEventArgs e)
        {

        }

        protected virtual void OnClick(object sender, EventArgs e)
        {

        }

        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {

        }

        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {

        }

        #endregion

        #region game implementation

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion

        #region dispatchers

        protected void ClickEventDispatcher(object sender, EventArgs e)
        {
            var h = Click;
            if (h != null)
                h(this, e);
        }

        protected void MouseEnterEventDispatcher(object sender, EventArgs e)
        {
            var h = MouseEnter;
            if (h != null)
                h(this, e);
        }

        protected void MouseLeaveEventDispatcher(object sender, EventArgs e)
        {
            var h = MouseLeave;
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
                    DetachEvents();

                    if (Click != null) Click = null;
                    if (MouseEnter != null) MouseEnter = null;
                    if (MouseLeave != null) MouseLeave = null;
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