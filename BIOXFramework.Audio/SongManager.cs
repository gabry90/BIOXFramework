using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BIOXFramework.Audio
{
    public sealed class SongManager : GameComponent, IDisposable
    {
        #region vars

        public EventHandler<SongPlayedEventArgs> Played;
        public EventHandler<SongPausedEventArgs> Paused;
        public EventHandler<SongStoppedEventArgs> Stopped;

        public string CurrentSong { get; private set; }

        public bool IsMuted 
        { 
            get { return MediaPlayer.IsMuted; }
            set { MediaPlayer.IsMuted = value; }
        }

        public float Volume 
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value < 0f || value > 1f ? 0.5f : value; }
        }

        private Game _game;
        private List<AudioSong> _songs;

        #endregion

        #region constructors

        public SongManager(Game game)
            : base(game)
        {
            _game = game;
            _songs = new List<AudioSong>();
            CurrentSong = null;
        }

        #endregion

        #region public methods

        public void Register(string name, string filePath)
        {
            if (string.IsNullOrEmpty(name))
                throw new SongNameException();

            if (!File.Exists(filePath))
                throw new SongPathException();

            if (_songs.FirstOrDefault(x => string.Equals(x.Name, name)) != null)
                throw new SongAlreadyRegistered();

            _songs.Add(new AudioSong(_game, name, filePath));   
        }

        public void Unregister(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new SongNameException();

            AudioSong song = _songs.FirstOrDefault(x => string.Equals(x.Name, name));
            if (song == null)
                throw new SongNotRegisteredException();

            song.Dispose();
            _songs.Remove(song);         
        }

        public float GetVolume()
        {
            return MediaPlayer.Volume;
        }

        public void ClearRegisteredSong()
        {
            MediaPlayer.Stop();
            _songs.ForEach(x => x.Dispose());
            _songs.Clear();
            CurrentSong = null;
        }

        public void Play(string songName)
        {
            if (string.IsNullOrEmpty(songName))
                throw new SongNameException();

            AudioSong song = _songs.FirstOrDefault(x => string.Equals(x.Name, songName));
            if (song == null)
                throw new SongNotRegisteredException();

            if (string.Equals(CurrentSong, song.Name))
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Playing: return;
                    case MediaState.Paused: 
                        MediaPlayer.Resume(); 
                        break;
                    case MediaState.Stopped:
                        InternalPlay(song);
                        break;;
                }
            }
            else
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Playing:
                    case MediaState.Paused:
                        MediaPlayer.Stop();
                        InternalPlay(song);
                        break;
                    case MediaState.Stopped:
                        InternalPlay(song);
                        break;
                }
            }
        }

        public void Pause()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
                SongPausedEventDispatcher(new SongPausedEventArgs(CurrentSong));
            }
        }

        public void Stop()
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                SongStoppedEventDispatcher(new SongStoppedEventArgs(CurrentSong));
                CurrentSong = null;
            }
        }

        #endregion

        #region private methods

        private void InternalPlay(AudioSong song)
        {
            MediaPlayer.Play(song.Song);
            CurrentSong = song.Name;
            SongPlayedEventDispatcher(new SongPlayedEventArgs(song.Name));
        }

        #endregion

        #region dispatcher

        private void SongPlayedEventDispatcher(SongPlayedEventArgs e)
        {
            var h = Played;
            if (h != null)
                h(this, e);
        }

        private void SongPausedEventDispatcher(SongPausedEventArgs e)
        {
            var h = Paused;
            if (h != null)
                h(this, e);
        }

        private void SongStoppedEventDispatcher(SongStoppedEventArgs e)
        {
            var h = Stopped;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            ClearRegisteredSong();
            if (Played != null) Played = null;
            if (Paused != null) Paused = null;
            if (Stopped != null) Stopped = null;
         }

        #endregion
    }
}