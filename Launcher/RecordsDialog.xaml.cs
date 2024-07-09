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

namespace Launcher
{
    public partial class RecordsDialog : Window
    {
        public RecordsDialog() { }

        public void ShowRecords(List<string> records)
        {
            InitializeComponent();
            foreach (string record in records)
            {
                list.Items.Add(record);
            }
            ShowDialog().GetValueOrDefault();
            return;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
