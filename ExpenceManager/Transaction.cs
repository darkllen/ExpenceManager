using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class Transaction
    {
        private double amount { get; set; }
        private string currency { get; set; }
        private Category category { get; set; }
        private string description { get; set; }
        private DateTime date_time { get; set; }
        private string file { get; set; }

        public Transaction (
            double amount,
            string currency,
            Category category, 
            string description,
            DateTime date_time,
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
