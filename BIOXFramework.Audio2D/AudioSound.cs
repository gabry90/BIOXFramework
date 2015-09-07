using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BIOXFramework.Audio
{
    internal sealed class AudioSound : IDisposable
    {
        #region vars

        public SoundEffect Sound;
        public SoundEffectInstance SoundInstance;
        public string Name;

        #endregion

        #region constructors

        public AudioSound(string name, string filePath)
        {
            try
            {
                Sound = SoundEffect.FromStream(File.OpenRead(filePath));
                SoundInstance = Sound.CreateInstance();
                SoundInstance.Volume = 0f;
                SoundInstance.Pan = 0f;
                SoundInstance.Pitch = 0f;
            }
            catch (Exception ex) { throw new SoundLoadException(ex.Message); }
            Name = name;
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            if (SoundInstance != null)
            {
                if (SoundInstance.State != SoundState.Stopped)
                    SoundInstance.Stop();
                SoundInstance.Dispose();
            }
            if (Sound != null)
                Sound.Dispose();
        }

        #endregion
    }
}