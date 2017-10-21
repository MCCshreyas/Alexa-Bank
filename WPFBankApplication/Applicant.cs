
namespace WPFBankApplication
{
    using System;
    using System.Windows;

    using ExtraTools;

    //Name,Address,phone_number,Email,Password,account_number,Balance,ImagePath,Gender,MobileVerification,BirthDate
    public class Applicant
    {
        private string _name;
        private string _address;
        private string _phonenumber;
        private string _email;
        private string _password;
        private string _balance;
        private string _gender;
        private string _birthdate;

        public string Name
        {
            get => _name;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show($"Name cannot be empty  or null", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                    throw new Exception();
                }

                _name = value;
            }
        }

        public string Address
        {
            get => _address;
            
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show($"Address cannot be empty or null", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                _name = value;
            }
        }
        
        public string PhoneNumber
        {
            get => _phonenumber;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show($"Address cannot be empty or null", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                    throw new Exception();
                }

                if (value.Length == 0)
                {
                    MessageBox.Show($"The number cannot be of leangh 0", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                    throw new Exception();
                }

                if (value.Length > 10 || value.Length < 10)
                {
                    MessageBox.Show($"The phone number is invalid");
                    throw new Exception();
                }

                _phonenumber = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show($"Address cannot be empty or null");
                    throw new Exception();
                }

                if (value.Contains(".com") && value.Contains("@"))
                {
                    _email = Email;
                }
                else
                {
                    MessageBox.Show($"Email address is invalid", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception();
                }
            }
        }

        public string Password
        {
            get => _password;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DialogBox.Show("ERROR", "Password cannot be null", "OK");
                    throw new Exception();
                }
                
                if (value.Length != 8)
                {
                    DialogBox.Show("ERROR", "Password should be 8 character long");
                    throw new Exception();
                }
            }
        }


        private enum GenderTypes
        {
            Male = 0,
            Female = 1
        }

        public string Gender
        {
            get => _gender;

            set
            {
                var result = Enum.IsDefined(typeof(GenderTypes), value);

                if (result)
                {
                    _gender = value;
                }
                else
                {
                    DialogBox.Show("ERROR", "Gender is undefined", "OK");
                }
            }
        }

        public string BirthDate
        {
            get => _birthdate;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DialogBox.Show("ERROR", "Birth date cannot be empty");
                    throw new ArgumentNullException();
                }
                _birthdate = value;
            }
        }


        public Applicant()
        {  
        }
    }
}
