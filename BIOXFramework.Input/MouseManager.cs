using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BIOXFramework.Input.Events;
using BIOXFramework.Input.Mappers;
using Microsoft.Xna.Framework.Input;
using BIOXFramework.Services;

namespace BIOXFramework.Input
{
    public sealed class MouseManager : GameComponent, IBIOXFrameworkService
    {
        #region vars

        public event EventHandler<MousePressedEventArgs> Pressed;
        public event EventHandler<MousePressingEventArgs> Pressing;
        public event EventHandler<MouseReleasedEventArgs> Released;
        public event EventHandler<MousePositionChangedEventArgs> PositionChanged;
        public event EventHandler WhellUp;
        public event EventHandler WhellDown;
        public bool EnableCapture;
        public int PressingDelay
        {
            get { return _pressingDelay; }
            set { _pressingDelay = value > 0 ? value : 1000; }  //default 1 seconds of delay
        }

        private List<MouseMap> _maps;
        private int _pressingDelay;
        private MouseState _oldMouseState;

        #endregion

        #region constructors

        public MouseManager(Game game)
            : base(game)
        {
            _maps = new List<MouseMap>();
            EnableCapture = true;
            _pressingDelay = 1000;
        }

        #endregion

        #region public methods

        public void Register(string mapName, MouseButtons? button)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new MouseManagerException("button name is null or empty!");

            if (_maps.FirstOrDefault(x => string.Equals(x.Name, mapName)) != null)
                throw new MouseManagerException(string.Format("the map \"{0}\" is already registered!", mapName));

            if (_maps.FirstOrDefault(x => string.Equals(x.Button, button)) != null)
                throw new MouseManagerException(string.Format("the button \"{0}\" is already registered in \"{1}\" map!", button.ToString(), mapName));

            _maps.Add(new MouseMap { Name = mapName, Button = button });
        }

        public void Unregister(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new MouseManagerException("map name is null or empty!");

            MouseMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new MouseManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            _maps.Remove(map);
        }

        public void UpdateMap(string mapName, MouseButtons? button)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new MouseManagerException("map name is null or empty!");

            MouseMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new MouseManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            map.Button = button;
        }

        public MouseButtons? GetMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
                throw new MouseManagerException("map name is null or empty!");

            MouseMap map = _maps.FirstOrDefault(x => string.Equals(x.Name, mapName));
            if (map == null)
                throw new MouseManagerException(string.Format("the map \"{0}\" is not registered!", mapName));

            return map.Button;
        }

        public List<MouseMap> GetMaps()
        {
            List<MouseMap> maps = new List<MouseMap>();
            Parallel.ForEach(_maps, x => maps.Add(new MouseMap { Name = x.Name, Button = x.Button }));
            return maps;
        }

        public override void Update(GameTime gameTime)
        {
            if (!EnableCapture)
                return; //ignoring mouse input events

            //get current mouse state
            MouseState currentMouseState = Mouse.GetState();

            //check button pressed, pressing and released events for each mapped buttons
            Parallel.ForEach(_maps, map =>
            {
                if (map.Button == null)
                    return; //button is not setted, so skip

                switch (map.Button.Value)
                {
                    case MouseButtons.Left:     //update mouse left button events
                        UpdateLeftButton(map, _oldMouseState, currentMouseState);
                        return;
                    case MouseButtons.Rigth:    //update mouse right button events
                        UpdateRightButton(map, _oldMouseState, currentMouseState);
                        return;
                    case MouseButtons.Middle:   //update mouse middle button events
                        UpdateMiddleButton(map, _oldMouseState, currentMouseState);
                        return;
                    case MouseButtons.X1:       //update mouse X1 button events
                        UpdateX1Button(map, _oldMouseState, currentMouseState);
                        return;
                    case MouseButtons.X2:       //update mouse X2 button events
                        UpdateX2Button(map, _oldMouseState, currentMouseState);
                        return;
                }
            });

            if (_oldMouseState.Position != currentMouseState.Position)  //mouse position changed
                MousePositionChangedEventDispatcher(new MousePositionChangedEventArgs(currentMouseState.Position));

            if (currentMouseState.ScrollWheelValue > _oldMouseState.ScrollWheelValue)
                MouseWhellUpEventDispatcher(EventArgs.Empty);   //mouse scroll up
            else if (currentMouseState.ScrollWheelValue < _oldMouseState.ScrollWheelValue)
                MouseWhellDownEventDispatcher(EventArgs.Empty); //mouse scroll down

            //update old mouse state
            _oldMouseState = currentMouseState;

            base.Update(gameTime);
        }

        #endregion

        #region private methods

        private void UpdateLeftButton(MouseMap map, MouseState old, MouseState current)
        {
            if (old.LeftButton == ButtonState.Released && current.LeftButton == ButtonState.Pressed)
            {
                map.PressedTime = DateTime.Now;
                MousePressedEventDispatcher(new MousePressedEventArgs(map.Name, map.Button.Value));
                return;
            }
            if (current.LeftButton == ButtonState.Pressed)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                {
                    map.PressedTime = currentTime;
                    MousePressingEventDispatcher(new MousePressingEventArgs(map.Name, map.Button.Value));
                }
                return;
            }
            if (old.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released)
                MouseReleasedEventDispatcher(new MouseReleasedEventArgs(map.Name, map.Button.Value));
        }

        private void UpdateRightButton(MouseMap map, MouseState old, MouseState current)
        {
            if (old.RightButton == ButtonState.Released && current.RightButton == ButtonState.Pressed)
            {
                map.PressedTime = DateTime.Now;
                MousePressedEventDispatcher(new MousePressedEventArgs(map.Name, map.Button.Value));
                return;
            }
            if (current.RightButton == ButtonState.Pressed)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                {
                    map.PressedTime = currentTime;
                    MousePressingEventDispatcher(new MousePressingEventArgs(map.Name, map.Button.Value));
                }
                return;
            }
            if (old.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Released)
                MouseReleasedEventDispatcher(new MouseReleasedEventArgs(map.Name, map.Button.Value));
        }

        private void UpdateMiddleButton(MouseMap map, MouseState old, MouseState current)
        {
            if (old.MiddleButton == ButtonState.Released && current.MiddleButton == ButtonState.Pressed)
            {
                map.PressedTime = DateTime.Now;
                MousePressedEventDispatcher(new MousePressedEventArgs(map.Name, map.Button.Value));
                return;
            }
            if (current.MiddleButton == ButtonState.Pressed)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                {
                    map.PressedTime = currentTime;
                    MousePressingEventDispatcher(new MousePressingEventArgs(map.Name, map.Button.Value));
                }
                return;
            }
            if (old.MiddleButton == ButtonState.Pressed && current.MiddleButton == ButtonState.Released)
                MouseReleasedEventDispatcher(new MouseReleasedEventArgs(map.Name, map.Button.Value));
        }

        private void UpdateX1Button(MouseMap map, MouseState old, MouseState current)
        {
            if (old.XButton1 == ButtonState.Released && current.XButton1 == ButtonState.Pressed)
            {
                map.PressedTime = DateTime.Now;
                MousePressedEventDispatcher(new MousePressedEventArgs(map.Name, map.Button.Value));
                return;
            }
            if (current.XButton1 == ButtonState.Pressed)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                {
                    map.PressedTime = currentTime;
                    MousePressingEventDispatcher(new MousePressingEventArgs(map.Name, map.Button.Value));
                }
                return;
            }
            if (old.XButton1 == ButtonState.Pressed && current.XButton1 == ButtonState.Released)
                MouseReleasedEventDispatcher(new MouseReleasedEventArgs(map.Name, map.Button.Value));
        }

        private void UpdateX2Button(MouseMap map, MouseState old, MouseState current)
        {
            if (old.XButton2 == ButtonState.Released && current.XButton2 == ButtonState.Pressed)
            {
                map.PressedTime = DateTime.Now;
                MousePressedEventDispatcher(new MousePressedEventArgs(map.Name, map.Button.Value));
                return;
            }
            if (current.XButton2 == ButtonState.Pressed)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.Subtract(map.PressedTime).TotalMilliseconds >= _pressingDelay)
                {
                    map.PressedTime = currentTime;
                    MousePressingEventDispatcher(new MousePressingEventArgs(map.Name, map.Button.Value));
                }
                return;
            }
            if (old.XButton2 == ButtonState.Pressed && current.XButton2 == ButtonState.Released)
                MouseReleasedEventDispatcher(new MouseReleasedEventArgs(map.Name, map.Button.Value));
        }

        #endregion

        #region dispatchers

        private void MousePressedEventDispatcher(MousePressedEventArgs e)
        {
            var h = Pressed;
            if (h != null)
                h(this, e);
        }

        private void MousePressingEventDispatcher(MousePressingEventArgs e)
        {
            var h = Pressing;
            if (h != null)
                h(this, e);
        }

        private void MouseReleasedEventDispatcher(MouseReleasedEventArgs e)
        {
            var h = Released;
            if (h != null)
                h(this, e);
        }

        private void MousePositionChangedEventDispatcher(MousePositionChangedEventArgs e)
        {
            var h = PositionChanged;
            if (h != null)
                h(this, e);
        }

        private void MouseWhellUpEventDispatcher(EventArgs e)
        {
            var h = WhellUp;
            if (h != null)
                h(this, e);
        }

        private void MouseWhellDownEventDispatcher(EventArgs e)
        {
            var h = WhellDown;
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
                    if (PositionChanged != null) PositionChanged = null;
                    if (WhellUp != null) WhellUp = null;
                    if (WhellDown != null) WhellDown = null;
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