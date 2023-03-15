using Microsoft.Azure.Cosmos;

namespace BasicAPICosmosDb.Models
{
    public class CosmosConnection
    {
        public string Environment { get; set; }
        public CosmosClient CosmosClient { get; set; }
        public Database Database { get; set; }
    }
}
