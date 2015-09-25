using System;

namespace BIOXFramework.GUI
{
    public class TextChangedEventArgs : EventArgs
    {
        public TextChangedEventArgs(string previusText, string newText)
        {
            PreviusText = previusText;
            NewText = newText;
        }

        public string PreviusText { get; private set; }
        public string NewText { get; private set; }
    }
}