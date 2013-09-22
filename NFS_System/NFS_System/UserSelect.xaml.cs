using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for UserSelect.xaml
    /// </summary>
    public partial class UserSelect : Window
    {
        string[] filenames = (Directory.GetFiles("Customer/", "*.xml").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToArray());
        public UserSelect(string message)
        {
            InitializeComponent();
            InputBox_Text.Text = message;
            
            for (int i = 0; i < filenames.Length; i++)
            {
                User_Selection.Items.Add(filenames[i]);
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (validateUsername()) this.Close(); 
            else MessageBox.Show("This user does not exist, please select one from the list.");
        }

        private bool validateUsername()
        {
            foreach (string x in filenames)
                if (User_Selection.Text == x)
                {
                    return true;
                }
            return false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (validateUsername()) e.Cancel = false; 
            else
            {
                e.Cancel = true;
                MessageBox.Show("This user does not exist, please select one from the list.");
            }
            
        }
    }
}
