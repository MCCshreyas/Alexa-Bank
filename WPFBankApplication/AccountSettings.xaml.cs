namespace WPFBankApplication
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings
    {
        private readonly string accnum;

        public AccountSettings(string accountNumber)
        {
            InitializeComponent();
            accnum = accountNumber;
        }
       
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            switch (ComboBox.Text)
            {
                case "Edit personal details":
                    Hide();
                    new EditPersonalDetails(accnum).Show();
                    break;
                case "Change account password":
                    Content = new EditPassword(accnum);
                    break;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            new Welcome(accnum).Show();
            Hide();
        }
    }
}
