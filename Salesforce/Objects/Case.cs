using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace SalesforceEngine.Objects
{
    public class Case : INotifyPropertyChanged
    {
        #region General

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

        private string _parentid;
        public string ParentId
        {
            get
            {
                return _parentid;
            }
            set
            {
                _parentid = value;
                OnPropertyChanged("ParentId");
            }
        }

        private Case _parent;
        public Case Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                OnPropertyChanged("Parent");
            }
        }

        private string _casenumber;
        public string CaseNumber
        {
            get
            {
                return _casenumber;
            }
            set
            {
                _casenumber = value;
                OnPropertyChanged("CaseNumber");
            }
        }

        private string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
                OnPropertyChanged("Subject");
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
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

        private string _internal_status__c;
        public string Internal_Status__c
        {
            get
            {
                return _internal_status__c;
            }
            set
            {
                _internal_status__c = value;
                OnPropertyChanged("Internal_Status__c");
            }
        }

        private string _severity__c;
        public string Severity__c
        {
            get
            {
                return _severity__c;
            }
            set
            {
                _severity__c = value;
                OnPropertyChanged("Severity__c");
            }
        }

        private string _serial__c;
        public string Serial__c
        {
            get
            {
                return _serial__c;
            }
            set
            {
                _serial__c = value;
                OnPropertyChanged("Serial__c");
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

        private string _account_region__c;
        public string Account_Region__c
        {
            get
            {
                return _account_region__c;
            }
            set
            {
                _account_region__c = value;
                OnPropertyChanged("Account_Region__c");
            }
        }

        private string _contactemail;
        public string ContactEmail
        {
            get
            {
                return _contactemail;
            }
            set
            {
                _contactemail = value;
                OnPropertyChanged("ContactEmail");
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
        public DateTime? CreatedMonth
        {
            get
            {
                if (CreatedDate != null)
                    return new DateTime(((DateTime)CreatedDate).Year, ((DateTime)CreatedDate).Month, 1, 0, 0, 0, 0);
                else
                    return null;
            }
        }

        private DateTime? _closeddate;
        public DateTime? ClosedDate
        {
            get
            {
                return _closeddate;
            }
            set
            {
                _closeddate = value;
                OnPropertyChanged("ClosedDate");
            }
        }
        public DateTime? ClosedMonth
        {
            get
            {
                if (ClosedDate != null)
                    return new DateTime(((DateTime)ClosedDate).Year, ((DateTime)ClosedDate).Month, 1, 0, 0, 0, 0);
                else
                {
                    return null;
                }
            }
        }
        
        public double Rolling_Age
        {
            get 
            {
                TimeSpan Age = DateTime.Now - (DateTime)CreatedDate;
                double days = Math.Round(Age.TotalDays, 2);
                return days;
            }
        }

        private string _recordtypeid;
        public string RecordTypeId
        {
            get
            {
                return _recordtypeid;
            }
            set
            {
                _recordtypeid = value;
                OnPropertyChanged("RecordTypeId");
            }
        }

        private RecordType _recordtype;
        public RecordType RecordType
        {
            get
            {
                return _recordtype;
            }
            set
            {
                _recordtype = value;
                OnPropertyChanged("RecordType");
            }
        }

        private string _ownerid;
        public string OwnerId
        {
            get
            {
                return _ownerid;
            }
            set
            {
                _ownerid = value;
                OnPropertyChanged("OwnerId");
            }
        }

        private string _productline__c;

        public string ProductLine__c
        {
            get { return _productline__c; }
            set 
            { 
                _productline__c = value;
                OnPropertyChanged("ProductLine__c");
            }
        }

        private User _owner;
        public User Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                OnPropertyChanged("Owner");
            }
        }

        public string _searchstring; 
        
        public string SearchString
        {
            get
            {
                return Id + "|" +
                       CaseNumber + "|" +
                       ParentId + "|" +
                       Description + "|" +
                       CR_Number__c + "|" +
                       State__c + "|" +
                       Property__c + "|" +
                       ProductLine__c + "|" +
                       Status + "|" +
                       Priority + "|" + 
                       Severity__c + "|" +
                       Owner.Name + "|";
            }



        }

        #endregion

        #region Service Request

        public string _businessHoursId;
        public string BusinessHoursId
        {
            get
            {
                return _businessHoursId;
            }
            set
            {
                _businessHoursId = value;
                OnPropertyChanged("BusinessHoursId");
            }
        }

        public string _supportLevel;
        public string SupportLevel
        {
            get
            {
                return _supportLevel;
            }
            set
            {
                _supportLevel = value;
                OnPropertyChanged("Support_Level__c");
            }
        }

        private double? _backline_resolution_time__c;
        public double? Backline_resolution_time__c
        {
            get
            {
                return _backline_resolution_time__c;
            }
            set
            {
                _backline_resolution_time__c = value;
                OnPropertyChanged("Backline_resolution_time__c");
            }
        }

        private string _first_technical_response__c;
        public string First_technical_response__c
        {
            get { return _first_technical_response__c; }
            set
            {
                _first_technical_response__c = value;
                OnPropertyChanged("First_Technical_Response__c");
            }
        }

        private double? _first_technical_response_time__c;
        public double? First_Technical_Response_Time__c
        {
            get
            {
                return _first_technical_response_time__c;
            }
            set
            {
                _first_technical_response_time__c = value;
                OnPropertyChanged("First_Technical_Response_Time__c");
            }
        }

        public string _provisional_entitlement_approved_date__c;
        public string Provisional_Entitlement_Approved_Date__c
        {
            get
            {
                return _provisional_entitlement_approved_date__c;
            }
            set
            {
                _provisional_entitlement_approved_date__c = value;
                OnPropertyChanged("Provisional_Entitlement_Approved_Date__c");
            }
        }

        private string _entitlementid;
        public string EntitlementId
        {
            get
            {
                return _entitlementid;
            }
            set
            {
                _entitlementid = value;
                OnPropertyChanged("EntitlementId");
            }
        }

        private Entitlement _entitlement;
        public Entitlement Entitlement
        {
            get
            {
                return _entitlement;
            }
            set
            {
                _entitlement = value;
                OnPropertyChanged("Entitlement");
            }
        }

        private string _action_next_step__c;
        public string Action_Next_Step__c
        {
            get
            {
                return _action_next_step__c;
            }
            set
            {
                _action_next_step__c = value;
                OnPropertyChanged("Action_Next_Step__c");
            }
        }

        private DateTime? _action_due__c;
        public DateTime? Action_Due__c
        {
            get
            {
                return _action_due__c;
            }
            set
            {
                _action_due__c = value;
                OnPropertyChanged("Action_Due__c");
            }
        }

        private DateTime? _expedite_approved_date__c;
        public DateTime? Expedite_Approved_Date__c
        {
            get
            {
                return _expedite_approved_date__c;
            }
            set
            {
                _expedite_approved_date__c = value;
                OnPropertyChanged("Expedite_Approved_Date__c");
            }
        }

        private DateTime? _customer_fix_supplied__c;
        public DateTime? Customer_Fix_Supplied__c
        {
            get
            {
                return _customer_fix_supplied__c;
            }
            set
            {
                _customer_fix_supplied__c = value;
                OnPropertyChanged("Customer_Fix_Supplied__c");
            }
        }

        private string _cr__c;
        public string CR__c
        {
            get
            {
                return _cr__c;
            }
            set
            {
                _cr__c = value;
                OnPropertyChanged("CR__c");
            }
        }

        private string _rma__c;
        public string RMA__c
        {
            get
            {
                return _rma__c;
            }
            set
            {
                _rma__c = value;
                OnPropertyChanged("RMA__c");
            }
        }

        private string e_mail_reference_string__c;
        public string E_mail_Reference_string__c
        {
            get
            {
                return e_mail_reference_string__c;
            }
            set
            {
                e_mail_reference_string__c = value;
                OnPropertyChanged("E_mail_Reference_string__c");
            }
        }

        #endregion

        #region Change Request

        private string _state__c;
        public string State__c
        {
            get
            {
                return _state__c;
            }
            set
            {
                _state__c = value;
                OnPropertyChanged("State__c");
            }
        }

        private string _property__c;
        public string Property__c
        {
            get
            {
                return _property__c;
            }
            set
            {
                _property__c = value;
                OnPropertyChanged("Property__c");
            }
        }

        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                OnPropertyChanged("Reason");
            }
        }

        private string _decline_reason__c;
        public string Decline_Reason__c
        {
            get
            {
                return _decline_reason__c;
            }
            set
            {
                _decline_reason__c = value;
                OnPropertyChanged("Decline_Reason__c");
            }
        }

        private string _jira_id__c;
        public string Jira_ID__c
        {
            get
            {
                return _jira_id__c;
            }
            set
            {
                _jira_id__c = value;
                OnPropertyChanged("Jira_ID__c");
            }
        }

        private string _priority;
        public string Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                OnPropertyChanged("Priority");
            }
        }

        #endregion

        #region Repair / Calibration

        private DateTime? _received_in_repair__c;
        public DateTime? Received_in_Repair__c
        {
            get
            {
                return _received_in_repair__c;
            }
            set
            {
                _received_in_repair__c = value;
                OnPropertyChanged("Received_in_Repair__c");
            }
        }

        private double? _time_in_repair__c;
        public double? Time_in_Repair__c
        {
            get
            {
                return _time_in_repair__c;
            }
            set
            {
                _time_in_repair__c = value;
                OnPropertyChanged("Time_in_Repair__c");
            }
        }

        #endregion

        #region Custom

        private string _customdescription;
        public string CustomDescription
        {
            get
            {
                return _customdescription;
            }
            set
            {
                _customdescription = value;
                OnPropertyChanged("CustomDescription");
            }
        }

        private string _unit;
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                OnPropertyChanged("Unit");
            }
        }

        private string _software;
        public string Software
        {
            get
            {
                return _software;
            }
            set
            {
                _software = value;
                OnPropertyChanged("Software");
            }
        }

        private string _posappversion;
        public string PosAppVersion
        {
            get
            {
                return _posappversion;
            }
            set
            {
                _posappversion = value;
                OnPropertyChanged("PosAppVersion");
            }
        }

        private string _folderpath;
        public string FolderPath
        {
            get
            {
                return _folderpath;
            }
            set
            {
                _folderpath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        private string _additionalnotes;
        public string AdditionalNotes
        {
            get
            {
                return _additionalnotes;
            }
            set
            {
                _additionalnotes = value;
                OnPropertyChanged("AdditionalNotes");
            }
        }

        // To show to Elliot -might not be correct.
        private string _CR_Number__c;
        public string CR_Number__c
        {
            get
            {
                return _CR_Number__c;
            }
            set
            {
                _CR_Number__c = value;
                OnPropertyChanged("CR_Number__c");
            }
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            List<string> Fields = new List<string>();

            #region General

            Fields.Add(Id);
            Fields.Add(ParentId);
            Fields.Add(CaseNumber);
            Fields.Add(Subject);
            Fields.Add(Status);
            Fields.Add(Serial__c);
            Fields.Add(Account_Name_ReadOnly__c);
            Fields.Add(Account_Region__c);

            if (CreatedDate != null)
            {
                Fields.Add(((DateTime)CreatedDate).ToString());
                Fields.Add(((DateTime)CreatedMonth).ToString("MM/yyyy"));
            }
            else
            {
                Fields.Add("");
                Fields.Add("");
            }

            if (ClosedDate != null)
            {
                Fields.Add(((DateTime)ClosedDate).ToString());
                Fields.Add(((DateTime)ClosedMonth).ToString("MM/yyyy"));
            }
            else
            {
                Fields.Add("");
                Fields.Add("");
            }

            if (RecordType != null) Fields.Add(RecordType.Name);
            else Fields.Add("Not Found");

            if (Owner != null) Fields.Add(Owner.Name);
            else Fields.Add("N/A");
            #endregion

            #region Service Request

            if (Backline_resolution_time__c != null) Fields.Add(Backline_resolution_time__c.ToString());
            else Fields.Add("N/A");

            if (First_Technical_Response_Time__c != null)
            {
                Fields.Add($"{First_Technical_Response_Time__c}");
            }
            else Fields.Add("N/A");

            if (Entitlement != null) Fields.Add(Entitlement.Name);
            else Fields.Add("");

            Fields.Add(Action_Next_Step__c);

            if (Action_Due__c != null) Fields.Add(Action_Due__c.ToString());
            else Fields.Add("");

            if (State__c != null) Fields.Add(State__c.ToString());
            else Fields.Add("");

            #region Repair / Calibration

            if (Received_in_Repair__c != null) Fields.Add(((DateTime)Received_in_Repair__c).ToString());
            else Fields.Add("");
            if (Time_in_Repair__c != null) Fields.Add(Time_in_Repair__c.ToString());
            else Fields.Add("");

            #endregion

            #endregion

            #region Change Request

            Fields.Add(State__c);
            if (CR_Number__c != null) Fields.Add(CR_Number__c);
            else Fields.Add("N/A");
            Fields.Add(Property__c);
            Fields.Add(Decline_Reason__c);
            Fields.Add(Jira_ID__c);

            #endregion

            return string.Join("||", Fields).Replace("\n", " ").Replace("\r", " ");
        }

        public static bool operator ==(Case a, Case b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(Case a, Case b)
        {
            return a.Id != b.Id;
        }

        public string ToJSON(bool pretty = true)
        {
            return JsonConvert.SerializeObject(this, pretty ? Formatting.Indented : Formatting.None);
        }

        public List<Case> ExceptCases(List<Case> first, List<Case> second)
        {
            // Returns a List of Cases that are not in the second list but are in the first list

            if (first.Count == 0) return new List<Case>();
            if (second.Count == 0) return first;

            List<Case> results = new List<Case>();
            List<string> secondIds = second.Select(thisCase => thisCase.Id).ToList();

            foreach (Case thisCase in first)
            {
                if (secondIds.Contains(thisCase.Id)) continue;
                results.Add(thisCase);
            }

            return results;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> FinalDict = new Dictionary<string, string>();

            FinalDict["CaseNumber"] = CaseNumber;
            FinalDict["Id"] = Id;
            FinalDict["BusinessHoursId"] = BusinessHoursId;
            FinalDict["Case Owner"] = Owner.ToString();


            return FinalDict;
        }
        #endregion
      

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
