using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesforceEngine.Objects;

namespace SalesforceEngine
{
    public static class Salesforce
    {
        #region Objects And Variables

        private static StreamWriter EngineWriter;
        private static readonly Process SFEngine = new Process();
        private static readonly ProcessStartInfo SFEngineInfo = new ProcessStartInfo()
        {
            FileName = @"C:\Windows\system32\cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        private static readonly Dictionary<string, string> AutoResponses = new Dictionary<string, string>()
        {
            { "Enter Username:", "PTDirect@spirent.com" },
            { "Enter Password:", "SPTscengen6" },
            { "Enter AuthToken:", "deZzgQviZrQSAMiHX5O0y9QdC" }
        };

        private static bool EngineBusy = true;
        private static bool EngineResponded = false;
        private static string LatestJSON;
        private static readonly string EnginePath = AppDomain.CurrentDomain.BaseDirectory + "Salesforce.py";
        private static Dictionary<string, string> QueuesIDs = new Dictionary<string, string>
        {
            {"EMEA", "00Oo0000004nZgXEAU" },
            {"Americas", "00Oo0000004nZi4EAE" },
            {"Federal", "00Oo0000004nZi9EAE" },
            {"APAC - Non Restricted", "00Oo0000004nZhkEAE"}, 
            {"APAC - Restricted", "00Oo0000004nZhzEAE"},
            {"Internal Queues",  "00Oo0000005vS1OEAU"},
            {"Internal Federal Queues", "00O5c000007xnOWEAY"},
        };

        #endregion

        #region Core Functions

        public static void StartEngine()
        {
            InitialiseEngineProcess();
            StartEngineReading();
            InitialiseEngineWriter();

            if (!File.Exists(EnginePath))
            {
                throw new Exception("Salesforce.py needs to be added to added to the same folder as caling .exe");
            }

            // ToDo: Find latest python version
            //EngineWriter.WriteLine($"cd {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Programs\\Python\\Python37");
            //EngineWriter.WriteLine($"cd {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Programs\\Python\\Python37");
            EngineWriter.WriteLine($"python \"{EnginePath}\"");
            
            WaitForEngineAvailability();
        }

        public static async Task<string> StartEngineAsync()
        {
            InitialiseEngineProcess();
            StartEngineReading();
            InitialiseEngineWriter();

            if (!File.Exists(EnginePath))
            {
                throw new Exception("Salesforce.py needs to be added to added to the same folder as caling .exe");
            }

            EngineWriter.WriteLine($"cd {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}");
            EngineWriter.WriteLine($"python.exe \"{EnginePath}\"");
            
            await WaitForEngineAvailabilityAsync();

            return "";
        }

        private static void InitialiseEngineProcess()
        {
            SFEngine.StartInfo = SFEngineInfo;
            SFEngine.OutputDataReceived += (sender2, e2) => EngineOutputTextReceieved(e2.Data);
            SFEngine.ErrorDataReceived += (sender3, e3) => EngineErrorTextReceived(e3.Data);
        }

        private static void StartEngineReading()
        {
            SFEngine.Start();
            SFEngine.BeginOutputReadLine();
        }

        private static void InitialiseEngineWriter()
        {
            EngineWriter = SFEngine.StandardInput;
        }

        private static void EngineOutputTextReceieved(string Text)
        {
            if (!string.IsNullOrEmpty(Text)) ProcessEngineOutput(Text);
        }

        private static void ProcessEngineOutput(string Text)
        {
            if (AutoResponses.ContainsKey(Text)) AutoRespond(Text);
            else if (Text == "Enter Command:") SetEngineToNotBusy();
            else if (Text == "Connected") OnEngineConnected?.Invoke(SFEngine, EventArgs.Empty);
            else if (Text.StartsWith("{") || Text.StartsWith("[")) SetLatestJSON(Text);
            else HandleError(Text);
        }

        private static void AutoRespond(string Text)
        {
            EngineWriter.WriteLine(AutoResponses[Text]);
        }

        private static void SetEngineToNotBusy()
        {
            EngineBusy = false;
        }

        private static void SetEngineToBusy()
        {
            EngineBusy = true;
            EngineResponded = false;
        }

        private static void SetLatestJSON(string Text)
        {
            LatestJSON = Text;
            EngineResponded = true;
        }

        private static void HandleError(string Text)
        {
            List<string> ExpectedMessages = new List<string>()
            {
                "Connecting To Salesforce...",
                "Connected"
            };
            if (!Text.StartsWith("Microsoft") && 
                !Text.StartsWith("(c)") &&
                !Text.StartsWith("C:\\") &&
                !Text.StartsWith("Querying") &&
                !Text.StartsWith("Getting") &&
                !Text.StartsWith("Running") &&
                !ExpectedMessages.Contains(Text))
            {
                throw new Exception("Engine Output Data Format Not Recognised: " + Text);
            }
        }

        private static void EngineErrorTextReceived(string Text)
        {

        }

        private static void WriteToEngine(string Message)
        {
            SetEngineToBusy();
            EngineWriter.WriteLine(Message);
        }

        private static void WaitForEngineAvailability()
        {
            while (EngineBusy) Thread.Sleep(10);
        }

        private static async Task<string> WaitForEngineAvailabilityAsync()
        {
            while (EngineBusy) await Task.Delay(10);
            return "";
        }

        private static void WaitForEngineResponse()
        {
            while (!EngineResponded) Thread.Sleep(10);
        }

        private static async Task<string> WaitForEngineResponseAsync()
        {
            while (!EngineResponded) await Task.Delay(10);
            return "";
        }

        #endregion

        #region Events

        public static event EventHandler OnEngineConnected;

        #endregion

        #region Public Functions

        public static string BuildQueryFromParts(List<string> FieldsToReturn, string ObjectType, List<string> FilterCriteria)
        {
            return $"SELECT {string.Join(", ", FieldsToReturn)} FROM {ObjectType} WHERE {string.Join(" AND ", FilterCriteria)}";
        }

        public static string ConvertDateToSalesforceFormat(DateTime InputDateTime)
        {
            return InputDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }

        public static void SaveCasesToFile(string FilePath, List<Case> CasesToSave)
        {
            List<string> ColumnHeaders = new List<string>()
            {
                "ID",
                "Parent ID",
                "Case Number",
                "Subject",              
                "Status",
                "Serial",
                "Account Name",
                "Account Region",
                "Created Date",
                "Created Month",
                "Closed Date",
                "Closed Month",
                "Case Record Type",
                "Case Owner",
                "Backline Resolution Time",
                "First Technical Response Time",
                "Entitlement Name",
                "Action Next Step",
                "Action Due",
                "State",
                "Received In Repair",
                "Time In Repair (hours)",
                "CR State",
                "CR Number",    // Added this
                "CR Property",
                "CR Decline Reason",
                "Jira ID"
        };
            List<string> FileLines = new List<string>()
            {
                string.Join("||", ColumnHeaders)
            };
            foreach(Case ThisCase in CasesToSave) FileLines.Add(ThisCase.ToString());
            File.WriteAllLines(FilePath, FileLines);
        }

        #endregion

        #region Engine Commands

        #region Synchronous Commands - Use In Console Applications

        public static void SendCommand(string Command)
        {
            WaitForEngineAvailability();
            WriteToEngine(Command);
            WaitForEngineResponse();
        }

        public static string Query(string Query)
        {
            SendCommand($"Query||{Query}");
            return LatestJSON;
        }

        public static string QueryAll(string Query)
        {
            SendCommand($"QueryAll||{Query}");
            return LatestJSON;
        }

        public static string RunReport(string ReportID)
        {
            SendCommand($"RunReport||{ReportID}");
            return LatestJSON;
        }

        #endregion

        #region Asynchronous Commands - Use In GUI Applications

        public static async Task<string> SendCommandAsync(string Command)
        {
            await WaitForEngineAvailabilityAsync();
            WriteToEngine(Command);
            await WaitForEngineResponseAsync();
            return "";
        }

        public static async Task<string> QueryAsync(string Query)
        {
            await SendCommandAsync($"Query||{Query}");
            return LatestJSON;
        }

        public static async Task<string> QueryAllAsync(string Query)
        {
            await SendCommandAsync($"QueryAll||{Query}");
            return LatestJSON;
        }

        public static async Task<string> RunReportAsync(string ReportID)
        {
            await SendCommandAsync($"RunReport||{ReportID}");
            return LatestJSON;
        }

        #endregion

        #endregion

        #region Get Functions

        #region Case

        #region Synchronous Functions - Use In Console Applications

        public static RecordType GetAllRecordTypeDetails(string RecordTypeID)
        {
            SendCommand($"GetObject||RecordType||{RecordTypeID}");
            RecordType thisRecordType = JsonConvert.DeserializeObject<RecordType>(LatestJSON);
            return thisRecordType;
        } 

        public static Case GetAllCaseDetails(string CaseID)
        {
            Console.WriteLine($"Getting Case Details for {CaseID}");
            SendCommand($"GetObject||Case||{CaseID}");
            Case thisCase = JsonConvert.DeserializeObject<Case>(LatestJSON);

            if (!string.IsNullOrEmpty(thisCase.ParentId))
            {
                thisCase.Parent = GetAllCaseDetails(thisCase.ParentId);
            }

            List<string> QueuesRecordTypeIDs = new List<string>()
            {
                "012o0000000y1PaAAI"
            };

            if (!string.IsNullOrEmpty(thisCase.RecordTypeId) && !QueuesRecordTypeIDs.Contains(thisCase.RecordTypeId))
            {
                SendCommand($"GetObject||RecordType||{thisCase.RecordTypeId}");
                thisCase.RecordType = JsonConvert.DeserializeObject<RecordType>(LatestJSON);
            }

            List<string> QueuesOwnerIDs = new List<string> ()
            {
                "00Go0000001rVdLEAU",
                "00Go0000001rVdIEAU"
            };

            if (!string.IsNullOrEmpty(thisCase.OwnerId) && !QueuesOwnerIDs.Contains(thisCase.OwnerId))
            {
                SendCommand($"GetObject||User||{thisCase.OwnerId}");
                thisCase.Owner = JsonConvert.DeserializeObject<User>(LatestJSON);
            }

            return thisCase;
        }

        public static Case GetAllCaseDetails(Case IncompleteCase)
        {
            return GetAllCaseDetails(IncompleteCase.Id);
        }

        public static List<Case> GetAllCaseDetails(List<Case> IncompleteCases)
        {
            List<Case> CompletedCases = new List<Case>();
            foreach (Case IncompleteCase in IncompleteCases)
            {
                CompletedCases.Add(GetAllCaseDetails(IncompleteCase.Id));
            }

            return CompletedCases;
        }

        public static List<Case> GetUserOpenCases(string UserName)
        {
            Query($"SELECT Id FROM Case WHERE Owner.Name = '{UserName}' AND (Status = 'New' OR Status = 'Open') ORDER BY CaseNumber ASC");
            List<Case> IncompleteCases = JsonConvert.DeserializeObject<List<Case>>(LatestJSON);

            return GetAllCaseDetails(IncompleteCases);
        }

        public static List<Case> GetUserClosedCases(string UserName)
        {
            Query($"SELECT Id FROM Case WHERE Owner.Name = '{UserName}' AND Status = 'Closed'");
            List<Case> IncompleteCases = JsonConvert.DeserializeObject<List<Case>>(LatestJSON);

            return GetAllCaseDetails(IncompleteCases);
        }

        #endregion

        #region Asynchronous Functions - Use In GUI Applications

        public static async Task<Entitlement> GetAllEntitlementDetailsAsync(string EntitlementID)
        {
            await SendCommandAsync($"GetObject||Entitlement||{EntitlementID}");
            return JsonConvert.DeserializeObject<Entitlement>(LatestJSON);
        }

        public static async Task<User> GetAllUserDetailsAsync(string UserID)
        {
            await SendCommandAsync($"GetObject||User||{UserID}");
            return JsonConvert.DeserializeObject<User>(LatestJSON);
        }

        public static async Task<Contact> GetAllContactDetailsAsync(string ContactID)
        {
            await SendCommandAsync($"GetObject||Contact||{ContactID}");
            return JsonConvert.DeserializeObject<Contact>(LatestJSON);
        }

        public static async Task<Account> GetAllAccountDetailsAsync(string AccountID)
        {
            await SendCommandAsync($"GetObject||Account||{AccountID}");
            return JsonConvert.DeserializeObject<Account>(LatestJSON);
        }

        public static async Task<RecordType> GetAllRecordTypeDetailsAsync(string RecordTypeID)
        {
            await SendCommandAsync($"GetObject||RecordType||{RecordTypeID}");
            return JsonConvert.DeserializeObject<RecordType>(LatestJSON);
        }

        public static async Task<Asset> GetAllAssetDetailsAsync(string AssetID)
        {
            await SendCommandAsync($"GetObject||Asset||{AssetID}");
            return JsonConvert.DeserializeObject<Asset>(LatestJSON);
        }

        public static async Task<Case> GetAllCaseDetailsAsync(string CaseID)
        {
            await SendCommandAsync($"GetObject||Case||{CaseID}");
            return JsonConvert.DeserializeObject<Case>(LatestJSON);
        }

        public static async Task<Case> GetAllCaseDetailsAsync(Case IncompleteCase)
        {
            await SendCommandAsync($"GetObject||Case||{IncompleteCase.Id}");
            return JsonConvert.DeserializeObject<Case>(LatestJSON);
        }

        public static async Task<List<Case>> GetAllCaseDetailsAsync(List<Case> IncompleteCases)
        {
            List<Case> CompleteCases = new List<Case>();
            foreach (Case IncompleteCase in IncompleteCases)
            {
                CompleteCases.Add(await GetAllCaseDetailsAsync(IncompleteCase));
            }

            return CompleteCases;
        }

        public static async Task<List<Case>> GetUserOpenCasesAsync(string UserName)
        {
            await QueryAsync($"SELECT Id FROM Case WHERE Owner.Name = '{UserName}' AND (Status = 'New' OR Status = 'Open')");
            List<Case> IncompleteCases = JsonConvert.DeserializeObject<List<Case>>(LatestJSON);

            return await GetAllCaseDetailsAsync(IncompleteCases);
        }

        public static async Task<List<Case>> GetUserClosedCasesAsync(string UserName)
        {
            await QueryAsync($"SELECT Id FROM Case WHERE Owner.Name = '{UserName}' AND Status = 'Closed'");
            List<Case> IncompleteCases = JsonConvert.DeserializeObject<List<Case>>(LatestJSON);

            return await GetAllCaseDetailsAsync(IncompleteCases);
        }

        public static string GetPrettyLatestJSON()
        {
            JToken parsedJson = JToken.Parse(LatestJSON);
            string prettyLatestJSON = parsedJson.ToString(Formatting.Indented);
            return prettyLatestJSON;
        }

        #endregion

        #endregion

        #region Entitlement

        public static Entitlement GetEntitlement(string EntitlementID)
        {
            SendCommand($"GetObject||Entitlement||{EntitlementID}");
            return JsonConvert.DeserializeObject<Entitlement>(LatestJSON);
        }

        public static async Task<Entitlement> GetEntitlementAsync(string EntitlementID)
        {
            await SendCommandAsync($"GetObject||Entitlement||{EntitlementID}");
            return JsonConvert.DeserializeObject<Entitlement>(LatestJSON);
        }

        #endregion

        #region KBArticle

        public static KBArticle GetKBArticle(string ArticleID)
        {
            SendCommand($"GetObject||KBArticle||{ArticleID}");
            return JsonConvert.DeserializeObject<KBArticle>(LatestJSON);
        }

        public static async Task<KBArticle> GetKBArticleAsync(string ArticleID)
        {
            await SendCommandAsync($"GetObject||KBArticle||{ArticleID}");
            return JsonConvert.DeserializeObject<KBArticle>(LatestJSON);
        }

        #endregion

        #endregion
    }
}