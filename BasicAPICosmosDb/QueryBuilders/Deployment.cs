using Microsoft.Azure.Cosmos;

namespace BasicAPICosmosDb.QueryBuilders
{
    static class DeploymentQuery
    {
        public static QueryDefinition GenerateGetDeploymentsQuery()
        {
            var query = $@"SELECT VALUE c
                           FROM c
                           Where c.Type = @Type and c.Latest = true
                           ORDER BY c._ts DESC
                          ";

            var queryDefinition = new QueryDefinition(query)
                .WithParameter("@Type", CosmosType.DeploymentRequest);
            return queryDefinition;
        }

        public static QueryDefinition GenerateGetDeploymentByEntityIdQuery(string entityId)
        {
            var query = $@"SELECT VALUE c
                           FROM c
                           Where c.Type = @Type and c.Latest = true
                           and c.EntityId = @EntityId
                          ";

            var queryDefinition = new QueryDefinition(query)
                .WithParameter("@Type", CosmosType.DeploymentRequest)
                .WithParameter("@EntityId", entityId);
            return queryDefinition;
        }
    }
}
