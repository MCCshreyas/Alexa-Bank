namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings
    {
        <<<<<<< HEAD
    private readonly string _accnum;
    ====== =
    private string _accnum;
    >>>>>>> parent of 388efd3...Refactored code
    public AccountSettings(string accountNumber)
    {
    internal InitializeComponent();
    _accnum = accountNumber;
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
    if (ComboBox.Text == "Edit personal details")
    {
    this.Hide();
    new EditPersonalDetails(_accnum).Show();
}
else if (ComboBox.Text == "Change account password")

{
<<<<<<< HEAD
case "Edit personal details":
Hide();
new EditPersonalDetails(_accnum).Show();
break;
case "Change account password":
Content = new EditPassword(_accnum);
break;
====== =
this.Content = new EditPassword(_accnum);
>>>>>>> parent of 388efd3...Refactored code
}
}
private void BackButton_Click(object sender, RoutedEventArgs e)
{
new Welcome(_accnum).Show();
this.Hide();
}
}
}