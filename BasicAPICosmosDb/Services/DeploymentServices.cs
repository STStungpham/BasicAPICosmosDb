using BasicAPICosmosDb.Models;
using BasicAPICosmosDb.QueryBuilders;

namespace BasicAPICosmosDb.Services
{
    #region interface
    public interface IDeploymentServices
    {
        public Task<ListResponse<Deployment>> GetDeploymentsAsync();    
        public Task<Response> InsertDeploymentAsync(Deployment input);
        public Task<Response> UpdateDeploymentAsync(Deployment input);
        public Task<Response> GetDeploymentByEntityIdAsync(string EntityId);
        public Task<Response> DeleteDeploymentRequestAsync(string entityId);
    }
    #endregion

    #region implement
    public class DeploymentsServices : IDeploymentServices
    {
        private readonly ICosmosDbServices _cosmosServices;
        public DeploymentsServices(ICosmosDbServices cosmosServices)
        {
            _cosmosServices = cosmosServices;
        }

        public async Task<Response> DeleteDeploymentRequestAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetDeploymentByEntityIdAsync(string EntityId)
        {
            var rst = new Response();
            var query = DeploymentQuery.GenerateGetDeploymentByEntityIdQuery(EntityId);
            var res = await _cosmosServices.ReadItemsByQueryAsync<Deployment>(Containers.deployment, query, string.Empty, null, false);
            rst.Result = res.Result.Values.FirstOrDefault();
            rst.RequestCharge = res.RequestCharge;
            return rst;
        }

        public async Task<ListResponse<Deployment>> GetDeploymentsAsync()
        {
            var query = DeploymentQuery.GenerateGetDeploymentsQuery();
            return await _cosmosServices.ReadItemsByQueryAsync<Deployment>(
                Containers.deployment, query, string.Empty);          
        }

        public async Task<Response> InsertDeploymentAsync(Deployment input)
        {
            Response rst = new Response();
            rst.Result = await _cosmosServices.CreateItemAsync<object>(
                Containers.deployment, input, input.Type);
            return rst;
        }

        public async Task<Response> UpdateDeploymentAsync(Deployment input)
        {
            Response rst = new Response();
            rst.Result = await _cosmosServices.UpsertItemAsync<object>(
                Containers.deployment, input, input.Type);
            return rst;
        }
    }
    #endregion
}
