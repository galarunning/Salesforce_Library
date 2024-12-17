using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Objects
{
    public class Asset : INotifyPropertyChanged
    {
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _serialnumber;
        public string SerialNumber
        {
            get
            {
                return _serialnumber;
            }
            set
            {
                _serialnumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        private string _xpieportsystemid;
        public string XPIePortSystemID__c
        {
            get
            {
                return _xpieportsystemid;
            }
            set
            {
                _xpieportsystemid = value;
                OnPropertyChanged("XPIePortSystemID__c");
            }
        }

        private string _product_name_readonly__c;
        public string Product_Name_ReadOnly__c
        {
            get
            {
                return _product_name_readonly__c;
            }
            set
            {
                _product_name_readonly__c = value;
                OnPropertyChanged("Product_Name_ReadOnly__c");
            }
        }

        private string _account_name_readonly__c;
        public string Account_Name_ReadOnly__c
        {
            get
            {
                return _account_name_readonly__c;
            }
            set
            {
                _account_name_readonly__c = value;
                OnPropertyChanged("Account_Name_ReadOnly__c");
            }
        }

        #region Functions

        public override string ToString()
        {
            List<string> Fields = new List<string>();

            Fields.Add(Id);
            Fields.Add(SerialNumber);
            Fields.Add(XPIePortSystemID__c);
            Fields.Add(Product_Name_ReadOnly__c);
            Fields.Add(Account_Name_ReadOnly__c);

            return string.Join("||", Fields);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
