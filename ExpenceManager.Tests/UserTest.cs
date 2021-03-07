using System;
using Xunit;


namespace ExpenceManager.Tests
{

    public class UserTest
    {
        public class TestCreation
        {
            /// <summary>
            /// test wallet creation
            /// </summary>
            [Fact]
            public void test_creation_wallet()
            {
                var user = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
                Assert.IsType<int>(id);
            }
        }

        public class TestGetters
        {
            /// <summary>
            /// test getting own wallets ids
            /// </summary>
            [Fact]
            public void test_get_wallets_ids()
            {
                var user = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
                var ids = user.get_wallets_ids();
                Assert.Contains(id, ids);
            }

            /// <summary>
            /// test getting shared wallets ids
            /// </summary>
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
        }

        public class TestSharing
        {
            /// <summary>
            /// trying to share wrong wallet
            /// </summary>
            [Fact]
            public void test_sharing_wrong_wallet()
            {
                var wrong_id = -1;
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");

                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.share_wallet_with_user(wrong_id, user2));
            }
            
            /// <summary>
            /// trying to share wallet with self
            /// </summary>
            [Fact]
            public void test_sharing_with_self()
            {
                var user = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.share_wallet_with_user(id, user));
            }

            /// <summary>
            /// trying share wallet twice to one user
            /// </summary>
            [Fact]
            public void test_sharing_twice()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
                user.share_wallet_with_user(id, user2);

                Assert.Throws<UserException>(() => user.share_wallet_with_user(id, user2));
            }
        }

        public class TestCategoriesPermissiions
        {
            /// <summary>
            /// trying to change permisiion on wrong category
            /// </summary>
            [Fact]
            public void test_switch_permission_wrong_category()
            {
                var user = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.switch_category_permisiion(id, new Category("s", "s", "s")));
            }

            /// <summary>
            /// trying to change permisiion on category of wrong wallet
            /// </summary>
            [Fact]
            public void test_switch_permission_category_wrong_wallet()
            {
                var wrong_id = -1;
                var user = new User("user", "user", "mail");
                user.create_new_wallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.switch_category_permisiion(wrong_id, new Category("s", "s", "s")));
            }

            /// <summary>
            /// changing category permission test
            /// </summary>
            [Fact]
            public void test_switch_permission_category()
            {
                var user = new User("user", "user", "mail");
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);

                user.switch_category_permisiion(id, category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<WalletException>(() => user.add_transaction(id, transaction));

                user.switch_category_permisiion(id, category);
                user.add_transaction(id, new Transaction(23, "USD", category, "S", DateTime.Today, "S"));
            }
        }


        public class TestWorkingWithTransactions
        {
            /// <summary>
            /// trying to add transaction to a wrong wallet
            /// </summary>
            [Fact]
            public void test_add_transaction_wrong_wallet()
            {
                var wrong_id = -1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<UserException>(() => user.add_transaction(wrong_id, transaction));
            }

            /// <summary>
            /// trying to add two same transactions to one wallet
            /// </summary>
            [Fact]
            public void test_add_two_same_transactions()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);
                Assert.Throws<WalletException>(() => user.add_transaction(id, transaction));
            }

            /// <summary>
            /// trying to create transaction with wrong category
            /// </summary>
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
                Assert.Throws<WalletException>(() => user.add_transaction(id, transaction));
            }

            /// <summary>
            /// trying to remove transaction of wrong wallet
            /// </summary>
            [Fact]
            public void test_remove_transaction_wrong_wallet()
            {
                var wrong_id = -1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<UserException>(() => user.remove_transaction(wrong_id, transaction));
            }

            /// <summary>
            /// test removing transaction from wallet
            /// </summary>
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


            /// <summary>
            /// trying to get transactions from a wrong wallet
            /// </summary>
            [Fact]
            public void test_get_transactions_wrong_wallet()
            {
                var wrong_id = -1;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                user.create_new_wallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.get_10_transactions(wrong_id, start));
            }

            /// <summary>
            /// test getting transactions from start
            /// </summary>
            [Fact]
            public void test_get_transactions()
            {
                var expected = 1;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);
                var got = user.get_10_transactions(id, start);
                Assert.Contains(transaction, got);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting transactions not from start
            /// </summary>
            [Fact]
            public void test_get_transactions_not_from_start()
            {
                var expected = 1;
                var start = 1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);
                user.add_transaction(id, transaction2);
                var got = user.get_10_transactions(id, start);
                Assert.Contains(transaction2, got);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting transaction from value higher than transactions List length
            /// </summary>
            [Fact]
            public void test_get_transactions_high_start()
            {
                var expected = 0;
                var start = 1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);
                var got = user.get_10_transactions(id, start);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting max possible num of transactions
            /// </summary>
            [Fact]
            public void test_get_transactions_get_10()
            {
                var expected = 10;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                for (int i = 0; i < 12; i++)
                {
                    var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                    user.add_transaction(id, transaction);
                }
                var got = user.get_10_transactions(id, start);
                Assert.Equal(expected, got.Count);
            }
        }

        public class TestGetWalletStats
        {
            /// <summary>
            /// test showing balance of wallet
            /// </summary>
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
                Assert.Equal(transaction.amount, balance);
            }

            /// <summary>
            /// test changing balance after transactions
            /// </summary>
            [Fact]
            public void test_get_wallet_balance_depends_on_transactions()
            {
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", start, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);

                var balance = user.get_wallet_ballance(id);
                Assert.Equal(transaction.amount, balance);
                user.remove_transaction(id, transaction);
                balance = user.get_wallet_ballance(id);
                Assert.Equal(start, balance);
            }

            /// <summary>
            /// test getting month profit
            /// </summary>
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

            /// <summary>
            /// trying to get month profit of wrong wallet
            /// </summary>
            [Fact]
            public void test_get_wallet_month_profit_wrong_wallet()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);

                Assert.Throws<UserException>(() => user.get_this_month_profit(-1));
            }

            /// <summary>
            /// test getting month spends
            /// </summary>
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

            /// <summary>
            /// trying to get month spends of wrong wallet
            /// </summary>
            [Fact]
            public void test_get_wallet_month_spends_wrong_wallet()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.categories.Add(category);
                var id = user.create_new_wallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.add_transaction(id, transaction);

                Assert.Throws<UserException>(() => user.get_this_month_spends(-1));
            }
        }
    }
}
