using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Utility
{
    #region texture region class

    public class AnimatedTextureRegion
    {
        public AnimatedTextureRegion(
            string name, 
            int row, 
            int columns, 
            int frameWidth, 
            int frameHeight)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new UtilityException("region name cannot be null or empty!");

            Name = name;
            Row = row < 0 ? 0 : row;
            Columns = columns < 0 ? 0 : columns;
            FrameHeight = frameHeight < 0 ? 0 : frameHeight;
            FrameWidth = frameWidth < 0 ? 0 : frameWidth;
        }

        public string Name { get; private set; }
        public int Row { get; private set; }
        public int Columns { get; private set; }
        public int FrameHeight { get; private set; }
        public int FrameWidth { get; private set; }
    }

    #endregion

    public class AnimatedTexture : DrawableGameComponent
    {
        #region vars

        public bool AutoAnimated = false;
        public int AnimationSpeed = 100;
        public Vector2 Position = Vector2.Zero;

        private SpriteBatch spriteBatch;
        private List<AnimatedTextureRegion> regions;
        private Texture2D texture;
        private AnimatedTextureRegion currentRegion;
        private int currentFrame = 0;
        private Rectangle destRect;
        private Rectangle sourceRect;
        private DateTime oldProcessFrameDate;

        #endregion

        #region constructors

        public AnimatedTexture(Game game, Texture2D texture, List<AnimatedTextureRegion> regions)
            : base(game)
        {
            if (regions == null || regions.Count == 0)
                throw new UtilityException("regions list cannot be null or empty!");

            if (texture == null)
                throw new UtilityException("texture cannot be null!");

            this.spriteBatch = game.Services.GetService<SpriteBatch>();
            this.texture = texture;
            this.regions = regions;

            SetRegion(regions.FirstOrDefault().Name); //set first region with default
        }

        #endregion

        #region public methods

        public void SetRegion(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
                throw new UtilityException("region name cannot be null or empty!");

            if (currentRegion == null || !string.Equals(currentRegion.Name, regionName))
            {
                currentRegion = regions.FirstOrDefault(x => string.Equals(x.Name, regionName));
                currentFrame = 0;
            }
        }

        public int GetCurrentFrame()
        {
            return currentFrame == 0 ? 1 : currentFrame;
        }

        public void IncrementFrame()
        {
            if (AutoAnimated || currentRegion == null)
                return;

            currentFrame++;
            if (currentFrame >= currentRegion.Columns)
                currentFrame = 0;
        }

        public void DecrementFrame()
        {
            if (AutoAnimated || currentRegion == null)
                return;

            currentFrame--;
            if (currentFrame < 0)
                currentFrame = currentRegion.Columns - 1;
        }

        public void SetFrame(int frame)
        {
            if (AutoAnimated || currentRegion == null)
                return;

            if (frame <= 1)
                currentFrame = 0;
            else if (frame >= currentRegion.Columns)
                currentFrame = currentRegion.Columns - 1;
            else
                currentFrame = frame;
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            if (currentRegion == null)
                return;

            if (AutoAnimated)
            {
                DateTime currentFrameProcessDate = DateTime.Now;

                if (oldProcessFrameDate == null)
                    oldProcessFrameDate = oldProcessFrameDate.Subtract(TimeSpan.FromMilliseconds(AnimationSpeed));

                if (currentFrameProcessDate.Subtract(oldProcessFrameDate).TotalMilliseconds >= AnimationSpeed)
                {
                    oldProcessFrameDate = currentFrameProcessDate;

                    currentFrame++;
                    if (currentFrame >= currentRegion.Columns)
                        currentFrame = 0;
                }
            }

            destRect = new Rectangle((int)Position.X, (int)Position.Y, currentRegion.FrameWidth, currentRegion.FrameHeight);
            sourceRect = new Rectangle(currentRegion.FrameWidth * currentFrame, currentRegion.FrameHeight * (currentRegion.Row - 1), currentRegion.FrameWidth, currentRegion.FrameHeight);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
            spriteBatch.End();

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
                    texture.Dispose();
                    regions.Clear();
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