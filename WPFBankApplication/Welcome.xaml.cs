using System.Threading.Tasks;
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
        public static string AccountNumber = "";

        public Welcome(string accountNum)
        {
            InitializeComponent();
            AccountNumber = accountNum;
            TextBlockWelcome.Text = "Hello " + Operations.GetAccountHolderName(AccountNumber);
            TextBlockAccountNumber.Text = accountNum;
            TextBlockAvaiableBalance.Text = Operations.GetCurrentBalance(accountNum);
            GetAccountHolderImage();
            ShowWelcomeSnakbar();
        }


        public void ShowWelcomeSnakbar()
        {
            Task.Factory.StartNew(() => { Thread.sleep(1000); })
                .ContinueWith(
                    t =>
                    {
                        MainSnackbar.MessageQueue.Enqueue("Welcome " + Operations.GetAccountHolderName(AccountNumber));
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
                ps.setString(1, AccountNumber);
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
            new WithdrawMoney(AccountNumber).Show();
        }

        private void ButtonSaveMoney_Click(object sender, RoutedEventArgs e)
        {
            new SaveMoney(AccountNumber).Show();
            Hide();
        }

        private void TransferMoneyButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransferMoney(AccountNumber).Show();
            Hide();
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            var result = (int) DialogBox.Show("Log out ?", "Are you sure you want to log out?", "YES", "NO");

            switch (DialogBox.Result)
            {
                case DialogBox.ResultEnum.LeftButtonClicked:
                    new LoggedIn().Show();
                    Hide();
                    break;
            }
        }

        private void ButtonAccountSettings_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new AccountSettings(AccountNumber).Show();
        }
    }
}