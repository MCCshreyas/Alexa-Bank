using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Exception = System.Exception;

namespace WPFBankApplication
{
    public partial class WithdrawMoney
    {
        private string _remainingBalance;
        private readonly string accountNum;

        /// <summary>
        ///     Following is constructor. Line no 27 code will read the current balance from database user account. Refer
        ///     Operations.cs and
        ///     look for method GetCurrentBalance
        /// </summary>
        public WithdrawMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;
            var accountBalance = Operations.GetCurrentBalance(accountNum);
            CurrentBalance.Text = accountBalance;
        }

        /// <summary>
        // Following method will send mobile notification to the account holder number as per the trasaction 
        // also refer Operations.cs file . We have used its method at line no 51
        /// </summary>
        public void SendMobileNotification()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

            TwilioClient.Init(accountSid, authToken);

            var sentMessage =
                string.Format(
                    "Your Alexa bank account (Acc no = {0}) has been debited with Rs.{1} . Your current balance is Rs.{2}",
                    accountNum, WithDrawMoneyTextBox.Text, _remainingBalance);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(accountNum));

            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }


        /// <summary>
        ///     Following method will check for input data from user and check for validation
        /// </summary>
        public bool DoValidation()
        {
            try
            {
                if (WithDrawMoneyTextBox.Text == string.Empty)
                {
                    DialogBox.Show("Warning", "You havent entered any amount to withdraw", "OK");
                    return false;
                }

                if (WithDrawMoneyTextBox.Text == "0")
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
            if (DoValidation())
                if (Convert.ToInt32(WithDrawMoneyTextBox.Text) >
                    Convert.ToInt32(Operations.GetCurrentBalance(accountNum)))
                {
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
                }
                else
                {
                    _remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(accountNum)) -
                                                         Convert.ToInt32(WithDrawMoneyTextBox.Text));

                    CurrentBalance.Text = _remainingBalance;

                    SaveFinalBalance();

                    if (Operations.DoesSendMobileNotifications(accountNum))
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
                var c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");
                var ps =
                    c.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, _remainingBalance);
                ps.setString(2, accountNum);
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
            if (WithDrawMoneyTextBox.Text != "" && WithDrawMoneyTextBox.Text != "")
                if (Convert.ToInt32(WithDrawMoneyTextBox.Text) >
                    Convert.ToInt32(Operations.GetCurrentBalance(accountNum)))
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
        }

        /// <summary>
        ///     Back button code
        /// </summary>
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Welcome(accountNum).Show();
            Hide();
        }
    }
}