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
using Exception = System.Exception;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for WithdrawMoney.xaml
    /// </summary>
    public partial class WithdrawMoney
    {
        private string accountNum;
        private string remainingBalance;


       // NOTE - Please refer Operations.cs file in this project. We have called bunch of methods from there


        public WithdrawMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;
            string accountBalance = Operations.GetCurrentbalance(accountNum);   //this will return the current balance of the logged in account holder
            CurrentBalance.Text = accountBalance;                               // and will save that in accountBalance variable

        }



        //following method will send mobile notification to the account holder number as per the trasaction 
        // also refer Operations.cs file . We have used its method at line no 51

        public void SendMobileNotification()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

            TwilioClient.Init(accountSid, authToken);

            string SentMessage =
                string.Format("Your Alexa bank account (Acc no = {0}) has been debited with Rs.{1} . Your current balance is Rs.{2}", accountNum,WithDrawMoneyTextBox.Text ,remainingBalance);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(accountNum));
            var message = MessageResource.Create
            (
                to,
                from: new PhoneNumber("+16674018291"),
                body: SentMessage
            );
        }
        
        public bool DoValidation()
        {
            try
            {
                if (WithDrawMoneyTextBox.Text == string.Empty)
                {
                    DialogBox.Show("Warning", "You havent entered any amount to withdraw","OK");
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
                MessageBox.Show("Something went wrong. " + e.Message,"Exception",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;

            }
            return true;
        }
        


        //following method will execute when withdraw money button gets clicked



        private void WithDrawMoney_Click(object sender, RoutedEventArgs e)
        {
            if (DoValidation())
            {

                //following condition will check is there sufficent balance in account holder itself before withdrawing money


                if (Convert.ToInt32(WithDrawMoneyTextBox.Text) > Convert.ToInt32(Operations.GetCurrentbalance(accountNum)))
                {
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
                }
                else
                {
                    remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentbalance(accountNum)) -
                                                        Convert.ToInt32(WithDrawMoneyTextBox.Text));
                    CurrentBalance.Text = remainingBalance;
                    SaveFinalBalance();
                   
                    if (Operations.DoesSendMobileNotifications(accountNum))
                    {
                        SendMobileNotification();
                    }

                    DialogBox.Show("Sucess", "Trasaction done sucessfully", "OK");
                    WithDrawMoneyTextBox.Text = "";
                }
            }
        }


        //following method will save the final balance to user account.


        private void SaveFinalBalance()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, remainingBalance);
                ps.setString(2, accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                MessageBox.Show("Something went wrong. " + exception.Message);
            }

        }
        
        private void WithDrawMoneyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void WithDrawMoneyTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (WithDrawMoneyTextBox.Text != "" && WithDrawMoneyTextBox.Text != "")
            {
                if (Convert.ToInt32(WithDrawMoneyTextBox.Text) > Convert.ToInt32(Operations.GetCurrentbalance(accountNum)))
                {
                    MainSnackbar.MessageQueue.Enqueue("You don't have sufficient balance to withdraw");
                }
            }
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
           new Welcome(accountNum).Show();
            this.Hide();
        }
    }
}
