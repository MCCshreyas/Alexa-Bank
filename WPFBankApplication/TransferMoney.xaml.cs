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
        string accountNum = "";
        public  string remainingReceiverBalance = "";
        public string remainingSenderBalance = "";


        public TransferMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;
        }

        public bool DoValidation()
        {
            if (TextBoxAccountNumber.Text == "" || TextBoxAccountPassword.Password == "" || TextBoxMoneyAmount.Text == "")
            {
                DialogBox.Show("Error", "Please fill up all the fields before procedding.","OK");
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
                DialogBox.Show("Error", "You don't have sufficient amount in your account to transfer. Your currrent balance is  " + Operations.GetCurrentbalance(accountNum),"OK");
            }
            else if (TextBoxAccountPassword.Password != Operations.GetPassword(accountNum))
            {
                DialogBox.Show("Error","Entered password for this account is incorrect.","OK");
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
                    UpdateSenderAccount(remainingSenderBalance);
                    UpdateReceiverAccount(remainingReceiverBalance);

                    if (Operations.DoesSendMobileNotifications(accountNum))
                    {
                        SendMobileNotification();
                        SendMobileNotificationToReciver();
                    }
                    DialogBox.Show("Sucess", "Transfer done sucessfully.","OK");
                }
                catch (SQLException error)
                {
                    DialogBox.Show("Error", "Something went wrong. " + error.Message, "OK");
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
                        (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");
                
                java.sql.PreparedStatement ps =
                        c.prepareStatement("update info set Balance = ? where account_number = ?");
                    ps.setString(1, bal);
                    ps.setString(2, accountNum);
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
                    DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
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
                        DialogBox.Show("Error","Account does not exist","OK");
                        return false;
                    }

                }
                catch (SQLException exception)
                {
                     DialogBox.Show("Error", "Something went wrong " + exception.Message, "OK");
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new Welcome(accountNum).Show();
            this.Hide();
        }
    }
}
