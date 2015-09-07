using System;

namespace BIOXFramework.Audio
{
    #region songs

    public class SongNotRegisteredException : Exception
    {
        public SongNotRegisteredException(string songName) : base(string.Format("[BIOXFramework.Audio.SongManager Exception]: \"{0}\" song not registered!", songName)) { }
    }

    public class SongAlreadyRegistered : Exception
    {
        public SongAlreadyRegistered(string songName) : base(string.Format("[BIOXFramework.Audio.SongManager Exception]: \"{0}\" song already registered!", songName)) { }
    }

    public class SongNameException : Exception
    {
        public SongNameException() : base("[BIOXFramework.Audio.SongManager Exception]: song name empty or null!") { }
    }

    public class SongPathException: Exception
    {
        public SongPathException(string filePath) : base(string.Format("[BIOXFramework.Audio.SongManager Exception]: \"{0}\" file path not exist or invalid!", filePath)) { }
    }

    public class SongLoadException: Exception
    {
        public SongLoadException(string inner) : base(string.Concat("[BIOXFramework.Audio.SongManager Exception]: cannot load song file: ", inner)) { }
    }

    #endregion

    #region sounds

    public class SoundtNotRegisteredException : Exception
    {
        public SoundtNotRegisteredException(string effectName) : base(string.Format("[BIOXFramework.Audio.SoundManager Exception]: \"{0}\" sound not registered!", effectName)) { }
    }

    public class SoundAlreadyRegistered : Exception
    {
        public SoundAlreadyRegistered(string effectName) : base(string.Format("[BIOXFramework.Audio.SoundManager Exception]: \"{0}\" sound already registered!", effectName)) { }
    }

    public class SoundNameException : Exception
    {
        public SoundNameException() : base("[BIOXFramework.Audio.SoundManager Exception]: sound name empty or null!") { }
    }

    public class SoundPathException : Exception
    {
        public SoundPathException(string filePath) : base(string.Format("[BIOXFramework.Audio.SoundManager Exception]: \"{0}\" file path not exist or invalid!", filePath)) { }
    }

    public class SoundLoadException : Exception
    {
        public SoundLoadException(string inner) : base(string.Concat("[BIOXFramework.Audio.SoundManager Exception]: cannot load sound file: ", inner)) { }
    }

    #endregion
}