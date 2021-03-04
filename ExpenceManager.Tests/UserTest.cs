using System;
using Xunit;


namespace ExpenceManager.Tests
{

    public class UserTest
    {
        [Fact]
        public void test_creation_wallet()
        {
            var user = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
            Assert.IsType<int>(id);
        }

        [Fact]
        public void test_get_wallets_ids()
        {
            var user = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var ids = user.get_wallets_ids();

            Assert.Contains(id, ids);
        }

        [Fact]
        public void test_get_shared_wallets_ids()
        {
            var user = new User("user", "user", "mail");
            var user2 = new User("user", "user", "mail");

            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            user.share_wallet_with_user(id, user2);

            var ids = user2.get_shared_wallets_ids();

            Assert.Contains(id, ids);
        }



        [Fact]
        public void test_sharing_wallet()
        {
            var user = new User("user", "user", "mail");
            var user2 = new User("user", "user", "mail");

            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            user.share_wallet_with_user(id, user2);

            Assert.Contains(id, user2.get_shared_wallets_ids());
        }

        [Fact]
        public void test_sharing_wrong_wallet()
        {
            var wrong_id = -1;
            var user = new User("user", "user", "mail");
            var user2 = new User("user", "user", "mail");

            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(()=> user.share_wallet_with_user(wrong_id, user2));
        }

        [Fact]
        public void test_sharing_with_self()
        {
            var user = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(() => user.share_wallet_with_user(id, user));
        }

        [Fact]
        public void test_sharing_twice()
        {
            var user = new User("user", "user", "mail");
            var user2 = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
            user.share_wallet_with_user(id, user2);

            Assert.Throws<User.UserException>(() => user.share_wallet_with_user(id, user2));
        }

        [Fact]
        public void test_switch_permission_wrong_category()
        {
            var user = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
           
            Assert.Throws<User.UserException>(() => user.switch_category_permisiion(id, new Category("s", "s", "s")));
        }

        [Fact]
        public void test_switch_permission_category_wrong_wallet()
        {
            var user = new User("user", "user", "mail");
            user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(() => user.switch_category_permisiion(-1, new Category("s", "s", "s")));
        }

        [Fact]
        public void test_switch_permission_category()
        {
            var user = new User("user", "user", "mail");
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            user.switch_category_permisiion(id, category);
            Assert.Throws<User.WalletException>(() => user.add_transaction(id, new Transaction(23,"USD", category, "S", DateTime.Today, "S")));
            user.switch_category_permisiion(id, category);
            user.add_transaction(id, new Transaction(23, "USD", category, "S", DateTime.Today, "S"));
        }


        [Fact]
        public void test_add_transaction_wrong_wallet()
        {
            var wrong_id = -1;
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(() => user.add_transaction(wrong_id, new Transaction(23, "USD", category, "S", DateTime.Today, "S")));
        }

        [Fact]
        public void test_add_two_same_transactions()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            Assert.Throws<User.WalletException>(() => user.add_transaction(id, transaction));
        }

        [Fact]
        public void test_impossible_category_transactions()
        {
            var user = new User("user", "user", "mail");
            var user2 = new User("user", "user", "mail");

            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            user.share_wallet_with_user(id, user2);
            Category category = new Category("s", "s", "s");
            user2.categories.Add(category);

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            Assert.Throws<User.WalletException>(() => user.add_transaction(id, transaction));
        }


        [Fact]
        public void test_remove_transaction_wrong_wallet()
        {
            var wrong_id = -1;
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(() => user.remove_transaction(wrong_id, new Transaction(23, "USD", category, "S", DateTime.Today, "S")));
        }

        [Fact]
        public void test_remove_transaction()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            user.remove_transaction(id, transaction);
        }


        [Fact]
        public void test_get_transactions_wrong_wallet()
        {
            var wrong_id = -1;
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            user.create_new_wallet("wallet", 0, "wallet", "USD");

            Assert.Throws<User.UserException>(() => user.get_10_transactions(wrong_id, 0));
        }


        [Fact]
        public void test_get_transactions()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            var got = user.get_10_transactions(id, 0);
            Assert.Contains(transaction, got);
            Assert.True(got.Count == 1);
        }

        [Fact]
        public void test_get_transactions_not_from_start()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            user.add_transaction(id, transaction2);
            var got = user.get_10_transactions(id, 1);
            Assert.Contains(transaction2, got);
            Assert.True(got.Count == 1);
        }

        [Fact]
        public void test_get_transactions_high_start()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            var got = user.get_10_transactions(id, 1);
            Assert.True(got.Count == 0);
        }

        [Fact]
        public void test_get_transactions_get_10()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            for (int i = 0; i< 12; i++)
            {
                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);
            }
            var got = user.get_10_transactions(id, 0);
            Assert.True(got.Count == 10);
        }

        [Fact]
        public void test_get_wallet_balance()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);

            var balance = user.get_wallet_ballance(id);
            Assert.Equal(balance, transaction.amount);
        }

        [Fact]
        public void test_get_wallet_balance_depends_on_transactions()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);

            var balance = user.get_wallet_ballance(id);
            Assert.Equal(balance, transaction.amount);
            user.remove_transaction(id, transaction);
            Assert.Equal(0, user.get_wallet_ballance(id));
        }

        [Fact]
        public void test_get_wallet_month_profit()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today.AddDays(-40), "S");
            var transaction3 = new Transaction(-23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            user.add_transaction(id, transaction2);
            user.add_transaction(id, transaction3);

            Assert.Equal(user.get_this_month_profit(id), transaction.amount);
        }

        [Fact]
        public void test_get_wallet_month_profit_wrong_wallet()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);

            Assert.Throws<User.UserException>(() => user.get_this_month_profit(-1));
        }


        [Fact]
        public void test_get_wallet_month_spends()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(-23, "USD", category, "S", DateTime.Today, "S");
            var transaction2 = new Transaction(-23, "USD", category, "S", DateTime.Today.AddDays(-40), "S");
            var transaction3 = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);
            user.add_transaction(id, transaction2);
            user.add_transaction(id, transaction3);

            Assert.Equal(user.get_this_month_spends(id), transaction.amount);
        }

        [Fact]
        public void test_get_wallet_month_spends_wrong_wallet()
        {
            var user = new User("user", "user", "mail");
            Category category = new Category("s", "s", "s");
            user.categories.Add(category);
            var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

            var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
            user.add_transaction(id, transaction);

            Assert.Throws<User.UserException>(() => user.get_this_month_spends(-1));
        }

    }
}
