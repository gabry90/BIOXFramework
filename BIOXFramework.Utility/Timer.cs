using System;
using Microsoft.Xna.Framework;

namespace BIOXFramework.Utility
{
    public sealed class Timer : GameComponent
    {
        #region vars

        public event EventHandler Tick;
        public event EventHandler Stopped;

        public int Interval
        {
            get { return _interval; }
            set { _interval = value < 0 ? 1000 : value; }
        }

        private int _interval;
        private DateTime currentDate;
        private DateTime oldDate;

        #endregion

        #region constructors

        public Timer(Game game)
            : base(game)
        {
            Enabled = false;
            _interval = 1000;
        }

        public Timer(Game game, int interval)
            : base(game)
        {
            Enabled = false;
            _interval = interval < 0 ? 1000 : interval;
        }

        #endregion

        #region public methods

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
            StoppedEventDispatcher(EventArgs.Empty);
        }

        #endregion

        #region game implementations

        public override void Initialize()
        {
            DateTime now = DateTime.Now;
            currentDate = now;
            oldDate = now;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            currentDate = DateTime.Now;
            if (currentDate.Subtract(oldDate).TotalMilliseconds >= _interval)
            {
                oldDate = currentDate;
                TickEventDispatcher(EventArgs.Empty);
            }
            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void TickEventDispatcher(EventArgs e)
        {
            var h = Tick;
            if (h != null)
                h(this, e);
        }

        private void StoppedEventDispatcher(EventArgs e)
        {
            var h = Stopped;
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
                    Stop();
                    if (Tick != null) Tick = null;
                    if (Stopped != null) Stopped = null;
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