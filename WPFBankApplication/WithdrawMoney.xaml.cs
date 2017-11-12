
namespace WPFBankApplication
{
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

    /// <summary>
    /// The withdraw money.
    /// </summary>
    public partial class WithdrawMoney
    {
        private readonly string accountNum;
        private string remainingBalance;

        /// <summary>
        ///     Following is constructor. Line no 27 code will read the current balance from database user account. Refer
        ///     Operations.cs and
        ///     look for method GetCurrentBalance
        /// </summary>
        // ReSharper disable once StyleCop.SA1642
        public WithdrawMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;
            var accountBalance = Operations.GetCurrentBalance(accountNum);
            CurrentBalance.Text = accountBalance;
        }
        

        private void SendMobileNotification()
        {
            App.InitializeTwilioAccount();

            var sentMessage =
                $"Your Alexa bank account (Acc no = {accountNum}) has been debited with Rs.{WithDrawMoneyTextBox.Text} . Your current balance is Rs.{remainingBalance}";


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(accountNum));

            MessageResource.Create(
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage);
        }

        /// <summary>
        /// Following method will check for input data from user and check for validation
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
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
        private void WithDrawMoneyClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;
            if (Convert.ToInt32(WithDrawMoneyTextBox.Text) >
                Convert.ToInt32(Operations.GetCurrentBalance(accountNum)))
            {
                MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
            }
            else
            {
                remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(accountNum)) -
                                                     Convert.ToInt32(WithDrawMoneyTextBox.Text));

                CurrentBalance.Text = remainingBalance;

                SaveFinalBalance();

                if (Operations.DoesSendMobileNotifications(accountNum))
                    SendMobileNotification();

                DialogBox.Show("Sucess", "Trasaction done sucessfully", "OK");
                WithDrawMoneyTextBox.Text = string.Empty;
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
                var connection = DriverManager.getConnection(
                    "jdbc:mysql://localhost/bankapplication",
                    "root",
                    "9970209265");
                var ps =
                    connection.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, remainingBalance);
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
       
        private void WithDrawMoneyTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (WithDrawMoneyTextBox.Text == string.Empty || WithDrawMoneyTextBox.Text == string.Empty)
                return;
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
