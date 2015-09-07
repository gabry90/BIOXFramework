using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BIOXFramework.Audio
{
    internal sealed class AudioSong : IDisposable
    {
        #region vars

        public Song Song;
        public string Name;

        #endregion

        #region constructors

        public AudioSong(string name, string filePath)
        {
            try { Song = Song.FromUri(name, new Uri(filePath)); }
            catch (Exception ex) { throw new SongManagerException(string.Format("cannot load \"{0}\" song: {1}", name, ex.Message)); }
            Name = name;
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            if (Song != null)
                Song.Dispose();
        }

        #endregion
    }
}