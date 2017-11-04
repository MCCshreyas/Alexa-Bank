using System.Windows;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings
    {
        private readonly string _accnum;

        public AccountSettings(string accountNumber)
        {
            InitializeComponent();
            _accnum = accountNumber;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBox.Text)
            {
                case "Edit personal details":
                    Hide();
                    new EditPersonalDetails(_accnum).Show();
                    break;
                case "Change account password":
                    Content = new EditPassword(_accnum);
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new Welcome(_accnum).Show();
            Hide();
        }
    }
}