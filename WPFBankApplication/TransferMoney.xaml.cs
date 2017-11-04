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

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for TransferMoney.xaml
    /// </summary>
    public partial class TransferMoney
    {
        readonly string _accountNum;
        public string RemainingReceiverBalance;
        public string RemainingSenderBalance;


        public TransferMoney(string accountNumber)
        {
            InitializeComponent();
            _accountNum = accountNumber;
        }

        /// <summary>
        /// Following method will look for empty input in text box
        /// </summary>
        /// <returns>If Empty returns false otherwise true</returns>
        public bool DoValidation()
        {
            if (TextBoxAccountNumber.Text == "" || TextBoxAccountPassword.Password == "" ||
                TextBoxMoneyAmount.Text == "")
            {
                DialogBox.Show("Error", "Please fill up all the fields before procedding.", "OK");
                return false;
            }
            return true;
        }


        /// <summary>
        /// Following method contains core of Trasfer money logic
        /// </summary>
        public void TransferMoneyLogic()
        {
            RemainingReceiverBalance =
                Convert.ToString(
                    Convert.ToInt32(Operations.GetCurrentBalance(TextBoxAccountNumber.Text)) + Convert.ToInt32(TextBoxMoneyAmount.Text));

            RemainingSenderBalance =
                Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)) -
                                 Convert.ToInt32(TextBoxMoneyAmount.Text));
        }


        /// <summary>
        /// Line no 71 : Will compare current balance and entered amount and will fire error accordingly
        /// Line no 79 : Will check for passsword does it correct or not.
        /// Line no 84 : Will check for Receiver account number exists or not
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DoValidation())
            {
                if (Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)) <
                    Convert.ToInt32(TextBoxMoneyAmount.Text))
                {
                    DialogBox.Show("Error",
                        "You don't have sufficient amount in your account to transfer. Your currrent balance is  " +
                        Operations.GetCurrentBalance(_accountNum), "OK");
                }
                else if (TextBoxAccountPassword.Password != Operations.GetPassword(_accountNum))
                {
                    DialogBox.Show("Error", "Entered password for this account is incorrect.", "OK");
                }
                else if (!CheckReceiverAccountNumber())
                {
                    DialogBox.Show("Error", "Account does not exist", "OK");
                }
                else
                {
                    try
                    {
                        TransferMoneyLogic();
                        UpdateSenderAccount(RemainingSenderBalance);
                        UpdateReceiverAccount(RemainingReceiverBalance);

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
            }
        }

        /// <summary>
        /// Following method will send mobile notification to registered mobile number
        /// </summary>
        private void SendMobileNotificationToReciver()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
            TwilioClient.Init(accountSid, authToken);
            string sentMessage =
                string.Format("Rs.{0} has been transfered to your Alexa bank account (Acc no - {1}) from other account (Acc no -  {2}). Your current balance is {3} .", TextBoxMoneyAmount.Text, TextBoxAccountNumber.Text, TextBoxAccountNumber.Text, RemainingReceiverBalance);

            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(TextBoxAccountNumber.Text));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }


        public void SendMobileNotification()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";
            TwilioClient.Init(accountSid, authToken);
            string sentMessage =
                string.Format("Rs.{0} has been transfered from your Alexa bank account (Acc no - {1}) to other account (Acc no -  {2})", TextBoxMoneyAmount.Text, _accountNum, TextBoxAccountNumber.Text);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(_accountNum));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: sentMessage
            );
        }

        public void UpdateSenderAccount(string bal)
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c =
                    (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps =
                        c.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, bal);
                ps.setString(2, _accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
            }
        }


        public void UpdateReceiverAccount(string bal)
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c =
                    (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                        "9970209265");

                java.sql.PreparedStatement ps =
                    c.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, bal);
                ps.setString(2, TextBoxAccountNumber.Text);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
            }
        }

        public bool CheckReceiverAccountNumber()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c =
                    (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                        "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("select * from info where account_number = ?");
                ps.setString(1, TextBoxAccountNumber.Text);
                ResultSet result = ps.executeQuery();

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
        /// Following code will restrict textbox to only accepts numbers and not chars. 
        /// As details will be numerics and not char
        /// </summary>
        private void TextBoxAccoutPassword_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBoxMoneyAmount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TextBoxMoneyAmount.Text != "" && TextBoxMoneyAmount.Text != "")
            {
                if (Convert.ToInt32(TextBoxMoneyAmount.Text) > Convert.ToInt32(Operations.GetCurrentBalance(_accountNum)))
                {
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new Welcome(_accountNum).Show();
            this.Hide();
        }
    }
}
