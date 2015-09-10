using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;
using BIOXFramework.Services;

namespace BIOXFramework.Input
{
    public sealed class KeyboardManager : GameComponent, IBIOXFrameworkService
    {
        #region vars

        public event EventHandler<KeyboardPressedEventArgs> Pressed;
        public event EventHandler<KeyboardPressingEventArgs> Pressing;
        public event EventHandler<KeyboardReleasedEventArgs> Released;
        public bool EnableCapture;
        public int PressingDelay
        {
            get { return _pressingDelay; }
            set { _pressingDelay = value > 0 ? value : 1000; }  //default 1 seconds of delay
        }

        private List<KeyboardMap> _maps;
        private int _pressingDelay;
        private KeyboardState _oldKeyboardState;

        #endregion

        #region constructors

        public KeyboardManager(Game game)
            : base (game)
        {
            _maps = new List<KeyboardMap>();
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
                Parallel.ForEach(Enum.GetValues(typeof(Keys)).OfType<Keys>().ToList(), x =>
                {
                    _maps.Add(new KeyboardMap { Name = x.ToString(), Key = x });
                });
            }
        }

        public void Register(string mapName, Keys? key)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            if (_maps.FirstOrDefault(x => string.Equals(x.Name, mapName)) != null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is already registered!", mapName));

            if (_maps.FirstOrDefault(x => string.Equals(x.Key, key)) != null)
                throw new KeyboardManagerException(string.Format("the key \"{0}\" is already registered in \"{1}\" map!", key.ToString(), mapName));

            lock (_maps) { _maps.Add(new KeyboardMap { Name = mapName, Key = key }); }
        }

        public void Unregister(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            lock (_maps) { _maps.Remove(map); }
        }

        public void UpdateMap(string mapName, Keys? key)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            lock (_maps) { map.Key = key; }
        }

        public Keys? GetMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            return map.Key;
        }

        public List<KeyboardMap> GetMaps()
        {
            List<KeyboardMap> maps = new List<KeyboardMap>();
            Parallel.ForEach(_maps, x => maps.Add(new KeyboardMap { Name = x.Name, Key = x.Key }));
            return maps;
        }

        public override void Update(GameTime gameTime)
        {
            if (!EnableCapture)
                return; //ignoring keyboard input events

            lock (_maps)
            {
                //get current keyboard state
                KeyboardState currentKeyboardState = Keyboard.GetState();

                //init old keyboard state
                if (_oldKeyboardState == null)
                    _oldKeyboardState = currentKeyboardState;

                //check key pressed, pressing and released events for each mapped keys
                Parallel.ForEach(_maps, map =>
                {
                    //avoid async operation that cause null value
                    if (map == null)
                        return;

                    if (map.Key == null)
                        return; //key is not setted, so skip

                    if (_oldKeyboardState.IsKeyUp(map.Key.Value) && currentKeyboardState.IsKeyDown(map.Key.Value))
                    {
                        //key is pressed for first time
                        map.PressedTime = DateTime.Now;
                        KeyboardPressedEventDispatcher(new KeyboardPressedEventArgs(map.Name, map.Key.Value));
                        return;
                    }

                    if (currentKeyboardState.IsKeyDown(map.Key.Value))
                    {
                        //key is continous pressing
                        DateTime currentTime = DateTime.Now;
                        if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                        {
                            //delay pressing time is over then pressing delay
                            map.PressedTime = currentTime;
                            KeyboardPressingEventDispatcher(new KeyboardPressingEventArgs(map.Name, map.Key.Value));
                        }
                        return;
                    }

                    if (_oldKeyboardState.IsKeyDown(map.Key.Value) && currentKeyboardState.IsKeyUp(map.Key.Value))
                    {
                        //key is released
                        KeyboardReleasedEventDispatcher(new KeyboardReleasedEventArgs(map.Name, map.Key.Value));
                    }
                });

                //update old keyboard state
                _oldKeyboardState = currentKeyboardState;
            }

            base.Update(gameTime);
        }

        #endregion

        #region dispatchers

        private void KeyboardPressedEventDispatcher(KeyboardPressedEventArgs e)
        {
            var h = Pressed;
            if (h != null)
                h(this, e);
        }

        private void KeyboardPressingEventDispatcher(KeyboardPressingEventArgs e)
        {
            var h = Pressing;
            if (h != null)
                h(this, e);
        }

        private void KeyboardReleasedEventDispatcher(KeyboardReleasedEventArgs e)
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
    }
}