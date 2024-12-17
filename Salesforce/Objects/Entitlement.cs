using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Objects
{
    public class Entitlement : INotifyPropertyChanged
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

        private bool? _isdeleted;
        public bool? IsDeleted
        {
            get
            {
                return _isdeleted;
            }
            set
            {
                _isdeleted = value;
                OnPropertyChanged("IsDeleted");
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

        private DateTime? _createddate;
        public DateTime? CreatedDate
        {
            get
            {
                return _createddate;
            }
            set
            {
                _createddate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        private DateTime? _lastmodifieddate;
        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastmodifieddate;
            }
            set
            {
                _lastmodifieddate = value;
                OnPropertyChanged("LastModifiedDate");
            }
        }

        private string _accountid;
        public string AccountId
        {
            get
            {
                return _accountid;
            }
            set
            {
                _accountid = value;
                OnPropertyChanged("AccountId");
            }
        }

        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private string _servicecontractid;
        public string ServiceContractId
        {
            get
            {
                return _servicecontractid;
            }
            set
            {
                _servicecontractid = value;
                OnPropertyChanged("ServiceContractId");
            }
        }

        private string _contractlineitem;
        public string ContractLineItem
        {
            get
            {
                return _contractlineitem;
            }
            set
            {
                _contractlineitem = value;
                OnPropertyChanged("ContractLineItem");
            }
        }

        private string _assetid;
        public string AssetId
        {
            get
            {
                return _assetid;
            }
            set
            {
                _assetid = value;
                OnPropertyChanged("AssetId");
            }
        }

        private DateTime? _startdate;
        public DateTime? StartDate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
                OnPropertyChanged("DateTime");
            }
        }

        private DateTime? _enddate;
        public DateTime? EndDate
        {
            get
            {
                return _enddate;
            }
            set
            {
                _enddate = value;
                OnPropertyChanged("EndDate");
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
