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
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                Assert.IsType<Wallet>(wallet);
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
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                var wallets = user.Wallets;
                Assert.Contains(wallet, wallets);
            }

            /// <summary>
            /// test getting shared wallets ids
            /// </summary>
            [Fact]
            public void TestGetSharedWalletsIds()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");

                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                user.ShareWalletWithUser(wallet, user2);

                var wallets = user2.WalletsShared;

                Assert.Contains(wallet, wallets);
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
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD"); 


                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(wrongWallet, user2));
            }
            
            /// <summary>
            /// trying to share wallet with self
            /// </summary>
            [Fact]
            public void TestSharingWithSelf()
            {
                var user = new User("user", "user", "mail");
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(wallet, user));
            }

            /// <summary>
            /// trying share wallet twice to one user
            /// </summary>
            [Fact]
            public void TestSharingTwice()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                user.ShareWalletWithUser(wallet, user2);

                Assert.Throws<UserException>(() => user.ShareWalletWithUser(wallet, user2));
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
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                Assert.Throws<UserException>(() => user.SwitchCategoryPermisiion(wallet, new Category("s", "s")));
            }

            /// <summary>
            /// trying to change permisiion on category of wrong wallet
            /// </summary>
            [Fact]
            public void TestSwitchPermissionCategoryWrongWallet()
            {
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD"); 

                var user = new User("user", "user", "mail");

                Assert.Throws<UserException>(() => user.SwitchCategoryPermisiion(wrongWallet, new Category("s", "s")));
            }

            /// <summary>
            /// changing category permission test
            /// </summary>
            [Fact]
            public void TestSwitchPermissionCategory()
            {
                var user = new User("user", "user", "mail");
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");
                Category category = new Category("s", "s");
                user.Categories.Add(category);

                user.SwitchCategoryPermisiion(wallet, category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                Assert.Throws<WalletException>(() => user.AddTransaction(wallet, transaction));

                user.SwitchCategoryPermisiion(wallet, category);
                user.AddTransaction(wallet, new Transaction(23, "USD", category, "S", DateTime.Today));
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
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD");

                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                Assert.Throws<UserException>(() => user.AddTransaction(wrongWallet, transaction));
            }

            /// <summary>
            /// trying to add two same transactions to one wallet
            /// </summary>
            [Fact]
            public void TestAddTwoSameTransactions()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                Assert.Throws<WalletException>(() => user.AddTransaction(wallet, transaction));
            }

            /// <summary>
            /// trying to create transaction with wrong category
            /// </summary>
            [Fact]
            public void TestImpossibleCategoryTransactions()
            {
                var user = new User("user", "user", "mail");
                var user2 = new User("user", "user", "mail");
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                user.ShareWalletWithUser(wallet, user2);
                Category category = new Category("s", "s");
                user2.Categories.Add(category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                Assert.Throws<WalletException>(() => user.AddTransaction(wallet, transaction));
            }

            /// <summary>
            /// trying to remove transaction of wrong wallet
            /// </summary>
            [Fact]
            public void TestRemoveTransactionWrongWallet()
            {

                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD");
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                Assert.Throws<UserException>(() => user.RemoveTransaction(wrongWallet, transaction));
            }

            /// <summary>
            /// test removing transaction from wallet
            /// </summary>
            [Fact]
            public void TestRemoveTransaction()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                user.RemoveTransaction(wallet, transaction);
            }


            /// <summary>
            /// trying to get transactions from a wrong wallet
            /// </summary>
            [Fact]
            public void TestGetTransactionsWrongWallet()
            {
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD");
                var start = 0;
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);

                Assert.Throws<UserException>(() => user.Get10Transactions(wrongWallet, start));
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                var got = user.Get10Transactions(wallet, start);
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                user.AddTransaction(wallet, transaction2);
                var got = user.Get10Transactions(wallet, start);
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                var got = user.Get10Transactions(wallet, start);
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                for (int i = 0; i < 12; i++)
                {
                    var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                    user.AddTransaction(wallet, transaction);
                }
                var got = user.Get10Transactions(wallet, start);
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);

                var balance = user.GetWalletBallance(wallet);
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
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", start, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);

                var balance = user.GetWalletBallance(wallet);
                Assert.Equal(transaction.Amount, balance);
                user.RemoveTransaction(wallet, transaction);
                balance = user.GetWalletBallance(wallet);
                Assert.Equal(start, balance);
            }

            /// <summary>
            /// test getting month profit
            /// </summary>
            [Fact]
            public void TestGetWalletMonthProfit()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                var transaction2 = new Transaction(23, "USD", category, "S", DateTime.Today.AddDays(-40));
                var transaction3 = new Transaction(-23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                user.AddTransaction(wallet, transaction2);
                user.AddTransaction(wallet, transaction3);

                Assert.Equal(user.GetThisMonthProfit(wallet), transaction.Amount);
            }

            /// <summary>
            /// trying to get month profit of wrong wallet
            /// </summary>
            [Fact]
            public void TestGetWalletMonthProfitWrongWallet()
            {
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD");

                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);

                Assert.Throws<UserException>(() => user.GetThisMonthProfit(wrongWallet));
            }

            /// <summary>
            /// test getting month spends
            /// </summary>
            [Fact]
            public void TestGetWalletMonthSpends()
            {
                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var wallet = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(-23, "USD", category, "S", DateTime.Today);
                var transaction2 = new Transaction(-23, "USD", category, "S", DateTime.Today.AddDays(-40));
                var transaction3 = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(wallet, transaction);
                user.AddTransaction(wallet, transaction2);
                user.AddTransaction(wallet, transaction3);

                Assert.Equal(user.GetThisMonthSpends(wallet), transaction.Amount);
            }

            /// <summary>
            /// trying to get month spends of wrong wallet
            /// </summary>
            [Fact]
            public void TestGetWalletMonthSpendsWrongWallet()
            {
                var userWrong = new User("user", "user", "mail");
                var wrongWallet = userWrong.CreateNewWallet("wallet", 0, "wallet", "USD");

                var user = new User("user", "user", "mail");
                Category category = new Category("s", "s");
                user.Categories.Add(category);
                var id = user.CreateNewWallet("wallet", 0, "wallet", "USD");

                var transaction = new Transaction(23, "USD", category, "S", DateTime.Today);
                user.AddTransaction(id, transaction);

                Assert.Throws<UserException>(() => user.GetThisMonthSpends(wrongWallet));
            }
        }
    }
}
