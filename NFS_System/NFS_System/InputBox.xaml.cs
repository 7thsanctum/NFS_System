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
using System.Windows.Shapes;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox(string textblock_text)
        {
            InitializeComponent();
            InputBox_Text.Text = textblock_text;
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if(validateInput()) this.Close();
        }

        private bool validateInput()
        {
            int amount;
            if (InputBox_Input.Text.Length == 0)
            {
                errormessage.Text = "Please enter a number";
                InputBox_Input.Focus();
                return false;
            }
            else if (Int32.TryParse(InputBox_Input.Text, out amount))
            {
                if (amount >= 0) return true;
                else errormessage.Text = "You cannot withdraw/deposit a negative value!";
                return false;
            }
            else
            {
                errormessage.Text = "Please enter a valid number";
                InputBox_Input.Focus();
                return false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (validateInput()) e.Cancel = false;
            else
            {
                e.Cancel = true;
                MessageBox.Show("Please enter a value.");
            }
        }
    }
}
