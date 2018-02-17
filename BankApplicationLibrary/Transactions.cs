using System;
using MySql.Data.MySqlClient;

namespace BankApplicationLibrary
{
    public class Transactions : ITransaction
    {
        private DatabaseConnectivity _dbconnect;

        private string AccountNumber
        {
            get;
        }

        public Transactions(Customer obj)
        {
            AccountNumber = obj.AccountNumber;
        }

        private void SaveFinalBalance(long balance)
        {
            _dbconnect = new DatabaseConnectivity();
            string query = $@"update @tableName set Balance=@newBalance where account_number=@accountNo";
            var cmd = new MySqlCommand(query, _dbconnect.Connect());
            cmd.Parameters.AddWithValue("@tableName", _dbconnect.DatabaseName);
            cmd.Parameters.AddWithValue("@newBalance", balance.ToString());
            cmd.Parameters.AddWithValue("@accountNo", AccountNumber);
            cmd.ExecuteNonQuery();
        }

        private long GetCurrentBalance()
        {
            long balance = 0;
            _dbconnect = new DatabaseConnectivity();
            string query = $@"select Balance from @tableName where account_number=@accountNo";
            var cmd = new MySqlCommand(query, _dbconnect.Connect());
            cmd.Parameters.AddWithValue("@tableName", _dbconnect.DatabaseName);
            cmd.Parameters.AddWithValue("@accountNo", AccountNumber);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                balance = (long)reader["Balance"];
            }

            return balance;
        }

        public long WithdrawMoney(long amount)
        {
            if (amount == 0)
            {
                return -1;
            }

            if (amount < 0)
            {
                return -2;
            }

            long balance = GetCurrentBalance();

            if (balance < amount)
            {
                return -3;
            }

            balance = balance - amount;
            SaveFinalBalance(balance);
            return 0;
        }

        public long DepositeMoney(long amount)
        {
            if (amount == 0)
            {
                return -1;
            }

            if (amount < 0)
            {
                return -2;
            }

            long balance = GetCurrentBalance();
            if (balance < amount)
            {
                return -3;
            }

            balance = balance + amount;
            SaveFinalBalance(balance);
            return 0;
        }

        public long CheckBalance()
        {
            throw new NotImplementedException();
        }

        public long DeleteAccount()
        {
           throw new NotImplementedException();
        }
    }
}
