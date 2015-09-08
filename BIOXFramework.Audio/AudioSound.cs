using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace BIOXFramework.Audio
{
    internal sealed class AudioSound : IDisposable
    {
        #region vars

        public SoundEffect Sound;
        public SoundEffectInstance SoundInstance;
        public string Name;
        public ISound3DEmitter Emitter;

        #endregion

        #region constructors

        public AudioSound(string name, string filePath)
        {
            try { Sound = SoundEffect.FromStream(File.OpenRead(filePath)); }
            catch (Exception ex) { throw new SoundManagerException(string.Format("cannot load \"{0}\" sound: {1}", name, ex.Message)); }
            Name = name;
        }

        public void Initialize()
        {
            Emitter = null;

            if (Sound == null)
                return;

            if (SoundInstance != null && !SoundInstance.IsDisposed)
            {
                if (SoundInstance.State != SoundState.Stopped)
                    SoundInstance.Stop();
                SoundInstance.Dispose();
            }

            SoundInstance = Sound.CreateInstance();
            SoundInstance.Volume = 0f;
            SoundInstance.Pan = 0f;
            SoundInstance.Pitch = 0f;
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            if (SoundInstance != null && !SoundInstance.IsDisposed)
            {
                if (SoundInstance.State != SoundState.Stopped)
                    SoundInstance.Stop();
                SoundInstance.Dispose();
            }
            if (Sound != null && !Sound.IsDisposed)
                Sound.Dispose();
            if (Emitter != null)
                Emitter = null;
        }

        #endregion
    }
}