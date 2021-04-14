using System;

namespace ExpenceManagerModels.Users
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
