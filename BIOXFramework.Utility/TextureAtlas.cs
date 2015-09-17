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
            _currentFrame = 0;
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
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

                    Console.WriteLine("current frame: " + _currentFrame);
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