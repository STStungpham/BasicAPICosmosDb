using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAPICosmosDb
{
    public static class Utils
    {
        public static string GetQueryParameters(QueryDefinition query)
        {
            string result = "null";
            var parameters = query.GetQueryParameters();
            if (parameters?.Count > 0)
            {
                result = JsonConvert.SerializeObject(parameters);
            }
            return result;
        }

        public static string GetErrorQueryMessage(QueryDefinition query, double requestCharge)
        {
            return $"Error query : {query.QueryText}\nRequest charge : {requestCharge } RUs\nParameters: {GetQueryParameters(query)}";
        }

        public static string GetErrorStoreProcedureMessage(string storeId, dynamic[] items)
        {
            return $"Error store : {storeId}\nRequest charge : N/A RUs\nParameters: {items}";
        }

        public static string GetQueryMessage(QueryDefinition query, double requestCharge)
        {
            return $"Cosmos query : {query.QueryText}\nRequest charge : {requestCharge } RUs\nParameters: {GetQueryParameters(query)}";
        }

        public static string GetExMessage(Exception ex, string className, string methodName)
        {
            return $@"{className}.{methodName}: {ex.Message} {ex.StackTrace}";
        }
    }
}
