using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;

namespace BIOXFramework.Input
{
    public sealed class KeyboardManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        #region vars

        public event EventHandler<KeyboardPressedEventArgs> Pressed;
        public event EventHandler<KeyboardPressingEventArgs> Pressing;
        public event EventHandler<KeyboardReleasedEventArgs> Released;
        public bool EnableCapture;

        public int PressingFrequency
        {
            get { return pressingFrequency; }
            set { pressingFrequency = value > 0 ? value : 500; } //default 500 milliseconds of frequency
        }

        public int PressingDelay
        {
            get { return pressingDelay; }
            set { pressingDelay = value > 0 ? value : 100; }  //default 100 milliseconds of delay
        }

        private List<KeyboardMap> maps;
        private int pressingDelay;
        private int pressingFrequency;
        private KeyboardState oldKeyboardState;

        #endregion

        #region constructors

        public KeyboardManager(Game game)
            : base (game)
        {
            maps = new List<KeyboardMap>();
            SetDefaultMaps();
            EnableCapture = true;
            pressingDelay = 1000;
            pressingFrequency = 100;
        }

        #endregion

        #region public methods

        public void SetDefaultMaps()
        {
            lock (maps)
            {
                maps.Clear();
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                    maps.Add(new KeyboardMap { Name = key.ToString(), Key = key });
            }
        }

        public void Register(string mapName, Keys? key)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            if (maps.FirstOrDefault(x => string.Equals(x.Name, mapName)) != null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is already registered!", mapName));

            if (maps.FirstOrDefault(x => string.Equals(x.Key, key)) != null)
                throw new KeyboardManagerException(string.Format("the key \"{0}\" is already registered in \"{1}\" map!", key.ToString(), mapName));

            lock (maps) { maps.Add(new KeyboardMap { Name = mapName, Key = key }); }
        }

        public void Unregister(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            lock (maps) { maps.Remove(map); }
        }

        public void UpdateMap(string mapName, Keys? key)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            lock (maps) { map.Key = key; }
        }

        public Keys? GetMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new KeyboardManagerException("map name is null or empty!");

            KeyboardMap map = maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new KeyboardManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            return map.Key;
        }

        public List<KeyboardMap> GetMaps()
        {
            List<KeyboardMap> maps = new List<KeyboardMap>();
            foreach(KeyboardMap map in maps) { maps.Add(new KeyboardMap { Name = map.Name, Key = map.Key }); };
            return maps;
        }

        #endregion

        #region component implementations

        public override void Update(GameTime gameTime)
        {
            if (!EnableCapture)
                return; //ignoring keyboard input events

            //get current keyboard state
            KeyboardState currentKeyboardState = Keyboard.GetState();

            //init old keyboard state
            if (oldKeyboardState == null)
                oldKeyboardState = currentKeyboardState;

            //check key pressed, pressing and released events for each mapped keys
            for (int i = 0; i < maps.Count; i++)
            {
                KeyboardMap map = maps[i];

                /*
                    skip if
                    map is null OR
                    map key is not setted
                */
                if (map == null || map.Key == null)
                    continue;

                if (oldKeyboardState.IsKeyUp(map.Key.Value) && currentKeyboardState.IsKeyDown(map.Key.Value))
                {
                    //key is pressed for first time
                    DateTime current = DateTime.Now;
                    map.LastPressedTime = current;
                    map.LastFrequencyTime = DateTime.MinValue;
                    KeyboardPressedEventDispatcher(new KeyboardPressedEventArgs(map.Name, map.Key.Value));
                    continue;
                }

                if (currentKeyboardState.IsKeyDown(map.Key.Value))
                {
                    //key is continous pressing
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.Subtract(map.LastPressedTime).TotalMilliseconds >= pressingDelay)
                    {
                        if (map.LastFrequencyTime == DateTime.MinValue)
                            map.LastFrequencyTime = currentTime.Subtract(TimeSpan.FromMilliseconds(pressingFrequency));

                        if (currentTime.Subtract(map.LastFrequencyTime).TotalMilliseconds >= pressingFrequency)
                        {
                            map.LastFrequencyTime = currentTime;
                            KeyboardPressingEventDispatcher(new KeyboardPressingEventArgs(map.Name, map.Key.Value));
                        }    
                    }
                    continue;
                }

                if (oldKeyboardState.IsKeyDown(map.Key.Value) && currentKeyboardState.IsKeyUp(map.Key.Value))
                {
                    //key is released
                    KeyboardReleasedEventDispatcher(new KeyboardReleasedEventArgs(map.Name, map.Key.Value));
                }
            }

            //update old keyboard state
            oldKeyboardState = currentKeyboardState;

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
                    lock (maps) { maps.Clear(); }
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