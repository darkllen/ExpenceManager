using System;
using System.Text.Json.Serialization;
using DataStorage;

namespace ExpenceManagerModels.Wallet
{
    public class WalletDb : IStorable
    {
        public WalletDb(string name, decimal startBalance, string description, string currency)
        {
            Guid = Guid.NewGuid();
            Name = name;
            CurrBalance = startBalance;
            Description = description;
            Currency = currency;
        }

        [JsonConstructor]
        public WalletDb(Guid guid, string name, decimal currBalance, string description, string currency)
        {
            Guid = guid;
            Name = name;
            CurrBalance = currBalance;
            Description = description;
            Currency = currency;
        }

        public WalletDb()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; }
        public string Name { get; set; }
        public decimal CurrBalance { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
    }
}
