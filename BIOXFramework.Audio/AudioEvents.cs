using System;

namespace BIOXFramework.Audio
{
    #region song

    public sealed class SongPlayedEventArgs : EventArgs
    {
        public SongPlayedEventArgs(string songName) { Name = songName; }
        public string Name { get; private set; }
    }

    public sealed class SongStoppedEventArgs : EventArgs
    {
        public SongStoppedEventArgs(string songName) { Name = songName; }
        public string Name { get; private set; }
    }

    public sealed class SongPausedEventArgs : EventArgs
    {
        public SongPausedEventArgs(string songName) { Name = songName; }
        public string Name { get; private set; }
    }

    #endregion

    #region effect

    public sealed class EffectPlayedEventArgs : EventArgs
    {

    }

    public sealed class EffectPausedEventArgs : EventArgs
    {

    }

    public sealed class EffectStoppedEventArgs : EventArgs
    {

    }

    #endregion
}