using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using ExtraTools;
using java.lang;
using java.sql;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
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
            Task.Factory.StartNew(() =>
            {
                Thread.sleep(1000);
            }).ContinueWith(t =>
            {
                MainSnackbar.MessageQueue.Enqueue("Welcome " + Operations.GetAccountHolderName(AccountNumber));
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void GetAccountHolderImage()
        {
            string imageFilePath = string.Empty;

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection c = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = c.prepareStatement("select ImagePath from info where account_number = ?");
                ps.setString(1, AccountNumber);
                java.sql.ResultSet rs = ps.executeQuery();
                while (rs.next())
                {
                    imageFilePath = rs.getString("ImagePath");
                }

               ImageSourceConverter img = new ImageSourceConverter();
               ImageBox.SetValue(System.Windows.Controls.Image.SourceProperty, img.ConvertFromString(imageFilePath));
            }
            catch (SQLException exception)
            {
                DialogBox.Show("Error",exception.ToString(),"OK");
            }
        }
        
        private void ButtonWithdrawMoney_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new WithdrawMoney(AccountNumber).Show();
        }

        private void ButtonSaveMoney_Click(object sender, RoutedEventArgs e)
        {
            new SaveMoney(AccountNumber).Show();
            this.Hide();
        }
        private void TransferMoneyButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransferMoney(AccountNumber).Show();
            this.Hide();
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            int result = (int)DialogBox.Show("Log out ?", "Are you sure you want to log out?", "YES", "NO");
            
            switch (DialogBox.Result)
            {
                case DialogBox.ResultEnum.LeftButtonClicked:
                    new LoggedIn().Show();
                    this.Hide();
                    break;
            }
        }

        private void ButtonAccountSettings_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new AccountSettings(AccountNumber).Show();
        }
    }
}
