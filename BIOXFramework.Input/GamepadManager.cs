using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Input
{
    public sealed class GamepadManager : GameComponent
    {
        #region vars

        public event EventHandler<GamepadPressedEventArgs> Pressed;
        public event EventHandler<GamepadPressingEventArgs> Pressing;
        public event EventHandler<GamepadReleasedEventArgs> Released;
        public bool EnableCapture;
        public int PressingDelay
        {
            get { return _pressingDelay; }
            set { _pressingDelay = value > 0 ? value : 1000; }  //default 1 seconds of delay
        }

        private List<GamepadMap> _maps;
        private int _pressingDelay;
        private GamePadState _oldGamepadState;

        #endregion

        #region constructors

        public GamepadManager(Game game)
            : base(game)
        {
            _maps = new List<GamepadMap>();
            EnableCapture = true;
            _pressingDelay = 1000;
        }

        #endregion

        #region public methods

        public void Register(string mapName, Buttons? button)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            if (_maps.FirstOrDefault(x => string.Equals(x.Name, mapName)) != null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is already registered!", mapName));

            if (_maps.FirstOrDefault(x => string.Equals(x.Button, button)) != null)
                throw new GamepadManagerException(string.Format("the button \"{0}\" is already registered in \"{1}\" map!", button.ToString(), mapName));

            _maps.Add(new GamepadMap { Name = mapName, Button = button });
        }

        public void Unregister(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            _maps.Remove(map);
        }

        public void UpdateMap(string mapName, Buttons? button)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            map.Button = button;
        }

        public Buttons? GetMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            return map.Button;
        }

        public List<GamepadMap> GetMaps()
        {
            List<GamepadMap> maps = new List<GamepadMap>();
            Parallel.ForEach(_maps, x => maps.Add(new GamepadMap { Name = x.Name, Button = x.Button }));
            return maps;
        }

        public override void Update(GameTime gameTime)
        {
            if (!EnableCapture)
                return; //ignoring gamepad input events

            //get current gamepad state
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            //check button pressed, pressing and released events for each mapped buttons
            Parallel.ForEach(_maps, map =>
            {
                if (map.Button == null)
                    return; //button is not setted, so skip

                if (_oldGamepadState.IsButtonUp(map.Button.Value) && currentGamepadState.IsButtonDown(map.Button.Value))
                {
                    //button is pressed for first time
                    map.PressedTime = DateTime.Now;
                    GamepadPressedEventDispatcher(new GamepadPressedEventArgs(map.Name, map.Button.Value));
                    return;
                }

                if (currentGamepadState.IsButtonDown(map.Button.Value))
                {
                    //button is continous pressing
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                    {
                        //delay pressing time is over then pressing delay
                        map.PressedTime = currentTime;
                        GamepadPressingEventDispatcher(new GamepadPressingEventArgs(map.Name, map.Button.Value));
                    }
                    return;
                }

                if (_oldGamepadState.IsButtonDown(map.Button.Value) && currentGamepadState.IsButtonUp(map.Button.Value))
                {
                    //button is released
                    GamepadReleasedEventDispatcher(new GamepadReleasedEventArgs(map.Name, map.Button.Value));
                }
            });

            //update old gamepad state
            _oldGamepadState = currentGamepadState;

            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void GamepadPressedEventDispatcher(GamepadPressedEventArgs e)
        {
            var h = Pressed;
            if (h != null)
                h(this, e);
        }

        private void GamepadPressingEventDispatcher(GamepadPressingEventArgs e)
        {
            var h = Pressing;
            if (h != null)
                h(this, e);
        }

        private void GamepadReleasedEventDispatcher(GamepadReleasedEventArgs e)
        {
            var h = Released;
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
                    _maps.Clear();
                    if (Pressed != null) Pressed = null;
                    if (Pressing != null) Pressing = null;
                    if (Released != null) Released = null;
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