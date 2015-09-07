using System;

namespace BIOXFramework.Audio
{
    public class SongManagerException : Exception
    {
        public SongManagerException(string message)
            : base(string.Format("[BIOXFramework.Audio.SongManager Exception]: {0}", message)) 
        { 
        
        }
    }

    public class SoundManagerException : Exception
    {
        public SoundManagerException(string message)
            : base (string.Format("[BIOXFramework.Audio.SoundManager Exception]: {0}", message))
        {

        }
    }
}