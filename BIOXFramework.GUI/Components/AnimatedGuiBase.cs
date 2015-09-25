using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BIOXFramework.Utility;

namespace BIOXFramework.GUI.Components
{
    #region enums

    public enum AnimatedGuiEvents
    {
        OnClick,
        OnMouseEnter,
        OnMouseLeave,
        OnFocused,
        OnLostFocus
    }

    #endregion

    #region animation class

    public class AnimatedGuiAnimations
    {
        public AnimatedGuiAnimations(AnimatedGuiEvents ev, string regionName)
        {
            Event = ev;
            RegionName = regionName;
        }

        public AnimatedGuiEvents Event { get; private set; }
        public string RegionName { get; private set; }
    }

    #endregion

    public class AnimatedGuiBase : GuiBase
    {
        #region vars

        protected AnimatedTexture animatedTexture;
        protected List<AnimatedGuiAnimations> animations;

        #endregion

        #region constructors

        public AnimatedGuiBase(Game game, AnimatedTexture animatedTexture, List<AnimatedGuiAnimations> animations, Vector2 position)
            : base(game, animatedTexture.Texture, position)
        {
            this.animatedTexture = animatedTexture;
            this.animatedTexture.Position = position;
        }

        #endregion

        #region public methods

        public void SetAnimations(List<AnimatedGuiAnimations> animations)
        {
            this.animations.Clear();
            this.animations.AddRange(animations);
        }

        public void SetAnimation(AnimatedGuiEvents ev, string regionName)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == ev && x.RegionName == regionName);
            if (animation == null)
                animations.Add(new AnimatedGuiAnimations(ev, regionName));
            else
                animations[animations.IndexOf(animation)] = new AnimatedGuiAnimations(ev, regionName);
        }

        public void Animate(AnimatedGuiEvents ev)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == ev);
            if (animation != null) 
                animatedTexture.SetRegion(animation.RegionName);
        }

        #endregion

        #region overridden base methods

        protected override Rectangle GetRectangle()
        {
           return new Rectangle((int)Position.X, (int)Position.Y, animatedTexture.Rectangle.Width, animatedTexture.Rectangle.Height);
        }

        protected override void OnClick(object sender, EventArgs e)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == AnimatedGuiEvents.OnClick);
            if (animation != null) animatedTexture.SetRegion(animation.RegionName);
            base.OnClick(sender, e);
        }

        protected override void OnMouseEnter(object sender, EventArgs e)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == AnimatedGuiEvents.OnMouseEnter);
            if (animation != null) animatedTexture.SetRegion(animation.RegionName);
            base.OnMouseEnter(sender, e);
        }

        protected override void OnMouseLeave(object sender, EventArgs e)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == AnimatedGuiEvents.OnMouseLeave);
            if (animation != null) animatedTexture.SetRegion(animation.RegionName);
            base.OnMouseLeave(sender, e);
        }

        protected override void OnFocused(object sender, EventArgs e)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == AnimatedGuiEvents.OnFocused);
            if (animation != null) animatedTexture.SetRegion(animation.RegionName);
            base.OnFocused(sender, e);
        }

        protected override void OnLostFocus(object sender, EventArgs e)
        {
            AnimatedGuiAnimations animation = animations.FirstOrDefault(x => x.Event == AnimatedGuiEvents.OnLostFocus);
            if (animation != null) animatedTexture.SetRegion(animation.RegionName);
            base.OnLostFocus(sender, e);
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            animatedTexture.Position = Position;
            animatedTexture.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            animatedTexture.Draw(gameTime);
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
                    animatedTexture.Dispose();
                    animations.Clear();
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