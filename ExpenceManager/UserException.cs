using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    /// <summary>
    /// class for User exeptions
    /// </summary>
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}
