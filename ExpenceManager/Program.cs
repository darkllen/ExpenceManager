using System;

namespace ExpenceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("S", "s", "s");

            Wallet wallet = Wallet.create_wallet_for_user("s", 23, "s", "s", user);


            Console.WriteLine("Hello World!");
        }
    }
}
