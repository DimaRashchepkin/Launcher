using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using DataProvider;

namespace Launcher
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private Provider provider = new Provider("players.xml", "games.xml");
		private Player player = new Player();
		private bool isLoggedIn = false;

        private void LogInButton_Click(object sender, RoutedEventArgs e)
		{
			InputDialog dialog = new InputDialog();
			if (dialog.GetResult())
			{
                LogIn(dialog.Login, dialog.Password);
			}
			return;
		}

		private void LogIn(string login, string password)
		{
            Player current = provider.LogIn(login, password);
            if (current.Id == 0)
                MessageBox.Show("Wrong login or password");
            else
            {
                player = current;
                isLoggedIn = true;
                infoLabel.Content = player.Name;
            }
            return;
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            InputDialog dialog = new InputDialog();
            if (dialog.GetResult())
            {
                if (provider.Register(dialog.Login, dialog.Password))
				{
                    MessageBox.Show("Registration completed");
                    LogIn(dialog.Login, dialog.Password);
                }
                else
                {
                    MessageBox.Show("User exists");
                }
            }
            return;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
		{
            if (isLoggedIn)
            {
                player = new Player();
                isLoggedIn = false;
                infoLabel.Content = "Log in or register";
                MessageBox.Show("Logging out completed");
            }
            return;
		}

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isLoggedIn)
            {
                string arguments = "";
                for (int i = 0; i < 15; i++)
                    arguments += "0 ";
                ProcessStartInfo startInfo = new ProcessStartInfo("preference.exe");
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.Arguments = arguments;
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                try
                {
                    provider.SaveGameResult(player.Id, 0);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("System error: result file isn't exist");
                }
            }
            else
                MessageBox.Show("Please log in or register first");
            return;
        }

		private void RecordsButton_Click(object sender, RoutedEventArgs e)
		{   
            List<string> records = provider.GetRecords();
            RecordsDialog dialog = new RecordsDialog();
            dialog.ShowRecords(records);
			return;
		}
    }
}
