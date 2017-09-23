using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using java.lang;
using java.sql;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        private static string accountNumber = string.Empty;
        public Welcome(string accountNum)
        {
            InitializeComponent();
            accountNumber = accountNum;
            TextBlockWelcome.Text = "Hello " + Operations.GetAccountHolderName(accountNumber);
            TextBlockAccountNo.Text = accountNum;
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
                MainSnackbar.MessageQueue.Enqueue("Welcome " + Operations.GetAccountHolderName(accountNumber));
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
                ps.setString(1, accountNumber);
                java.sql.ResultSet rs = ps.executeQuery();
                while (rs.next())
                {
                    imageFilePath = rs.getString("ImagePath");
                }

                ImageSourceConverter img = new ImageSourceConverter();
               // ImageBox.SetValue(System.Windows.Controls.Image.SourceProperty, img.ConvertFromString(imageFilePath));
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void ButtonCheckAccountDetails_Click(object sender, RoutedEventArgs e)
        {
            new AccountDetails(accountNumber).Show();
            this.Hide();
        }

        private void ButtonWithdrawMoney_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new WithdrawMoney(accountNumber).Show();
        }

        private void ButtonSaveMoney_Click(object sender, RoutedEventArgs e)
        {
            new SaveMoney(accountNumber).Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AccountSettings(accountNumber).Show();
            this.Hide();
        }

        private void TransferMoneyButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransferMoney(accountNumber).Show();
            this.Hide();
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
           new LoggedIn().Show();
            this.Hide();
        }
    }

   


}
