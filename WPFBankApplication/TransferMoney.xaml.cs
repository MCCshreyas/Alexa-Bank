using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
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
    public partial class TransferMoney : Window
    {
        string accountNum = "";
        private string remainingReceiverBalance = "";
        private string remainingSenderBalance = "";


        public TransferMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;
        }

        public bool DoValidation()
        {
            if (TextBoxAccountNumber.Text == "" || TextBoxAccountPassword.Password == "" || TextBoxMoneyAmount.Text == "")
            {
                MessageBox.Show("Please fill up all the fields before procedding.","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return false;
            }
        
            return true;
        }
        
        
        public void TransferMoneyLogic()
        {
            remainingReceiverBalance =
                Convert.ToString(
                    Convert.ToInt32(Operations.GetCurrentbalance(TextBoxAccountNumber.Text)) + Convert.ToInt32(TextBoxMoneyAmount.Text));

            remainingSenderBalance =
                Convert.ToString(Convert.ToInt32(Operations.GetCurrentbalance(accountNum)) -
                                 Convert.ToInt32(TextBoxMoneyAmount.Text));
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
            {
               
            }
           else if (Convert.ToInt32(Operations.GetCurrentbalance(accountNum)) < Convert.ToInt32(TextBoxMoneyAmount.Text))
            {
                MessageBox.Show("You don't have sufficient amount in your account to transfer. Your currrent balance is  " + Operations.GetCurrentbalance(accountNum), "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else if (TextBoxAccountPassword.Password != Operations.GetPassword(accountNum).ToString())
            {
                MessageBox.Show("Entered password for this account is incorrect. ", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else if (!CheckReceiverAccountNumber())
            {
                MessageBox.Show("Account does not exist");
            }
            else
            {
                try
                {
                    TransferMoneyLogic();
                    UpdateSenderAccount(remainingSenderBalance);
                    UpdateReceiverAccount(remainingReceiverBalance);

                    if (Operations.DoesSendMobileNotifications(accountNum))
                    {
                        SendMobileNotification();
                        SendMobileNotificationToReciver();
                    }
                    MessageBox.Show("Transfer done sucessfully.", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SQLException error)
                {
                    MessageBox.Show(error.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }

        private void SendMobileNotificationToReciver()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

            TwilioClient.Init(accountSid, authToken);

            string SentMessage =
                string.Format("Rs.{0} has been transfered to your Alexa bank account (Acc no - {1}) from other account (Acc no -  {2}). Your current balance is {3} .", TextBoxMoneyAmount.Text, TextBoxAccountNumber.Text, TextBoxAccountNumber.Text,remainingReceiverBalance);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(TextBoxAccountNumber.Text));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: SentMessage
            );
        }


        public void SendMobileNotification()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

            TwilioClient.Init(accountSid, authToken);

            string SentMessage =
                string.Format("Rs.{0} has been transfered from your Alexa bank account (Acc no - {1}) to other account (Acc no -  {2})" , TextBoxMoneyAmount.Text , accountNum, TextBoxAccountNumber.Text);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(accountNum));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: SentMessage
            );
        }

        public void UpdateSenderAccount(string bal)
            {
                try
                {
                   
                    Class.forName("com.mysql.jdbc.Driver");
                    Connection c =
                        (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                            "9970209265");

                    java.sql.PreparedStatement ps =
                        c.prepareStatement("update info set Balance = ? where account_number = ?");
                    ps.setString(1, bal);
                    ps.setString(2, accountNum);
                    ps.executeUpdate();
                }
                catch (SQLException exception)
                {
                    MessageBox.Show("Something went wrong. " + exception.Message);
                }
            }


           public void UpdateReceiverAccount(string bal)
            {
                try
                {
                    
                    Class.forName("com.mysql.jdbc.Driver");
                    Connection c =
                        (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                            "9970209265");

                    java.sql.PreparedStatement ps =
                        c.prepareStatement("update info set Balance = ? where account_number = ?");
                    ps.setString(1, bal);
                    ps.setString(2, TextBoxAccountNumber.Text);
                    ps.executeUpdate();
                }
                catch (SQLException exception)
                {
                    MessageBox.Show("Something went wrong. " + exception.Message);
                }
            }

            public bool CheckReceiverAccountNumber()
            {
                try
                {
                    Class.forName("com.mysql.jdbc.Driver");
                    Connection c =
                        (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                            "9970209265");

                    java.sql.PreparedStatement ps = c.prepareStatement("select * from info where account_number = ?");
                    ps.setString(1, TextBoxAccountNumber.Text);
                    ResultSet result = ps.executeQuery();


                    if (result.next() == false)
                    {
                        MessageBox.Show("Account does not exist");
                        return false;
                    }

                }
                catch (SQLException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                return true;
            }


        private void TextBoxAccoutPassword_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxMoneyAmount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TextBoxMoneyAmount.Text != "" && TextBoxMoneyAmount.Text != "")
            {
                if (Convert.ToInt32(TextBoxMoneyAmount.Text) > Convert.ToInt32(Operations.GetCurrentbalance(accountNum)))
                {
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
                }
            }
        }
    }
}
