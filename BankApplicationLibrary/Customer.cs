using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BankApplicationLibrary
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public class Customer
    {
        #region Fields
        private string _name;
        private string _address;
        private string _phoneNumber;
        private string _email;
        private string _password;
        private string _accountNumber;
        private string _balance;
        private string _image;
        private Gender _gender;
        private DateTime _birthDate;
        #endregion

        #region FieldInitialization

        private string Name
        {
            get => this._name;
            set
            {
                if (value == null)
                {
                    throw new Exception("Name cannot be null");
                }
            }
        }

        private string Address
        {
            get => this._address;
            set => _address = value;
        }

        private string PhoneNumber
        {
            get => this._phoneNumber;
            set
            {
                if (value == null)
                {
                    throw new Exception("Phone number cannot be null");
                }

                if (value.Length > 10)
                {
                    throw new Exception("Phone number cannot be less than 10 numbers");
                }

                if (value.Length < 10)
                {
                    throw new Exception("Phone number cannot be greater than 10");
                }

                this._phoneNumber = value;
            }
        }


        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value == null)
                {
                    throw new Exception("Birth date cannot be null");
                }

                this._birthDate = value;
            }

        }

        public string Email
        {
            get => _email;
            set
            {
                var regex = new Regex(@"\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
                var match = regex.Match(value);
                if (match.Success)
                {
                    this._email = value;
                }
                else
                {
                    throw new Exception("Email address is not valid");
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value.Length < 8)
                {
                    throw new Exception("Password cannot be less than 8 characters");
                }

                this._password = value;
            }
        }

        public string AccountNumber
        {
            get => _accountNumber;
            set => _accountNumber = value;
        }

        public string Balance
        {
            get => _balance;
            set
            {
                if (value == null || value.Contains("-"))
                {
                    throw new Exception("Balance cannot be null or negative");
                }
            }
        }

        public string Image
        {
            get => _image;
            set => _image = value;
        }

        public Gender Gender1
        {
            get => _gender;
            set => _gender = value;
        }

        #endregion

        private MySqlConnection conn;
        
        Customer(string name)
        {
            Name = name;
            this.Balance = "100";
        }

        private bool AuthenticateLogin(string accountNumber, string enteredPassword)
        {
            var password = "";
            DatabaseConnectivity dbconnect = new DatabaseConnectivity();
            string query = $@"select Password from @tableName where account_number=@accountNo";
            var cmd = new MySqlCommand(query, dbconnect.Connect());
            cmd.Parameters.AddWithValue("@tableName", dbconnect.DatabaseName);
            cmd.Parameters.AddWithValue("@accountNo", accountNumber);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                password = (string)reader["Password"];
            }

            if (enteredPassword.Equals(password))
            {
                return true;
            }

            return false;
        }
    }
}