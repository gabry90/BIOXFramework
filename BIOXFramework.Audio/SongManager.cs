using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using BIOXFramework.Services;

namespace BIOXFramework.Audio
{
    public sealed class SongManager : GameComponent, IBIOXFrameworkService
    {
        #region vars

        public event EventHandler<SongPlayedEventArgs> Played;
        public event EventHandler<SongPausedEventArgs> Paused;
        public event EventHandler<SongResumedEventArgs> Resumed;
        public event EventHandler<SongStoppedEventArgs> Stopped;

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

        private List<AudioSong> _songs;

        #endregion

        #region constructors

        public SongManager(Game game)
            : base(game)
        {
            _songs = new List<AudioSong>();
            CurrentSong = null;
        }

        #endregion

        #region public methods

        public void Register(string songName, string filePath)
        {
            if (string.IsNullOrEmpty(songName))
                throw new SongManagerException("the song name is null or empty!");

            if (!File.Exists(filePath))
                throw new SongManagerException(string.Format("the song \"{0}\" path is not exists or invalid!", filePath));

            if (_songs.FirstOrDefault(x => string.Equals(x.Name, songName)) != null)
                throw new SongManagerException(string.Format("the song \"{0}\" is already registered!", songName));

            lock (_songs) { _songs.Add(new AudioSong(songName, filePath)); } 
        }

        public void Unregister(string songName)
        {
            if (string.IsNullOrEmpty(songName))
                throw new SongManagerException("the song name is null or empty!");

            AudioSong song = _songs.FirstOrDefault(x => string.Equals(x.Name, songName));
            if (song == null)
                throw new SongManagerException(string.Format("the song \"{0}\" is not registered!", songName));

            lock (_songs)
            {
                song.Dispose();
                _songs.Remove(song);
            }
        }

        public void ClearRegisteredSongs()
        {
            lock (_songs)
            {
                MediaPlayer.Stop();
                Parallel.ForEach(_songs, x => x.Dispose());
                _songs.Clear();
                CurrentSong = null;
            }
        }

        public void Play(string songName)
        {
            if (string.IsNullOrEmpty(songName))
                throw new SongManagerException("the song name is null or empty!");

            AudioSong song = _songs.FirstOrDefault(x => string.Equals(x.Name, songName));
            if (song == null)
                throw new SongManagerException(string.Format("the song \"{0}\" is not registered!", songName));

            if (string.Equals(CurrentSong, song.Name))
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Playing: return;
                    case MediaState.Paused: 
                        MediaPlayer.Resume();
                        SongResumedEventDispatcher(new SongResumedEventArgs(CurrentSong));
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

        #region dispatchers

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

        private void SongResumedEventDispatcher(SongResumedEventArgs e)
        {
            var h = Resumed;
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    ClearRegisteredSongs();
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