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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    /// <summary>
    /// Interaction logic for SetInterestScreen.xaml
    /// </summary>
    public partial class SetInterestScreen : Window
    {
        XDocument interestrates;
        public SetInterestScreen()
        {
            InitializeComponent();
            interestrates = XDocument.Load("NFS_System_Files/InterestRates.xml");
            savings_text.Text = interestrates.Root.Element("Savings").Value;
            mortgage_text.Text = interestrates.Root.Element("Mortgage").Value;
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (formValidation())
            {
                interestrates.Root.Element("Savings").SetValue(savings_text.Text);
                interestrates.Root.Element("Mortgage").SetValue(mortgage_text.Text);
                interestrates.Save("NFS_System_Files/InterestRates.xml");
                this.Close();
            }
        }

        private bool formValidation()
        {
            float value;
            if (savings_text.Text.Length == 0 || !float.TryParse(savings_text.Text, out value))
            {
                errormessage.Text = "Invalid Savings Value";
                savings_text.Focus();
                return false;
            }
            else if (value < 0)
            {
                errormessage.Text = "Cannot have a value less than 0!";
                savings_text.Focus();
                return false;
            }
            if (mortgage_text.Text.Length == 0 || !float.TryParse(mortgage_text.Text, out value))
            {
                errormessage.Text = "Invalid Mortgage Value";
                mortgage_text.Focus();
                return false;
            }
            else if (value < 0)
            {
                errormessage.Text = "Cannot have a value less than 0!";
                mortgage_text.Focus();
                return false;
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (formValidation()) e.Cancel = false;
            else e.Cancel = true; 
        }
    }
}
