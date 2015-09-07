using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Mappers
{
    internal sealed class KeyboardMapper : IDisposable
    {
        private Dictionary<string, Keys> _map;

        public KeyboardMapper()
        {
            _map = new Dictionary<string, Keys>();
        }

        public void AddMap(string keyName, Keys keyValue)
        {
            if (!string.IsNullOrEmpty(keyName) && !_map.ContainsKey(keyName))
                _map.Add(keyName, keyValue);
        }

        public void UpdateMap(string keyName, Keys keyValue)
        {
            if (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName))
                _map[keyName] = keyValue;
        }

        public void RemoveMap(string keyName)
        {
            if (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName))
                _map.Remove(keyName);
        }

        public Keys? GetMap(string keyName)
        {
            return (!string.IsNullOrEmpty(keyName) && _map.ContainsKey(keyName)) ? (Keys?)_map[keyName] : null;
        }

        public void Dispose()
        {
            _map.Clear();
        }
    }
}