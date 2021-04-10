using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    /// <summary>
    /// class for Wallet exeptions
    /// </summary>
    public class WalletException : Exception
    {
        public WalletException(string message)
            : base(message)
        {
        }
    }
}
