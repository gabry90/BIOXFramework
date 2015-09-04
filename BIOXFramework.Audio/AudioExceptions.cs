using System;

namespace BIOXFramework.Audio
{
    #region song

    public class SongNotRegisteredException : Exception
    {
        public SongNotRegisteredException() : base("[BIOXFramework.Audio.Song Exception]: not registered!") { }
    }

    public class SongAlreadyRegistered : Exception
    {
        public SongAlreadyRegistered() : base("[BIOXFramework.Audio.Song Exception]: already registered!") { }
    }

    public class SongNameException : Exception
    {
        public SongNameException() : base("[BIOXFramework.Audio.Song Exception]: name empty or null!") { }
    }

    public class SongPathException: Exception
    {
        public SongPathException() : base("[BIOXFramework.Audio.Song Exception]: file path not exist or invalid!") { }
    }

    public class SongLoadException: Exception
    {
        public SongLoadException(string inner) : base(string.Concat("[BIOXFramework.Audio.Song Exception]: cannot load song file: ", inner)) { }
    }

    #endregion

    #region effect

    public class EffectNotRegisteredException : Exception
    {
        public EffectNotRegisteredException() : base("[BIOXFramework.Audio.Effect Exception]: not registered!") { }
    }

    public class EffectAlreadyRegistered : Exception
    {
        public EffectAlreadyRegistered() : base("[BIOXFramework.Audio.Effect Exception]: already registered!") { }
    }

    #endregion

}