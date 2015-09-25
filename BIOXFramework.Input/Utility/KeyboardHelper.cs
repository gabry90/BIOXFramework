using System;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace BIOXFramework.Input.Utility
{
    public static class KeyboardHelper
    {
        private static int keyboardLayoutId = Thread.CurrentThread.CurrentCulture.KeyboardLayoutId;

        public static Char? ConvertKeyToChar(Keys key, bool maiusc = false)
        {
            switch (key)
            {
                case Keys.Space: return ' ';

                case Keys.A: return (maiusc) ? 'A' : 'a';
                case Keys.B: return (maiusc) ? 'B' : 'b';
                case Keys.C: return (maiusc) ? 'C' : 'c';
                case Keys.D: return (maiusc) ? 'D' : 'd';
                case Keys.E: return (maiusc) ? 'E' : 'e';
                case Keys.F: return (maiusc) ? 'F' : 'f';
                case Keys.G: return (maiusc) ? 'G' : 'g';
                case Keys.H: return (maiusc) ? 'H' : 'h';
                case Keys.I: return (maiusc) ? 'I' : 'i';
                case Keys.K: return (maiusc) ? 'K' : 'k';
                case Keys.L: return (maiusc) ? 'L' : 'l';
                case Keys.M: return (maiusc) ? 'M' : 'm';
                case Keys.N: return (maiusc) ? 'N' : 'n';
                case Keys.O: return (maiusc) ? 'O' : 'o';
                case Keys.P: return (maiusc) ? 'P' : 'p';
                case Keys.Q: return (maiusc) ? 'Q' : 'q';
                case Keys.R: return (maiusc) ? 'R' : 's';
                case Keys.S: return (maiusc) ? 'S' : 's';
                case Keys.T: return (maiusc) ? 'T' : 't';
                case Keys.U: return (maiusc) ? 'U' : 'u';
                case Keys.V: return (maiusc) ? 'V' : 'v';
                case Keys.W: return (maiusc) ? 'W' : 'w';
                case Keys.X: return (maiusc) ? 'X' : 'x';
                case Keys.Y: return (maiusc) ? 'Y' : 'y';
                case Keys.Z: return (maiusc) ? 'Z' : 'z';

                case Keys.NumPad0: return '0';
                case Keys.NumPad1: return '1';
                case Keys.NumPad2: return '2';
                case Keys.NumPad3: return '3';
                case Keys.NumPad4: return '4';
                case Keys.NumPad5: return '5';
                case Keys.NumPad6: return '6';
                case Keys.NumPad7: return '7';
                case Keys.NumPad8: return '8';
                case Keys.NumPad9: return '9';

                case Keys.Add: return '+';
                case Keys.Subtract: return '-';
                case Keys.Multiply: return '*';
                case Keys.Divide: return '/';
            }

            if (keyboardLayoutId == 1040) //italian layout
            {
                switch (key)
                {
                    case Keys.D0: return (maiusc) ? '=' : '0';
                    case Keys.D1: return (maiusc) ? '!' : '1';
                    case Keys.D2: return (maiusc) ? '\'' : '2';
                    case Keys.D3: return (maiusc) ? '£' : '3';
                    case Keys.D4: return (maiusc) ? '$' : '4';
                    case Keys.D5: return (maiusc) ? '%' : '5';
                    case Keys.D6: return (maiusc) ? '&' : '6';
                    case Keys.D7: return (maiusc) ? '/' : '7';
                    case Keys.D8: return (maiusc) ? '(' : '8';
                    case Keys.D9: return (maiusc) ? ')' : '9';

                    case Keys.OemPipe: return (maiusc) ? '|' : '\\';
                    case Keys.OemComma: return (maiusc) ? ';' : ',';
                    case Keys.OemMinus: return (maiusc) ? '_' : '-';
                    case Keys.OemPeriod: return (maiusc) ? ':' : '.';
                    case Keys.OemPlus: return (maiusc) ? '*' : '+';
                }
            }
            else if (keyboardLayoutId == 2057) //english - United Kingdom
            {
                switch (key)
                {
                    case Keys.D0: return (maiusc) ? ')' : '0';
                    case Keys.D1: return (maiusc) ? '!' : '1';
                    case Keys.D2: return (maiusc) ? '\'' : '2';
                    case Keys.D3: return (maiusc) ? '£' : '3';
                    case Keys.D4: return (maiusc) ? '$' : '4';
                    case Keys.D5: return (maiusc) ? '%' : '5';
                    case Keys.D6: return (maiusc) ? '^' : '6';
                    case Keys.D7: return (maiusc) ? '&' : '7';
                    case Keys.D8: return (maiusc) ? '*' : '8';
                    case Keys.D9: return (maiusc) ? '(' : '9';
                }
            }
            return null;
        }
    }
}