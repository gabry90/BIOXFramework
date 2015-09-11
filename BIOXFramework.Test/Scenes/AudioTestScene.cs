using System;
using Microsoft.Xna.Framework;
using BIOXFramework.Scene;

namespace BIOXFramework.Test.Scenes
{
    public class AudioTestScene : BIOXScene
    {
        public AudioTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Audio Test Scene";
        }

        protected override void OnKeyPressed(object sender, Input.Events.KeyboardPressedEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Left)
                SceneManager.Load<InputTestScene>(game);

            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Right)
                SceneManager.Load<GuiTestScene>(game);

            base.OnKeyPressed(sender, e);
        }

        protected override void OnSongPlayed(object sender, Audio.SongPlayedEventArgs e)
        {
           base.OnSongPlayed(sender, e);
        }

        protected override void OnSongPaused(object sender, Audio.SongPausedEventArgs e)
        {
            base.OnSongPaused(sender, e);
        }

        protected override void OnSongResumed(object sender, Audio.SongResumedEventArgs e)
        {
            base.OnSongResumed(sender, e);
        }

        protected override void OnSongStopped(object sender, Audio.SongStoppedEventArgs e)
        {
            base.OnSongStopped(sender, e);
        }

        protected override void OnSoundPlayed(object sender, Audio.SoundPlayedEventArgs e)
        {
            base.OnSoundPlayed(sender, e);
        }

        protected override void OnSoundPaused(object sender, Audio.SoundPausedEventArgs e)
        {
            base.OnSoundPaused(sender, e);
        }

        protected override void OnSoundResumed(object sender, Audio.SoundResumedEventArgs e)
        {
            base.OnSoundResumed(sender, e);
        }

        protected override void OnSoundStopped(object sender, Audio.SoundStoppedEventArgs e)
        {
            base.OnSoundStopped(sender, e);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Red);
            base.Draw(gameTime);
        }
    }
}