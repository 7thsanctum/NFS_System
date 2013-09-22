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
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NFS_System
{
    class Customer
    {
        private string username;
        public string getUsername { get { return username; } }
        private string password;
        public string getPassword { get { return password; } set { password = value; } }
        private string name;
        public string getName { get { return name; } set { name = value; } }
        private string address;
        public string getAddress { get { return address; } set { address = value; } }
        private List<string> accNums;
        public List<string> getAccountNumbers { get { return this.accNums; } set { accNums = value; } }

        public Customer(string user)
        {
            this.username = user;

            XDocument customer = XDocument.Load("Customer/" + username + ".xml");

            this.password = customer.Element("Customer").Element("password").Value;
            this.name = customer.Element("Customer").Element("name").Value;
            this.address = customer.Element("Customer").Element("address").Value;
            this.accNums = ((customer.Element("Customer").Element("accNum").Value).ToString()).Split(',').ToList();
        }

        public Customer(string user, string password, string name, string address)
        {
            this.username = user;
            this.password = password;
            this.name = name;
            this.address = address;
        }

        public bool saveCustomer()
        {
            XDocument newUser = new XDocument(new XElement("Customer",
                                    new XElement("username"),
                                    new XElement("password"),
                                    new XElement("name"),
                                    new XElement("address"),
                                    new XElement("accNum")));
            newUser.Element("Customer").Element("username").SetValue(username);
            newUser.Element("Customer").Element("password").SetValue(password);
            newUser.Element("Customer").Element("name").SetValue(name);
            newUser.Element("Customer").Element("address").SetValue(address);
            if(!(accNums == null)) newUser.Element("Customer").Element("accNum").SetValue(string.Join(",", accNums));
            newUser.Save("Customer/" + username + ".xml"); // saves the changes made to the file                
            return true;
        }

    }
}
