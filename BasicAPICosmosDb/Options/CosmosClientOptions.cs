using BasicAPICosmosDb.Services;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace BasicAPICosmosDb
{
    public static class Options
    {
        public static CosmosClientOptions DefaultCosmosClientOptions
        {
            get
            {
                CosmosClientOptions cosmosClientOptions = new CosmosClientOptions();

                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                CosmosJsonNetSerializer cosmosJsonNetSerializer = new CosmosJsonNetSerializer(jsonSerializerSettings);
                cosmosClientOptions.Serializer = cosmosJsonNetSerializer;

                return cosmosClientOptions;
            }
        }
    }
}
