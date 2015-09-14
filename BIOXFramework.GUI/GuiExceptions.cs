using System;

namespace BIOXFramework.GUI
{
    public class GuiException : Exception
    {
        public GuiException(string message) 
            : base(string.Format("[BIOXFramework.GUI Exception]: {0}", message)) 
        { 
        
        }
    }
}