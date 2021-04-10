using System;

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
