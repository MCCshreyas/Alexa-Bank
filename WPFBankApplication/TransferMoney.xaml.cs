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
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for TransferMoney.xaml
    /// </summary>
    public partial class TransferMoney
    {
        private readonly string _accountNum;
        private string _remainingReceiverBalance;
        private string _remainingSenderBalance;


        public TransferMoney(string accountNumber)
        {
            InitializeComponent();
            _accountNum = accountNumber;
        }

        /// <summary>
        ///     Following method will look for empty input in text box
        /// </summary>
        /// <returns>If Empty returns false otherwise true</returns>
        private bool DoValidation()
        {
            if (TextBoxAccountNumber.Text != "" && TextBoxAccountPassword.Password != "" &&
                TextBoxMoneyAmount.Text != "")
                return true;
            DialogBox.Show("Error", "Please fill up all the fields before procedding.", "OK");
            return false;
        }


        /// <summary>
        ///     Following method contains core of Trasfer money logic
        /// </summary>
        private void TransferMoneyLogic()
        {
            _remainingReceiverBalance =
                Convert.ToString(
                    Convert.ToInt32(Operations.GetCurrentBalance(TextBoxAccountNumber.Text)) +
                    Convert.ToInt32(TextBoxMoneyAmount.Text));

            _remainingSenderBalance =
                Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)) -
                                 Convert.ToInt32(TextBoxMoneyAmount.Text));
        }


        /// <summary>
        ///     Line no 71 : Will compare current balance and entered amount and will fire error accordingly
        ///     Line no 79 : Will check for passsword does it correct or not.
        ///     Line no 84 : Will check for Receiver account number exists or not
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;
            if (Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)) <
                Convert.ToInt32(TextBoxMoneyAmount.Text))
                DialogBox.Show("Error",
                    "You don't have sufficient amount in your account to transfer. Your currrent balance is  " +
                    Operations.GetCurrentBalance(_accountNum), "OK");
            else if (TextBoxAccountPassword.Password != Operations.GetPassword(_accountNum))
                DialogBox.Show("Error", "Entered password for this account is incorrect.", "OK");
            else if (!CheckReceiverAccountNumber())
                DialogBox.Show("Error", "Account does not exist", "OK");
            else
                try
                {
                    TransferMoneyLogic();
                    UpdateSenderAccount(_remainingSenderBalance);
                    UpdateReceiverAccount(_remainingReceiverBalance);

                    if (Operations.DoesSendMobileNotifications(_accountNum))
                    {
                        SendMobileNotification();
                        SendMobileNotificationToReciver();
                    }

                    DialogBox.Show("Sucess", "Transfer done sucessfully.", "OK");
                }
                catch (SQLException error)
                {
                    DialogBox.Show("Error", "Something went wrong. " + error.Message, "OK");
                }
        }

        /// <summary>
        ///     Following method will send mobile notification to registered mobile number
        /// </summary>
        private void SendMobileNotificationToReciver()
        {
            App.InitializeTwilioAccount();

            var sentMessage =
                $"Rs.{TextBoxMoneyAmount.Text} has been transfered to your Alexa bank account (Acc no - {TextBoxAccountNumber.Text}) from other account (Acc no -  {TextBoxAccountNumber.Text}). Your current balance is {_remainingReceiverBalance} .";

            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(TextBoxAccountNumber.Text));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }


        private void SendMobileNotification()
        {
            App.InitializeTwilioAccount();
            var sentMessage =
                $"Rs.{TextBoxMoneyAmount.Text} has been transfered from your Alexa bank account (Acc no - {_accountNum}) to other account (Acc no -  {TextBoxAccountNumber.Text})";


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(_accountNum));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }

        private void UpdateSenderAccount(string bal)
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection =
                    (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                        "9970209265");

                var ps =
                    connection.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, bal);
                ps.setString(2, _accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
            }
        }


        private void UpdateReceiverAccount(string bal)
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection =
                    (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                        "9970209265");

                var ps =
                    connection.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, bal);
                ps.setString(2, TextBoxAccountNumber.Text);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
            }
        }

        private bool CheckReceiverAccountNumber()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var connection =
                    (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                        "9970209265");

                var ps = connection.prepareStatement("select * from info where account_number = ?");
                ps.setString(1, TextBoxAccountNumber.Text);
                var result = ps.executeQuery();

                if (result.next() == false)
                {
                    DialogBox.Show("Error", "Account does not exist", "OK");
                    return false;
                }
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
            }
            return true;
        }

        /// <summary>
        ///     Following code will restrict textbox to only accepts numbers and not chars.
        ///     As details will be numerics and not char
        /// </summary>
        private void TextBoxAccoutPassword_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBoxMoneyAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxMoneyAmount.Text == "" || TextBoxMoneyAmount.Text == "")
                return;
            if (Convert.ToInt32(TextBoxMoneyAmount.Text) >
                Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)))
                MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new Welcome(_accountNum).Show();
            Hide();
        }
    }
}
