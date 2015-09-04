using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BIOXFramework.Audio
{
    internal sealed class AudioSong : IDisposable
    {
        #region vars

        public Song Song;
        public string Name { get; set; }

        #endregion

        #region constructors

        public AudioSong(Game game, string name, string filePath)
        {
            try { Song = game.Content.Load<Song>(filePath); }
            catch (Exception ex) { throw new SongLoadException(ex.Message); }
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