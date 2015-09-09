using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using BIOXFramework.Services;

namespace BIOXFramework.Audio
{
    public sealed class SoundManager : GameComponent, IBIOXFrameworkService
    {
        #region vars

        public event EventHandler<SoundPlayedEventArgs> Played;
        public event EventHandler<SoundPausedEventArgs> Paused;
        public event EventHandler<SoundResumedEventArgs> Resumed;
        public event EventHandler<SoundStoppedEventArgs> Stopped;
        public AudioListener Sound3DListener { get { return _listener; } }

        private List<AudioSound> _sounds;
        private AudioListener _listener;
        private AudioEmitter _emitter;

        #endregion

        #region constructors

        public SoundManager(Game game)
            : base(game)
        {
            _sounds = new List<AudioSound>();
            _listener = new AudioListener();
            _emitter = new AudioEmitter();
        }

        #endregion

        #region public methods

        public void Register(string soundName, string filePath)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            if (!File.Exists(filePath))
                throw new SoundManagerException(string.Format("the sound \"{0}\" path is not exists or invalid!", filePath));

            if (_sounds.FirstOrDefault(x => string.Equals(x.Name, soundName)) != null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is already registered!", soundName));

            _sounds.Add(new AudioSound(soundName, filePath));
        }

        public void Unregister(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is already registered!", soundName));

            sound.Dispose();
            _sounds.Remove(sound);         
        }

        public void ClearRegisteredSounds()
        {
            _sounds.ForEach(x => x.Dispose());
            _sounds.Clear();
        }

        public void Play(string soundName, bool loop)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            switch (sound.SoundInstance.State)
            {
                case SoundState.Playing: return;
                case SoundState.Stopped:
                    sound.Initialize();
                    sound.SoundInstance.IsLooped = loop;
                    sound.SoundInstance.Play();
                    SoundPlayedEventDispatcher(new SoundPlayedEventArgs(soundName));
                    break;
                case SoundState.Paused:
                    sound.SoundInstance.Resume();
                    SoundResumedEventDispatcher(new SoundResumedEventArgs(soundName));
                    break;
            } 
        }

        public void Play3D(string soundName, bool loop, ISound3DEmitter emitter)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            switch (sound.SoundInstance.State)
            {
                case SoundState.Playing: return;
                case SoundState.Stopped:
                    sound.Initialize();
                    sound.Emitter = emitter;
                    sound.SoundInstance.IsLooped = loop;
                    sound.SoundInstance.Play();
                    SoundPlayedEventDispatcher(new SoundPlayedEventArgs(soundName));
                    break;
                case SoundState.Paused:
                    sound.SoundInstance.Resume();
                    SoundResumedEventDispatcher(new SoundResumedEventArgs(soundName));
                    break;
            } 
        }

        public void Pause(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            if (sound.SoundInstance.State == SoundState.Playing)
            {
                sound.SoundInstance.Pause();
                SoundPausedEventDispatcher(new SoundPausedEventArgs(soundName));
            }
        }

        public void Stop(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            if (sound.SoundInstance.State != SoundState.Stopped)
            {
                sound.SoundInstance.Stop();
                sound.SoundInstance.Dispose();
                SoundStoppedEventDispatcher(new SoundStoppedEventArgs(soundName));
            }
        }

        public void SetVolume(string soundName, float volume)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            if (!sound.SoundInstance.IsDisposed)
                sound.SoundInstance.Volume = volume < 0f || volume > 1f ? 0.5f : volume;
        }

        public void SetPan(string soundName, float pan)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            if (!sound.SoundInstance.IsDisposed)
                sound.SoundInstance.Pan = pan < 0f || pan > 1f ? 0.5f : pan;
        }

        public void SetPitch(string soundName, float pitch)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            if (!sound.SoundInstance.IsDisposed)
                sound.SoundInstance.Pitch = pitch < 0f || pitch > 1f ? 0.5f : pitch;
        }

        public float GetVolume(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            return !sound.SoundInstance.IsDisposed ? sound.SoundInstance.Volume : 0f;
        }

        public float GetPan(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            return !sound.SoundInstance.IsDisposed ? sound.SoundInstance.Pan : 0f;
        }

        public float GetPitch(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            return !sound.SoundInstance.IsDisposed ? sound.SoundInstance.Pitch : 0f;
        }

        public bool IsLooped(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundManagerException("the sound name is null or empty");

            AudioSound sound = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (sound == null)
                throw new SoundManagerException(string.Format("the sound \"{0}\" is not registered!", soundName));

            return !sound.SoundInstance.IsDisposed ? sound.SoundInstance.IsLooped : false;
        }

        public override void Update(GameTime gameTime)
        {
            Parallel.ForEach(_sounds, x =>
            {
                if (x.Emitter == null)
                    return; //sound is not 3D

                if (x.SoundInstance == null || x.SoundInstance.IsDisposed || x.SoundInstance.State == SoundState.Stopped)
                    return; //sound is not playing or is disposed

                _emitter.Position = x.Emitter.SoundEmitterPosition;
                _emitter.Forward = x.Emitter.SoundEmitterForward;
                _emitter.Up = x.Emitter.SoundEmitterUp;
                _emitter.Velocity = x.Emitter.SoundEmitterVelocity;

                x.SoundInstance.Apply3D(_listener, _emitter);
            });

            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void SoundPlayedEventDispatcher(SoundPlayedEventArgs e)
        {
            var h = Played;
            if (h != null)
                h(this, e);
        }

        private void SoundPausedEventDispatcher(SoundPausedEventArgs e)
        {
            var h = Paused;
            if (h != null)
                h(this, e);
        }

        private void SoundResumedEventDispatcher(SoundResumedEventArgs e)
        {
            var h = Resumed;
            if (h != null)
                h(this, e);
        }

        private void SoundStoppedEventDispatcher(SoundStoppedEventArgs e)
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
                    ClearRegisteredSounds();
                    if (Played != null) Played = null;
                    if (Paused != null) Paused = null;
                    if (Resumed != null) Resumed = null;
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