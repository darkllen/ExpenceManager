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
            public void TestCreationWallet()
            {
                var user = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                Assert.IsType<int>(id);
            }
        }

        public class TestGetters
        {
            /// <summary>
            /// test getting own wallets ids
            /// </summary>
            [Fact]
            public void TestGetWalletsIds()
            {
                var user = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                var ids = user.GetWalletsIds();
                Assert.Contains(id, ids);
            }

            /// <summary>
            /// test getting shared wallets ids
            /// </summary>
            [Fact]
            public void TestGetSharedWalletsIds()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");

                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                user.ShareWalletWithUser(id, user2);

                var ids = user2.GetSharedWalletsIds();

                Assert.Contains(id, ids);
            }
        }

        public class TestSharing
        {
            /// <summary>
            /// trying to share wrong wallet
            /// </summary>
            [Fact]
            public void TestSharingWrongWallet()
            {
                var wrongId = -1;
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");

                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(wrongId, user2));
            }
            
            /// <summary>
            /// trying to share wallet with self
            /// </summary>
            [Fact]
            public void TestSharingWithSelf()
            {
                var user = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(id, user));
            }

            /// <summary>
            /// trying share wallet twice to one user
            /// </summary>
            [Fact]
            public void TestSharingTwice()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                user.ShareWalletWithUser(id, user2);

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(id, user2));
            }
        }

        public class TestCategoriesPermissiions
        {
            /// <summary>
            /// trying to change permisiion on wrong category
            /// </summary>
            [Fact]
            public void TestSwitchPermissionWrongCategory()
            {
                var user = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.SwitchCategoryPermisiion(id, new Category("s", "s", "s")));
            }

            /// <summary>
            /// trying to change permisiion on category of wrong wallet
            /// </summary>
            [Fact]
            public void TestSwitchPermissionCategoryWrongWallet()
            {
                var wrongId = -1;
                var user = new User("user", "user", "mail");
                user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.SwitchCategoryPermisiion(wrongId, new Category("s", "s", "s")));
            }

            /// <summary>
            /// changing category permission test
            /// </summary>
            [Fact]
            public void TestSwitchPermissionCategory()
            {
                var user = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);

                user.SwitchCategoryPermisiion(id, category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<WalletException>(() => user.AddTransaction(id, transaction));

                user.SwitchCategoryPermisiion(id, category);
                user.AddTransaction(id, new Transaction(23, "USD", category, "S", DateTime.Today, "S"));
            }
        }


        public class TestWorkingWithTransactions
        {
            /// <summary>
            /// trying to add transaction to a wrong wallet
            /// </summary>
            [Fact]
            public void TestAddTransactionWrongWallet()
            {
                var wrongId = -1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<UserException>(() => user.AddTransaction(wrongId, transaction));
            }

            /// <summary>
            /// trying to add two same transactions to one wallet
            /// </summary>
            [Fact]
            public void TestAddTwoSameTransactions()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                Assert.Throws<WalletException>(() => user.AddTransaction(id, transaction));
            }

            /// <summary>
            /// trying to create transaction with wrong category
            /// </summary>
            [Fact]
            public void TestImpossibleCategoryTransactions()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                user.ShareWalletWithUser(id, user2);
                Category category = new Category("s", "s", "s");
                user2.Categories.Add(category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<WalletException>(() => user.AddTransaction(id, transaction));
            }

            /// <summary>
            /// trying to remove transaction of wrong wallet
            /// </summary>
            [Fact]
            public void TestRemoveTransactionWrongWallet()
            {
                var wrongId = -1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                Assert.Throws<UserException>(() => user.RemoveTransaction(wrongId, transaction));
            }

            /// <summary>
            /// test removing transaction from wallet
            /// </summary>
            [Fact]
            public void TestRemoveTransaction()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                user.RemoveTransaction(id, transaction);
            }


            /// <summary>
            /// trying to get transactions from a wrong wallet
            /// </summary>
            [Fact]
            public void TestGetTransactionsWrongWallet()
            {
                var wrongId = -1;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.Get10Transactions(wrongId, start));
            }

            /// <summary>
            /// test getting transactions from start
            /// </summary>
            [Fact]
            public void TestGetTransactions()
            {
                var expected = 1;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                var got = user.Get10Transactions(id, start);
                Assert.Contains(transaction, got);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting transactions not from start
            /// </summary>
            [Fact]
            public void TestGetTransactionsNotFromStart()
            {
                var expected = 1;
                var start = 1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                user.AddTransaction(id, transaction2);
                var got = user.Get10Transactions(id, start);
                Assert.Contains(transaction2, got);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting transaction from value higher than transactions List length
            /// </summary>
            [Fact]
            public void TestGetTransactionsHighStart()
            {
                var expected = 0;
                var start = 1;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                var got = user.Get10Transactions(id, start);
                Assert.Equal(expected, got.Count);
            }

            /// <summary>
            /// test getting max possible num of transactions
            /// </summary>
            [Fact]
            public void TestGetTransactionsGet10()
            {
                var expected = 10;
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                for (int i = 0; i < 12; i++)
                {
                    var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                    user.AddTransaction(id, transaction);
                }
                var got = user.Get10Transactions(id, start);
                Assert.Equal(expected, got.Count);
            }
        }

        public class TestGetWalletStats
        {
            /// <summary>
            /// test showing balance of wallet
            /// </summary>
            [Fact]
            public void TestGetWalletBalance()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);

                var balance = user.GetWalletBallance(id);
                Assert.Equal(transaction.Amount, balance);
            }

            /// <summary>
            /// test changing balance after transactions
            /// </summary>
            [Fact]
            public void TestGetWalletBalanceDependsOnTransactions()
            {
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", start, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);

                var balance = user.GetWalletBallance(id);
                Assert.Equal(transaction.Amount, balance);
                user.RemoveTransaction(id, transaction);
                balance = user.GetWalletBallance(id);
                Assert.Equal(start, balance);
            }

            /// <summary>
            /// test getting month profit
            /// </summary>
            [Fact]
            public void TestGetWalletMonthProfit()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today.AddDays(-40), "S");
                var transaction3 = new Transaction(-23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                user.AddTransaction(id, transaction2);
                user.AddTransaction(id, transaction3);

                Assert.Equal(user.GetThisMonthProfit(id), transaction.Amount);
            }

            /// <summary>
            /// trying to get month profit of wrong wallet
            /// </summary>
            [Fact]
            public void TestGetWalletMonthProfitWrongWallet()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);

                Assert.Throws<UserException>(() => user.GetThisMonthProfit(-1));
            }

            /// <summary>
            /// test getting month spends
            /// </summary>
            [Fact]
            public void TestGetWalletMonthSpends()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(-23, "USD", category, "S", DateTime.Today, "S");
                var transaction2 = new Transaction(-23, "USD", category, "S", DateTime.Today.AddDays(-40), "S");
                var transaction3 = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);
                user.AddTransaction(id, transaction2);
                user.AddTransaction(id, transaction3);

                Assert.Equal(user.GetThisMonthSpends(id), transaction.Amount);
            }

            /// <summary>
            /// trying to get month spends of wrong wallet
            /// </summary>
            [Fact]
            public void TestGetWalletMonthSpendsWrongWallet()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today, "S");
                user.AddTransaction(id, transaction);

                Assert.Throws<UserException>(() => user.GetThisMonthSpends(-1));
            }
        }
    }
}
