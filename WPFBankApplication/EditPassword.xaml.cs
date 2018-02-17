

using System.Windows;
using ExtraTools;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for EditPassword.xaml
    /// </summary>
    public partial class EditPassword
    {
        private readonly string _acc;

        public EditPassword(string accountNumber)
        {
            InitializeComponent();
            _acc = accountNumber;
        }

        private bool DoValidation()
        {
            if (ReEnterPasswordBox.Password != string.Empty && NewPasswordTextBox.Password != string.Empty
                && OldPasswordTextBox.Password != string.Empty)
                return true;

            DialogBox.Show("Error", "Please fill all the fields", "OK");
            return false;
        }


        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!DoValidation())
                return;

            var oldPassword = Operations.GetPassword(_acc);

            if (oldPassword == OldPasswordTextBox.Password)
                if (NewPasswordTextBox.Password.Equals(ReEnterPasswordBox.Password))
                    SaveNewPassword(NewPasswordTextBox.Password);
                else
                    DialogBox.Show("Error", "Your new password is incorret", "OK");
            else
                DialogBox.Show("Error", "Your old password is incorrect", "OK");
        }

        private void SaveNewPassword(string newPass)
        {
            //try
            //{
            //    Class.forName("com.mysql.jdbc.Driver");
            //    var connection =
            //        (Connection) DriverManager.getConnection(Resource.DATABASE_URL, Resource.USERNAME,
            //            Resource.PASSWORD);

            //    var ps = connection.prepareStatement("update info set Password = ? where account_number = ?");
            //    ps.setString(1, newPass);
            //    ps.setString(2, _acc);
            //    ps.executeUpdate();
            //    connection.close();
            //    MessageBox.Show(
            //        "Password changed sucessfully",
            //        "Sucess",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Information);
            //    new LoggedIn().Show();
            //}
            //catch (SQLException exception)
            //{
            //    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            //}
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(new AccountSettings(_acc));
            parentWindow?.Show();
        }
    }
}
