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
    /// Interaction logic for SaveMoney.xaml
    /// </summary>
    public partial class SaveMoney
    {
        private string accountNum;
        private string remainingBalance;
        public SaveMoney(string accountNumber)
        {
            InitializeComponent();
            accountNum = accountNumber;

            // please refer operations.cs file for GetCurrentBalance method
            string accountBalance = Operations.GetCurrentBalance(accountNum);   
            CurrentBalance.Text = accountBalance;
        }
        

        public bool DoValidation()
        {
            try
            {
                if (SaveMoneyTextBox.Text == string.Empty)
                {
                    DialogBox.Show("Error","You havent entered any amount to save");
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
                DialogBox.Show("Error","Something went wrong. " + e.Message,"OK");
                return false;

            }
            return true;
        }

        private void SaveMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (DoValidation())
            {
                remainingBalance = Convert.ToString(Convert.ToInt32(Operations.GetCurrentBalance(accountNum)) +
                                                        Convert.ToInt32(SaveMoneyTextBox.Text));
                CurrentBalance.Text = remainingBalance;
                SaveFinalBalance();

                if (Operations.DoesSendMobileNotifications(accountNum))
                {
                    SendMobileNotification();
                }

                DialogBox.Show("Sucess", "Trasaction done sucessfully","OK");
                SaveMoneyTextBox.Text = "";
            }
        }

        public void SendMobileNotification()
        {
            const string accountSid = "ACa4e91ac77184d82e6b7e7db26612c8d0";
            const string authToken = "cf88bc0c7f9a1c67f9ea49d5917a9be6";

            TwilioClient.Init(accountSid, authToken);

            string SentMessage =
                string.Format("Your Alexa bank account (Acc no = {0}) has been credited with Rs.{1} . Your current balance is Rs.{2}", accountNum, SaveMoneyTextBox.Text, remainingBalance);


            var to = new PhoneNumber("+91" + Operations.GetAccountHolderMobileNumber(accountNum));
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber("+16674018291"),
                body: SentMessage);
        }

        private void SaveFinalBalance()
        {
            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                PreparedStatement ps = c.prepareStatement("update info set Balance = ? where account_number = ?");
                ps.setString(1, remainingBalance);
                ps.setString(2, accountNum);
                ps.executeUpdate();
            }
            catch (SQLException exception)
            {
                MessageBox.Show("Something went wrong. " + exception.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void SaveMoneyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
           this.Hide();
           new Welcome(accountNum).Show();
        }

    }
}
