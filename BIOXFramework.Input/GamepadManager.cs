using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Input
{
    public sealed class GamepadManager : GameComponent, INonPausableComponent, IPersistentComponent
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
            SetDefaultMaps();
            EnableCapture = true;
            _pressingDelay = 1000;
        }

        #endregion

        #region public methods

        public void SetDefaultMaps()
        {
            lock (_maps)
            {
                _maps.Clear();
                foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
                    foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                        _maps.Add(new GamepadMap { Name = button.ToString(), Button = button, Player = player });
            }
        }

        public void Register(string mapName, Buttons? button, PlayerIndex player)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            if (_maps.FirstOrDefault(x => string.Equals(x.Name, mapName)) != null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is already registered!", mapName));

            if (_maps.FirstOrDefault(x => x.Player == player && string.Equals(x.Button, button)) != null)
                throw new GamepadManagerException(string.Format("the button \"{0}\" is already registered in \"{1}\" map for player \"{2}\"!", button.ToString(), mapName, player.ToString()));

            lock (_maps) { _maps.Add(new GamepadMap { Name = mapName, Button = button, Player = player }); }
        }

        public void Unregister(string mapName, PlayerIndex player)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => x.Player == player && string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered for player \"{1}\"!", mapName, player.ToString()));

            lock (_maps) { _maps.Remove(map); }
        }

        public void UpdateMap(string mapName, Buttons? button, PlayerIndex player)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => x.Player == player && string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered for player \"{1}\"!", mapName, player.ToString()));

            lock (_maps) { map.Button = button; }
        }

        public Buttons? GetMap(string mapName, PlayerIndex player)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new GamepadManagerException("map name is null or empty!");

            GamepadMap map = _maps.FirstOrDefault(x => x.Player == player && string.Equals(x.Name, mapName));
            if (map == null)
                throw new GamepadManagerException(string.Format("the map \"{0}\" is not registered for player \"{1}\"!", mapName, player.ToString()));

            return map.Button;
        }

        public List<GamepadMap> GetMaps()
        {
            List<GamepadMap> maps = new List<GamepadMap>();
            foreach (GamepadMap map in _maps) { maps.Add(new GamepadMap { Name = map.Name, Button = map.Button, Player = map.Player }); };
            return maps;
        }

        #endregion

        #region game implementations

        public override void Update(GameTime gameTime)
        {
            if (!EnableCapture)
                return; //ignoring gamepad input events

            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                //get current gamepad state
                GamePadState currentGamepadState = GamePad.GetState(player);

                //init old gamepad state
                if (_oldGamepadState == null)
                    _oldGamepadState = currentGamepadState;

                //check button pressed, pressing and released events for each mapped buttons
                for (int i = 0; i < _maps.Count; i++)
                {
                    GamepadMap map = _maps[i];

                    /* skip if
                        map is null OR
                        button is not setted OR
                        player index is not same then object
                    */
                    if (map == null || map.Button == null || map.Player != player)
                        continue;

                    if (_oldGamepadState.IsButtonUp(map.Button.Value) && currentGamepadState.IsButtonDown(map.Button.Value))
                    {
                        //button is pressed for first time
                        map.PressedTime = DateTime.Now;
                        GamepadPressedEventDispatcher(new GamepadPressedEventArgs(map.Name, map.Button.Value));
                        continue;
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
                        continue;
                    }

                    if (_oldGamepadState.IsButtonDown(map.Button.Value) && currentGamepadState.IsButtonUp(map.Button.Value))
                    {
                        //button is released
                        GamepadReleasedEventDispatcher(new GamepadReleasedEventArgs(map.Name, map.Button.Value));
                    }
                }

                //update old gamepad state
                _oldGamepadState = currentGamepadState;
            }

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
                    lock (_maps) { _maps.Clear(); }
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

        #region interface implementations

        public bool ForcePausableStatus { get; set; }
        public bool ForceDisposing { get; set; }

        #endregion
    }
}