using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BIOXFramework.Services;
using BIOXFramework.Audio;
using BIOXFramework.Input;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Test
{
    public class GameTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SoundManager soundManager;
        private SongManager songManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;

        public GameTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //load bioxframework services
            soundManager = ServiceManager.Get<SoundManager>();
            songManager = ServiceManager.Get<SongManager>();
            keyboardManager = ServiceManager.Get<KeyboardManager>();
            mouseManager = ServiceManager.Get<MouseManager>();

            //register sound and song for this scene
            /*songManager.Register("musica", "musica_1.wav");
            soundManager.Register("suono_1", "suono_1.wav");
            soundManager.Register("suono_2", "suono_2.wav");*/

            //register song events
            songManager.Played += (o, e) =>
            {

            };
            songManager.Paused += (o, e) =>
            {

            };
            songManager.Resumed += (o, e) =>
            {

            };
            songManager.Stopped += (o, e) =>
            {

            };

            //register sound events
            soundManager.Played += (o, e) =>
            {

            };
            soundManager.Paused += (o, e) =>
            {

            };
            soundManager.Resumed += (o, e) =>
            {

            };
            soundManager.Stopped += (o, e) =>
            {

            };

            //register keyboard events
            keyboardManager.Pressed += (o, e) => 
            {
                //need first song and sound resources
                /*switch (e.Key)
                {
                    //test song
                    case Keys.A:
                        songManager.Play("musica");
                        break;
                    case Keys.B:
                        songManager.Pause();
                        break;
                    case Keys.C:
                        songManager.Stop();
                        break;
                    //test sound
                    case Keys.Z:
                        soundManager.Play("suono_1", false);
                        break;
                    case Keys.X:
                         soundManager.Play("suono_2", false);
                        break;
                }*/
            };
            keyboardManager.Pressing += (o, e) => 
            { 
                
            };
            keyboardManager.Released += (o, e) => 
            {

            };

            //register mouse events
            mouseManager.Pressed += (o, e) =>
            {

            };
            mouseManager.Pressing += (o, e) =>
            {

            };
            mouseManager.Released += (o, e) =>
            {

            };
            mouseManager.WhellUp += (o, e) =>
            {

            };
            mouseManager.WhellDown += (o, e) =>
            {

            };

            //add managers to game
            this.Components.Add(songManager);
            this.Components.Add(soundManager);
            this.Components.Add(keyboardManager);
            this.Components.Add(mouseManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);  
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
