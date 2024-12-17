using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Objects
{
    public class ReportRow : INotifyPropertyChanged
    {
        private List<DataCell> _datacells;
        public List<DataCell> DataCells
        {
            get
            {
                return _datacells;
            }
            set
            {
                _datacells = value;
                OnPropertyChanged("DataCells");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
