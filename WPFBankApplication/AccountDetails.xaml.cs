using System.Windows;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for AccountDetails.xaml
    /// </summary>
    public partial class AccountDetails
    {
        public static string AccountNo = string.Empty;

        public AccountDetails(string accountNumber)
        {
            InitializeComponent();
            AccountNo = accountNumber;
            
        }
    }
}
