using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BIOXFramework.Utility
{
    public sealed class TextureAtlas : DrawableGameComponent
    {
        #region vars

        public int CurrentFrame 
        {
            get { return _currentFrame; }
            set
            {
                if (AutoAnimated)
                    return;

                if (value < 0)
                    _currentFrame = 0;
                else if (value > _totalFrame)
                    _currentFrame = _totalFrame;
                else
                    _currentFrame = value;
            }
        }

        public Vector2 Position;
        public bool AutoAnimated = false;

        private Texture2D _texture;
        private int _columns;
        private int _rows;
        private int _totalFrame;
        private Rectangle _sourceRect;
        private Rectangle _destRect;
        private SpriteBatch _spriteBatch;
        private int _currentFrame;

        #endregion

        #region constructors

        public TextureAtlas(Game game, Texture2D texture, int columns, int rows)
            : base(game)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            _texture = texture;
            _columns = columns;
            _rows = rows;
            _totalFrame = rows * columns;
            _sourceRect = Rectangle.Empty;
            _destRect = Rectangle.Empty;
            _currentFrame = 0;
        }

        #endregion

        #region game implementations

        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            if (AutoAnimated)
                _currentFrame = _currentFrame > _totalFrame ? 0 : _currentFrame + 1;

            int width = _texture.Width / _columns;
            int height = _texture.Height / _rows;
            int row = (int)((float)_currentFrame / (float)_columns);
            int column = _currentFrame % _columns;

            _destRect = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            _sourceRect = new Rectangle(width * column, height * row, width, height);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

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
                    _texture.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}