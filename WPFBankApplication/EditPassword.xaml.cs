using System.Windows;
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
            if (ReEnterPasswordBox.Text == string.Empty || NewPasswordTextBox.Text == string.Empty || OldPasswordTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill all the fields");
                return false;
            }
            return true;
        }
        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        { 
            if (DoValidation())
            {
                string oldPassword = Operations.GetPassword(_acc);
                if (oldPassword == OldPasswordTextBox.Text)
                {
                    if (NewPasswordTextBox.Text.Equals(ReEnterPasswordBox.Text))
                    {
                        SaveNewPassword(NewPasswordTextBox.Text.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Your new password is incorret","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Your old password is incorrect","Error",MessageBoxButton.OK,MessageBoxImage.Error);
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
    }
}
