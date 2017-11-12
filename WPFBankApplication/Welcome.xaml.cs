﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ExtraTools;
using java.lang;
using java.sql;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome
    {
        private static string _accountNumber = "";

        public Welcome(string accountNum)
        {
            InitializeComponent();
            _accountNumber = accountNum;
            TextBlockWelcome.Text = "Hello " + Operations.GetAccountHolderName(_accountNumber);
            TextBlockAccountNumber.Text = accountNum;
            TextBlockAvaiableBalance.Text = Operations.GetCurrentBalance(accountNum);
            GetAccountHolderImage();
            ShowWelcomeSnakbar();
        }


        private void ShowWelcomeSnakbar()
        {
            Task.Factory.StartNew(() => { Thread.sleep(1000); })
                .ContinueWith(
                    t =>
                    {
                        MainSnackbar.MessageQueue.Enqueue("Welcome " + Operations.GetAccountHolderName(_accountNumber));
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void GetAccountHolderImage()
        {
            var imageFilePath = string.Empty;

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                var c = (Connection) DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root",
                    "9970209265");

                var ps = c.prepareStatement("select ImagePath from info where account_number = ?");
                ps.setString(1, _accountNumber);
                var rs = ps.executeQuery();
                while (rs.next())
                    imageFilePath = rs.getString("ImagePath");

                var img = new ImageSourceConverter();
                ImageBox.SetValue(Image.SourceProperty, img.ConvertFromString(imageFilePath));
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error", exception.ToString(), "OK");
            }
        }

        private void ButtonWithdrawMoney_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new WithdrawMoney(_accountNumber).Show();
        }

        private void ButtonSaveMoney_Click(object sender, RoutedEventArgs e)
        {
            new SaveMoney(_accountNumber).Show();
            Hide();
        }

        private void TransferMoneyButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransferMoney(_accountNumber).Show();
            Hide();
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            var result = (int) DialogBox.Show("Log out ?", "Are you sure you want to log out?", "YES", "NO");

            if (DialogBox.Result != DialogBox.ResultEnum.LeftButtonClicked)
                return;
            new LoggedIn().Show();
            Hide();
        }

        private void ButtonAccountSettings_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new AccountSettings(_accountNumber).Show();
        }
    }
}
