using Newtonsoft.Json;
using SalesforceEngine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceEngine.Dashboards
{
    public static class APACInternal
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
            string Rule4 = "(Owner.Name LIKE '%Backline%')";
            string Rule5 = "(Account.Name LIKE 'PAPT Spirent Backline%')";

            return $"SELECT Id FROM Case WHERE ({Rule1} AND {Rule2} AND {Rule3} AND {Rule4}) AND {Rule5}";
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
