namespace BankApplicationLibrary
{
    interface ITransaction
    {
        long CheckBalance();
        long WithdrawMoney(long amount);
        long DepositeMoney(long amount);
        long DeleteAccount();
    }
}