namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for AccountDetails.xaml
    /// </summary>
    public partial class AccountDetails
    {
        public static string accountNo = string.Empty;

        public AccountDetails(string accountNumber)
        {
            InitializeComponent();
            accountNo = accountNumber;
        }
    }
}