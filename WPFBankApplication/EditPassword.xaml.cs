using System.Windows;
using ExtraTools;
using java.lang;
using java.sql;
using static System.Windows.MessageBoxButton;
using Connection = com.mysql.jdbc.Connection;

namespace WPFBankApplication
{
    /// <summary>
    /// Interaction logic for EditPassword.xaml
    /// </summary>
    public partial class EditPassword
    {
        string _acc;
        public EditPassword(string accountNumber)
        {
            InitializeComponent();
            _acc = accountNumber;
        }
        
        private bool DoValidation()
        {
            if (ReEnterPasswordBox.Password == "" || NewPasswordTextBox.Password == "" || OldPasswordTextBox.Password == "")
            {
                DialogBox.Show("Error","Please fill all the fields","OK");
                return false;
            }
            return true;
        }
        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        { 
            if (DoValidation())
            {
                string oldPassword = Operations.GetPassword(_acc);
                if (oldPassword == OldPasswordTextBox.Password)
                {
                    if (NewPasswordTextBox.Password.Equals(ReEnterPasswordBox.Password))
                    {
                        SaveNewPassword(NewPasswordTextBox.Password);
                    }
                    else
                    {
                        DialogBox.Show("Error", "Your new password is incorret","OK");
                    }
                }
                else
                {
                    DialogBox.Show("Error", "Your old password is incorrect","OK");
                }
            }
        }
        
        private void SaveNewPassword(string newPass)
        {

            try
            {
                Class.forName("com.mysql.jdbc.Driver");
                Connection connection = (Connection)DriverManager.getConnection("jdbc:mysql://localhost/bankapplication", "root", "9970209265");

                java.sql.PreparedStatement ps = connection.prepareStatement("update info set Password = ? where account_number = ?");
                ps.setString(1, newPass);
                ps.setString(2, _acc);
                ps.executeUpdate();
                connection.close();
                MessageBox.Show("Password changed sucessfully","Sucess",MessageBoxButton.OK,MessageBoxImage.Information);
                new LoggedIn().Show();
            }
            catch (SQLException exception)
            {
                MessageBox.Show(exception.ToString(), "Error", OK, MessageBoxImage.Stop);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(new AccountSettings(_acc));
            parentWindow.Show();
        }
    }
}
