using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BIOXFramework.Audio
{
    public sealed class SoundManager : GameComponent
    {
        #region vars

        public EventHandler<SoundPlayedEventArgs> Played;
        public EventHandler<SoundPausedEventArgs> Paused;
        public EventHandler<SoundStoppedEventArgs> Stopped;

        private List<AudioSound> _sounds;

        #endregion

        #region constructors

        public SoundManager(Game game) 
            : base(game)
        {
            _sounds = new List<AudioSound>();
        }

        #endregion

        #region public methods

        public void Register(string soundName, string filePath)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            if (!File.Exists(filePath))
                throw new SoundPathException(filePath);

            if (_sounds.FirstOrDefault(x => string.Equals(x.Name, soundName)) != null)
                throw new SoundAlreadyRegistered(soundName);

            _sounds.Add(new AudioSound(soundName, filePath));
        }

        public void Unregister(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            effect.Dispose();
            _sounds.Remove(effect);         
        }

        public void ClearRegisteredSounds()
        {
            _sounds.ForEach(x => x.Dispose());
            _sounds.Clear();
        }

        public void Play(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            switch (effect.SoundInstance.State)
            {
                case SoundState.Stopped:
                    effect.SoundInstance.Play();
                    SoundPlayedEventDispatcher(new SoundPlayedEventArgs(soundName));
                    break;
                case SoundState.Paused:
                    effect.SoundInstance.Resume();
                    SoundPlayedEventDispatcher(new SoundPlayedEventArgs(soundName));
                    break;
            } 
        }

        public void Pause(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            if (effect.SoundInstance.State == SoundState.Playing)
            {
                effect.SoundInstance.Pause();
                SoundPausedEventDispatcher(new SoundPausedEventArgs(soundName));
            }
        }

        public void Stop(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            if (effect.SoundInstance.State != SoundState.Stopped)
            {
                effect.SoundInstance.Stop();
                SoundStoppedEventDispatcher(new SoundStoppedEventArgs(soundName));
            }
        }

        public void SetVolume(string soundName, float volume)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            effect.SoundInstance.Volume = volume < 0f || volume > 1f ? 0.5f : volume;
        }

        public void SetPan(string soundName, float pan)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            effect.SoundInstance.Pan = pan < 0f || pan > 1f ? 0.5f : pan;
        }

        public void SetPitch(string soundName, float pitch)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            effect.SoundInstance.Pitch = pitch < 0f || pitch > 1f ? 0.5f : pitch;
        }

        public float GetVolume(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            return effect.SoundInstance.Volume;
        }

        public float GetPan(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            return effect.SoundInstance.Pan;
        }

        public float GetPitch(string soundName)
        {
            if (string.IsNullOrEmpty(soundName))
                throw new SoundNameException();

            AudioSound effect = _sounds.FirstOrDefault(x => string.Equals(x.Name, soundName));
            if (effect == null)
                throw new SoundtNotRegisteredException(soundName);

            return effect.SoundInstance.Pitch;
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
            ClearRegisteredSounds();
            if (Played != null) Played = null;
            if (Paused != null) Paused = null;
            if (Stopped != null) Stopped = null;
 	        base.Dispose(disposing);
        }

        #endregion
    }
}