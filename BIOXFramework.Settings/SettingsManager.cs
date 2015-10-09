using System;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using BIOXFramework.Utility.Extensions;

namespace BIOXFramework.Settings
{
    public sealed class SettingsManager : GameComponent, INonPausableComponent, IPersistentComponent
    {
        #region vars

        public event EventHandler Loaded;
        public event EventHandler Saved;
        public string Path;

        private RootSettings _settings;

        #endregion

        #region constructors

        public SettingsManager(Game game)
            : base(game)
        {
            
        }

        #endregion

        #region public methods

        public void Load()
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new SettingManagerException("the setting file path is not setted!");

            if (!File.Exists(Path))
                throw new SettingManagerException(string.Format("the setting path \"{0}\" not exists!", Path));

            lock (_settings)
            {
                string xml = null;

                try { File.ReadAllText(Path, Encoding.UTF8); }
                catch (Exception ex) 
                { 
                    throw new SettingManagerException(string.Format("cannot load settings file: {0}", ex.ToString())); 
                }
                
                _settings = new RootSettings().XmlDeserialize(xml);
                if (_settings == null)
                    throw new SettingManagerException("error during serialization load process!");
            }

            SettingsLoadedEventDispatcher(EventArgs.Empty);
        }

        public void Add(string name, object setting)
        {
            if (_settings == null)
                throw new SettingManagerException("the setting file is not loaded!");

            if (_settings.Settings.FirstOrDefault(x => string.Equals(x.Name, name)) != null)
                throw new SettingManagerException(string.Format("the setting \"{0}\" is already present!", name));

            lock (_settings) { _settings.Settings.Add(new Setting { Name = name, Value = setting }); }
        }

        public void Remove(string name)
        {
            if (_settings == null)
                throw new SettingManagerException("the setting file is not loaded!");

            Setting sett = _settings.Settings.FirstOrDefault(x => string.Equals(x.Name, name));
            if (sett == null)
                throw new SettingManagerException(string.Format("the setting \"{0}\" not exists!", name));

            lock (_settings) { _settings.Settings.Remove(sett); }
        }

        public void Update(string name, object setting)
        {
            if (_settings == null)
                throw new SettingManagerException("the setting file is not loaded!");

            Setting sett = _settings.Settings.FirstOrDefault(x => string.Equals(x.Name, name));
            if (sett == null)
                throw new SettingManagerException(string.Format("the setting \"{0}\" not exists!", name));

            lock (_settings) { sett.Value = setting; }
        }

        public T Get<T>(string name)
        {
            if (_settings == null)
                throw new SettingManagerException("the setting file is not loaded!");

            Setting sett = _settings.Settings.FirstOrDefault(x => string.Equals(x.Name, name));
            if (sett == null)
                throw new SettingManagerException(string.Format("the setting \"{0}\" not exists!", name));

            T value = default(T);
            try { value = (T)sett.Value; }
            catch { value = default(T); }
            return value;
        }

        public void Save()
        {
            if (_settings == null)
                throw new SettingManagerException("the setting file is not loaded!");

            lock (_settings)
            {
                string xml = _settings.XmlSerialize();
                if (string.IsNullOrWhiteSpace(xml))
                    throw new SettingManagerException("error during serialization save process!");

                try { File.WriteAllText(Path, xml, Encoding.UTF8); }
                catch (Exception ex)
                {
                    throw new SettingManagerException(string.Format("cannot save settings file: {0}", ex.ToString()));
                }

                SettingsSavedEventDispatcher(EventArgs.Empty);
            }
        }

        #endregion

        #region dispatchers

        private void SettingsLoadedEventDispatcher(EventArgs e)
        {
            var h = Loaded;
            if (h != null)
                h(this, e);
        }

        private void SettingsSavedEventDispatcher(EventArgs e)
        {
            var h = Saved;
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
                    if (Loaded != null) Loaded = null;
                    if (Saved != null) Saved = null;
                    _settings = null;
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