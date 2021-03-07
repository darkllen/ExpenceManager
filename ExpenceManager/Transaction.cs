using System;

namespace ExpenceManager
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string File { get; set; }

        public Transaction (
            decimal amount,
            string currency,
            Category category, 
            string description,
            DateTime dateTime,
            //path to file
            string file)
        {
            Amount = amount;
            Currency = currency;
            Category = category;
            Description = description;
            DateTime = dateTime;
            File = file;
        }
    }
}
