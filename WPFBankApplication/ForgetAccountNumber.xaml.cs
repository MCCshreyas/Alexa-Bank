namespace WPFBankApplication
{
    using System.Windows;

    using ExtraTools;

    using java.lang;
    using java.sql;

    /// <summary>
    ///     Interaction logic for ForgetAccountNumber.xaml
    /// </summary>
    public partial class ForgetAccountNumber
    {
        public ForgetAccountNumber()
        {
            InitializeComponent();
        }

        
        private bool DoValidation()
        {
            var isEmailValid = TextBoxEmailAddresss.Text.Contains("@");
            var isEmailValid2 = TextBoxEmailAddresss.Text.Contains(".com");

            if (TextBoxEmailAddresss.Text.Equals(string.Empty)
                || TextBoxPassword.Password.Equals(string.Empty))
            {
                DialogBox.Show("Error", "Please enter all fields", "OK");
                return false;
            }

            if (isEmailValid && isEmailValid2)
            {
                return true;
            }

            DialogBox.Show("Error", "Please enter valid email", "OK");
            return false;
        }

        private string AccountNumber
        {
            get
            {
                var accountNumber = string.Empty;
                try
                {
                    Class.forName("com.mysql.jdbc.Driver");
                    var c = DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME, Resource.PASSWORD);

                    var ps = c.prepareStatement("select account_number from info where Email = ? and Password = ?");
                    ps.setString(1, TextBoxEmailAddresss.Text);
                    ps.setString(2, TextBoxPassword.Password);
                    var result = ps.executeQuery();
                    while (result.next())
                    {
                        accountNumber = result.getString("account_number");
                    }

                    return accountNumber;
                }
                catch (SQLException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                return string.Empty;
            }
        }

        private void ButtonSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
            {
                return;
            }
            
            DialogBox.Show("Sucess", "Your account number is " + AccountNumber, "OK");
            Hide();
            new LoggedIn().Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new LoggedIn().Show();
            Hide();
        }
    }
}
