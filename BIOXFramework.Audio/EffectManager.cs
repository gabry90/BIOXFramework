using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BIOXFramework.Audio
{
    public sealed class EffectManager : IDisposable
    {
        #region vars

        public EventHandler<EffectPlayedEventArgs> Played;
        public EventHandler<EffectPausedEventArgs> Paused;
        public EventHandler<EffectStoppedEventArgs> Stopped;

        #endregion

        #region dispatcher

        private void EffectPlayedEventDispatcher(EffectPlayedEventArgs e)
        {
            var h = Played;
            if (h != null)
                h(this, e);
        }

        private void EffectPausedEventDispatcher(EffectPausedEventArgs e)
        {
            var h = Paused;
            if (h != null)
                h(this, e);
        }

        private void EffectStoppedEventDispatcher(EffectStoppedEventArgs e)
        {
            var h = Stopped;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            if (Played != null) Played = null;
            if (Paused != null) Paused = null;
            if (Stopped != null) Stopped = null;
        }

        #endregion
    }
}