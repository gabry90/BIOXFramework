using System;

namespace BIOXFramework.Settings
{
    public class SettingManagerException : Exception
    {
        public SettingManagerException(string message)
            : base(string.Format("[BIOXFramework.Settings.SettingsManager Exception]: {0}", message))
        {

        }
    }
}