using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Objects
{
    public class Report : INotifyPropertyChanged
    {
        private List<ReportRow> _rows;
        public List<ReportRow> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
                OnPropertyChanged("Rows");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
