using Tenderfoot.TfSystem.Diagnostics;
using System;

namespace Tenderfoot.TfSystem
{
    public class TfException : Exception
    {
        public TfException(string message)
        {
            TfDebug.WriteLine("System Exception:", message);

            throw new Exception(message);
        }
    }
}
