﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    public class GuiBase : DrawableGameComponent
    {
        #region vars

        protected static GuiManager guiManager;
        protected static MouseManager mouseManager;

        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Focused;
        public event EventHandler LostFocus;

        public Texture2D Texture;
        public Vector2 Position = Vector2.Zero;

        protected bool mouseInner = false;
        protected bool focused = false;
        protected SpriteBatch spriteBatch;
        protected Game game;

        #endregion

        #region constructors

        public GuiBase(Game game)
            : base(game)
        {
            InitGuiBase(game);
        }

        public GuiBase(Game game, Texture2D texture, Vector2 position)
            : base(game)
        {
            Texture = texture;
            Position = position;
            InitGuiBase(game);
        }

        #endregion

        #region private / protected methods

        private void InitGuiBase(Game game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            InitializeServices();
            AttachGUIEventsHandlers();
        }

        private void InitializeServices()
        {
            if (guiManager == null)
            {
                guiManager = game.Services.GetService<GuiManager>();
                if (guiManager == null)
                    throw new GuiException("the GuiBase required GuiManager game service!");
            }

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
            if (!Enabled || Texture == null)
                return; //avoid null value

            Rectangle textureRect = GetRectangle();
            Rectangle cursorRect = guiManager.CurrentCursor.GetRectangle();
            Rectangle mouseRect = new Rectangle(e.Position.X, e.Position.Y, cursorRect.Size.X, cursorRect.Size.Y);

            if (mouseRect.Intersects(textureRect))
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