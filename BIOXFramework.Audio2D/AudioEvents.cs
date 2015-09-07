using System;

namespace BIOXFramework.Audio
{
    #region songs

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

    #region sounds

    public sealed class SoundPlayedEventArgs : EventArgs
    {
        public SoundPlayedEventArgs(string effectName) { Name = effectName; }
        public string Name { get; private set; }
    }

    public sealed class SoundPausedEventArgs : EventArgs
    {
        public SoundPausedEventArgs(string effectName) { Name = effectName; }
        public string Name { get; private set; }
    }

    public sealed class SoundStoppedEventArgs : EventArgs
    {
        public SoundStoppedEventArgs(string effectName) { Name = effectName; }
        public string Name { get; private set; }
    }

    #endregion
}