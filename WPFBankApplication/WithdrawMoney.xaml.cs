using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Exception = System.Exception;

namespace WPFBankApplication
{
    public partial class WithdrawMoney
    {
        private readonly string _accountNum;
        private string _remainingBalance;

        /// <summary>
        ///     Following is constructor. Line no 27 code will read the current balance from database user account. Refer
        ///     Operations.cs and
        ///     look for method GetCurrentBalance
        /// </summary>
        public WithdrawMoney(string accountNumber)
        {
            InitializeComponent();
            _accountNum = accountNumber;
            var accountBalance = Operations.GetCurrentBalance(_accountNum);
            CurrentBalance.Text = accountBalance;
        }

        /// <summary>
        // Following method will send mobile notification to the account holder number as per the trasaction 
        // also refer Operations.cs file . We have used its method at line no 51
        /// </summary>
        private void SendMobileNotification()
        {
            App.InitializeTwilioAccount();

            var sentMessage =
                $"Your Alexa bank account (Acc no = {_accountNum}) has been debited with Rs.{WithDrawMoneyTextBox.Text} . Your current balance is Rs.{_remainingBalance}";


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(_accountNum));

            MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }


        /// <summary>
        ///     Following method will check for input data from user and check for validation
        /// </summary>
        private bool DoValidation()
        {
            try
            {
                if (WithDrawMoneyTextBox.Text.Equals(string.Empty))
                {
                    DialogBox.Show("Warning", "You havent entered any amount to withdraw", "OK");
                    return false;
                }

                if (WithDrawMoneyTextBox.Text.Equals("0"))
                {
                    DialogBox.Show("Warning", "0 is not valid input", "Got it");
                    return false;
                }
            }
            catch (Exception e)
            {
                DialogBox.Show("Exception", "Something went wrong. " + e.Message, "OK");
                return false;
            }
            return true;
        }

        /// <summary>
        ///     following method will execute when withdraw money button gets clicked
        /// </summary>
        private void WithDrawMoney_Click(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;
            if (Convert.ToInt32(WithDrawMoneyTextBox.Text) >
                Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)))
            {
                MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
            }
            else
            {
                _remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)) -
                                                     Convert.ToInt32(WithDrawMoneyTextBox.Text));

                CurrentBalance.Text = _remainingBalance;

                SaveFinalBalance();

                if (Operations.DoesSendMobileNotifications(_accountNum))
                    SendMobileNotification();

                DialogBox.Show("Sucess", "Trasaction done sucessfully", "OK");
                WithDrawMoneyTextBox.Text = "";
            }
        }

        /// <summary>
        ///     Following method will save the final balance to user account number
        /// </summary>
        private void SaveFinalBalance()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");
                var ps =
                    connection.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, _remainingBalance);
                ps.setString(2, _accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong. " + exception.Message, "OK");
            }
        }

        /// <summary>
        ///     Following code will restrict textbox to only accepts numbers and not chars.
        ///     As details will be numerics and not char
        /// </summary>
        private void WithDrawMoneyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        ///     Following event handler will execute as soon as user will enter amount
        /// </summary>
        private void WithDrawMoneyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WithDrawMoneyTextBox.Text == "" || WithDrawMoneyTextBox.Text == "")
                return;
            if (Convert.ToInt32(WithDrawMoneyTextBox.Text) >
                Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)))
                MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
        }

        /// <summary>
        ///     Back button code
        /// </summary>
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Welcome(_accountNum).Show();
            Hide();
        }
    }
}
