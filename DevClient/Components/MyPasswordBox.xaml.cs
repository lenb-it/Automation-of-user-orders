using System.Windows;
using System.Windows.Controls;

namespace DevClient.Components
{
    public partial class MyPasswordBox : UserControl
    {
        public static DependencyProperty PasswordProperty = DependencyProperty
            .Register(nameof(Password), typeof(string), typeof(MyPasswordBox), 
                new PropertyMetadata(string.Empty));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public MyPasswordBox()
        {
            InitializeComponent();
        }

        private void _passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _passwordBox.Password;
        }
    }
}
