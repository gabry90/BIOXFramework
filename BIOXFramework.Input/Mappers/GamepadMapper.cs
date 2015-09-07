using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    internal sealed class GamepadMapper : IDisposable
    {
        private Dictionary<string, Buttons> _map;

        public GamepadMapper()
        {
            _map = new Dictionary<string, Buttons>();
        }

        public void AddMap(string keyName, Buttons keyValue)
        {
            if (!string.IsNullOrEmpty(keyName) && !_map.ContainsKey(keyName))
                _map.Add(keyName, keyValue);
        }

        public void UpdateMap(string keyName, Buttons keyValue)
        {
            if (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName))
                _map[keyName] = keyValue;
        }

        public void RemoveMap(string keyName)
        {
            if (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName))
                _map.Remove(keyName);
        }

        public Buttons? GetMap(string keyName)
        {
            return (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName)) ? (Buttons?)_map[keyName] : null;
        }

        public void Dispose()
        {
            _map.Clear();
        }
    }
}