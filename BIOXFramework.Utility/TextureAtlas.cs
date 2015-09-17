using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Utility
{
    public enum TextureAtlasRegionOrientation
    {
        Left,
        Right
    }

    public sealed class TextureAtlasRegion
    {
        public string Name;
        public List<int> Frames;
        public TextureAtlasRegionOrientation Orientation; 
    }

    public sealed class TextureAtlas : DrawableGameComponent
    {
        #region vars

        public int CurrentFrame 
        {
            get { return _currentFrame < 1 ? 1 : _currentFrame; }
            set
            {
                if (AutoAnimated)
                    return;

                if (value < 1)
                    _currentFrame = _totalFrame - 1;
                else if (value >= _totalFrame)
                    _currentFrame = 1;
                else
                    _currentFrame = value;
            }
        }

        public bool UseRegions = false;
        public Vector2 Position = Vector2.Zero;
        public bool AutoAnimated = false;
        public int AnimationSpeed = 1000;

        private Texture2D _texture;
        private int _columns;
        private int _rows;
        private int _totalFrame;
        private Rectangle _destRect;
        private Rectangle _sourceRect;
        private SpriteBatch _spriteBatch;
        private int _currentFrame;
        private DateTime _oldFrameProcessdate;
        private List<TextureAtlasRegion> _regions;
        private TextureAtlasRegion _currentRegion;

        #endregion

        #region constructors

        public TextureAtlas(Game game, Texture2D texture, int columns, int rows)
            : base(game)
        {
            InitComponent(game, texture, columns, rows, null);
        }

        public TextureAtlas(Game game, Texture2D texture, int columns, int rows, List<TextureAtlasRegion> regions)
            : base(game)
        {
            InitComponent(game, texture, columns, rows, regions);
        }

        #endregion

        #region public methods

        public void SetRegion(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
                return;

            TextureAtlasRegion region = _regions.FirstOrDefault(x => string.Equals(x.Name, regionName));
            if (region != null)
                _currentRegion = region;
        }

        #endregion

        #region private methods

        private void InitComponent(Game game, Texture2D texture, int columns, int rows, List<TextureAtlasRegion> regions)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            _texture = texture;
            _columns = columns;
            _rows = rows;
            _totalFrame = rows * columns;
            _currentFrame = 0;
            _regions = regions;
        }

        private void UpdateTextureWithRegions()
        {

        }

        private void UpdateTextureWithoutRegions()
        {

        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            if (UseRegions)
                UpdateTextureWithRegions();
            else
                UpdateTextureWithoutRegions();

            if (AutoAnimated)
            {
                DateTime currentFrameProcessDate = DateTime.Now;

                if (_oldFrameProcessdate == null)
                    _oldFrameProcessdate = _oldFrameProcessdate.Subtract(TimeSpan.FromMilliseconds(AnimationSpeed));

                if (currentFrameProcessDate.Subtract(_oldFrameProcessdate).TotalMilliseconds >= AnimationSpeed)
                {
                    _oldFrameProcessdate = currentFrameProcessDate;

                    _currentFrame++;
                    if (_currentFrame >= _totalFrame)
                        _currentFrame = 1;
                }
            }

            int width = _texture.Width / _columns;
            int height = _texture.Height / _rows;
            int row = (int)((float)_currentFrame / (float)_columns);
            int column = _currentFrame % _columns;
            column = _currentFrame <= _columns && column == 1 ? 0 : column;

            _destRect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            _sourceRect = new Rectangle(width * column, height * row, width, height);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _destRect, _sourceRect, Color.White);
            _spriteBatch.End();

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
                    _texture.Dispose();
                    _regions.Clear();
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