using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        private string login;
        public string Login{ get { return login; } }

        private string password;
        public string Password { get { return password; } }

        public InputDialog() { }

        public bool GetResult()
        {
            InitializeComponent();
            if (ShowDialog().GetValueOrDefault())
                return true;
            else
                return false;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text != "" && passwordBox.Text != "")
            {
                login = loginBox.Text;
                password = passwordBox.Text;
                this.DialogResult = true;
            }
                
            else
                errorLabel.Content = "Wrong format";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
