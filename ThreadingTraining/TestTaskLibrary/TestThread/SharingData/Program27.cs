using System;
using System.Threading.Tasks;

//An Immutable Bank Account
namespace TestThread
{
   //Immutability solves the shared data problem by not allowing data to be changed. If data can’t be
    //changed, there is no scope for a data race.
    class Program27
    {
        class ImmutableBankAccount
        {
            public const int AccountNumber = 123456;
            public readonly int Balance;
            public ImmutableBankAccount(int InitialBalance)
            {
                Balance = InitialBalance;
            }
            public ImmutableBankAccount()
            {
                Balance = 0;
            }
        }
        static void Main27(string[] args)
        {
            // create a bank account with the default balance
            ImmutableBankAccount bankAccount1 = new ImmutableBankAccount();
            Console.WriteLine("Account Number: {0}, Account Balance: {1}",
            ImmutableBankAccount.AccountNumber, bankAccount1.Balance);
            // create a bank account with a starting balance
            ImmutableBankAccount bankAccount2 = new ImmutableBankAccount(200);
            Console.WriteLine("Account Number: {0}, Account Balance: {1}",
            ImmutableBankAccount.AccountNumber, bankAccount2.Balance);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
