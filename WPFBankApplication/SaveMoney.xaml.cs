using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ExtraTools;
using java.lang;
using java.sql;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Connection = com.mysql.jdbc.Connection;
using Exception = System.Exception;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for SaveMoney.xaml
    /// </summary>
    public partial class SaveMoney
    {
        private readonly string accountNum;
        private string remainingBalance;

        public SaveMoney(string accountNumber)
        {
            InitializeComponent();
            this.accountNum = accountNumber;

            // please refer operations.cs file for GetCurrentBalance method
            var accountBalance = Operations.GetCurrentBalance(this.accountNum);
            CurrentBalance.Text = accountBalance;
        }


        private bool DoValidation()
        {
            try
            {
                if (SaveMoneyTextBox.Text.Equals(string.Empty))
                {
                    DialogBox.Show("Error", "You havent entered any amount to save");
                    return false;
                }

                if (SaveMoneyTextBox.Text == "0")
                {
                    MessageBox.Show("0 is not valid input");
                    return false;
                }
            }
            catch (Exception e)
            {
                DialogBox.Show("Error", "Something went wrong. " + e.Message, "OK");
                return false;
            }
            return true;
        }

        private void SaveMoneyButtonClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;
            this.remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(this.accountNum)) +
                                                 Convert.ToInt32(SaveMoneyTextBox.Text));
            CurrentBalance.Text = this.remainingBalance;
            SaveFinalBalance();

            if (Operations.DoesSendMobileNotifications(this.accountNum))
                SendMobileNotification();

            DialogBox.Show("Sucess", "Trasaction done sucessfully", "OK");
            SaveMoneyTextBox.Text = "";
        }

        private void SendMobileNotification()
        {
            App.InitializeTwilioAccount();

            var sentMessage =
                $"Your Alexa bank account (Acc no = {this.accountNum}) has been credited with Rs.{SaveMoneyTextBox.Text} . Your current balance is Rs.{this.remainingBalance}";


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(this.accountNum));
            MessageResource.Create(
                to,
                @from: new PhoneNumber("+16674018291"),
                body: sentMessage);
        }

        private void SaveFinalBalance()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = connection.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, this.remainingBalance);
                ps.setString(2, this.accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                MessageBox.Show("Something went wrong. " + exception.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveMoneyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new Welcome(this.accountNum).Show();
        }
    }
}
