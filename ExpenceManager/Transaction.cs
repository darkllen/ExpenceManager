using System;
using System.Text.Json.Serialization;
using DataStorage;

namespace ExpenceManager
{
    public class Transaction : IStorable
    {
        public Guid Guid { get; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public Transaction (
            decimal amount,
            string currency,
            Category category, 
            string description,
            DateTime dateTime)
        {
            Guid = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            Category = category;
            Description = description;
            DateTime = dateTime;
        }

        [JsonConstructor]
        public Transaction(
            decimal amount,
            string currency,
            Category category,
            string description,
            DateTime dateTime,
            Guid guid)
        {
            Guid = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            Category = category;
            Description = description;
            DateTime = dateTime;
            Guid = guid;
        }


        public Transaction(string currency)
        {
            Guid = Guid.NewGuid();
            Currency = currency;
            DateTime = DateTime.Now;
        }

        protected bool Equals(Transaction other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Transaction) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
