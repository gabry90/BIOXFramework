using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Scene;
using BIOXFramework.Input.Events;
using BIOXFramework.Audio;
using BIOXFramework.GUI.Components;

namespace BIOXFramework.Test.Scenes
{
    public class AudioTestScene : BIOXScene
    {
        public AudioTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "Audio Test Scene";
        }

        protected override void OnKeyPressed(object sender, KeyboardPressedEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Left:
                    sceneManager.Load<InputTestScene>();
                    break;
                case Keys.Right:
                    sceneManager.Load<GuiTestScene>();
                    break;
            }

            base.OnKeyPressed(sender, e);
        }

        protected override void OnSongPlayed(object sender, SongPlayedEventArgs e)
        {
           base.OnSongPlayed(sender, e);
        }

        protected override void OnSongPaused(object sender, SongPausedEventArgs e)
        {
            base.OnSongPaused(sender, e);
        }

        protected override void OnSongResumed(object sender, SongResumedEventArgs e)
        {
            base.OnSongResumed(sender, e);
        }

        protected override void OnSongStopped(object sender, SongStoppedEventArgs e)
        {
            base.OnSongStopped(sender, e);
        }

        protected override void OnSoundPlayed(object sender, SoundPlayedEventArgs e)
        {
            base.OnSoundPlayed(sender, e);
        }

        protected override void OnSoundPaused(object sender, SoundPausedEventArgs e)
        {
            base.OnSoundPaused(sender, e);
        }

        protected override void OnSoundResumed(object sender, SoundResumedEventArgs e)
        {
            base.OnSoundResumed(sender, e);
        }

        protected override void OnSoundStopped(object sender, SoundStoppedEventArgs e)
        {
            base.OnSoundStopped(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Red);
            base.Draw(gameTime);
        }
    }
}