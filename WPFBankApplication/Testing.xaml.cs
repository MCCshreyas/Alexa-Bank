using System.Windows;

namespace WPFBankApplication
{
    /// <summary>
    ///     Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : Window
    {
        public Testing()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            myDialog.IsOpen = true;
        }
    }
}