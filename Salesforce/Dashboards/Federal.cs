using Newtonsoft.Json;
using SalesforceEngine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Dashboards
{
    public static class Federal
    {
        public static int NumCases;

        public static int Update()
        {
            string Query = GenerateQuery();
            List<Case> Cases = QueryCases(Query);
            NumCases = Cases.Count;
            return NumCases;
        }

        public static async Task<int> UpdateAsync()
        {
            string Query = GenerateQuery();
            List<Case> Cases = await QueryCasesAsync(Query);
            NumCases = Cases.Count;
            return NumCases;
        }

        private static string GenerateQuery()
        {
            string Rule1 = "(Status != 'Closed' AND Status != 'Cancelled')";
            string Rule2 = "(RecordType.Name = 'Service Request' OR RecordType.Name = 'Service Request - Locked')";
            string Rule3 = "(Product_Line__c LIKE 'PT%')";
            string Rule4 = "(Owner.Name = 'Backline Americas Queue')";
            string Rule5 = "(RecordType.Name = 'Scenario Request Special 6300M' OR RecordType.Name = 'Scenario Request Special 6300M LOCKED' OR RecordType.Name = 'Scenario Request Special Spacecraft' OR " +
                            "RecordType.Name = 'Scenario Request Special Spacecraft LOCKED' OR RecordType.Name = 'Scenario Request Special Terrestrial' OR RecordType.Name = 'Scenario Request Special Terrestrial LOCKED')";
            string Rule6 = "(Owner.Name = 'PT Scenario Requests')";
            string Rule7 = "(Account_Region__c = 'Americas')";
            string Rule8 = "(Account_PT_Federal__c = TRUE)";
            string Rule9 = "(Owner.Name = 'Backline Federal Queue')";
            string Rule10 = "(NOT (Account_Name_ReadOnly__c LIKE '%PAPT Spirent Backline%'))";

            return $"SELECT Id FROM Case WHERE ({Rule1} AND {Rule8} AND (({Rule2} AND {Rule3} AND ({Rule4} OR {Rule9})) OR ({Rule5} AND {Rule6} AND {Rule7}))) AND {Rule10}";
        }

        private static List<Case> QueryCases(string Query)
        {
            string EngineResponse = Salesforce.Query(Query);
            return JsonConvert.DeserializeObject<List<Case>>(EngineResponse);
        }

        private static async Task<List<Case>> QueryCasesAsync(string Query)
        {
            string EngineResponse = await Salesforce.QueryAsync(Query);
            return JsonConvert.DeserializeObject<List<Case>>(EngineResponse);
        }
    }
}
