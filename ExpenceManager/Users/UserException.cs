using System;

namespace ExpenceManagerModels.Users
{
    /// <summary>
    /// class for User exceptions
    /// </summary>
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}
