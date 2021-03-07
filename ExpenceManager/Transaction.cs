using System;

namespace ExpenceManager
{
    public class Transaction
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
        public Category category { get; set; }
        public string description { get; set; }
        public DateTime date_time { get; set; }
        public string file { get; set; }

        public Transaction (
            decimal amount,
            string currency,
            Category category, 
            string description,
            DateTime date_time,
            //path to file
            string file)
        {
            this.amount = amount;
            this.currency = currency;
            this.category = category;
            this.description = description;
            this.date_time = date_time;
            this.file = file;
        }
    }
}
