using Microsoft.Azure.Cosmos;
using BasicAPICosmosDb.Models;
using Polly;
using System.Net;
using Microsoft.JSInterop.Implementation;

namespace BasicAPICosmosDb.Services
{
    #region interfaces
    public interface ICosmosDbServices
    {
        Task<ListResponse<T>> ReadItemsByQueryAsync<T>(string containerName, QueryDefinition query, string partitionKey, ILogger logger = null, bool applyRetryPolicy = true);
        Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action, Action<CosmosException> retryAction = null);
        Task<object> UpsertItemAsync<T>(string containerName, T item, string partitionKey, bool applyRetryPolicy = true);
        Task<object> CreateItemAsync<T>(string containerName, T item, string partitionKey, bool applyRetryPolicy = true);
        Task<IList<T>> ExecuteStoreProcedureAsync<T>(string storeId, string containerName, string partitionKey, dynamic[] items, ILogger logger = null, bool applyRetryPolicy = true);

    }
    #endregion

    #region implements
    public class CosmosDbServices : ICosmosDbServices
    {
        #region declare
        private ILogger _logger;
        #endregion

        #region public
        public CosmosDbServices(ILogger logger)
        {
            _logger = logger;
        }
        private Container GetContainer(string containerName)
        {
            var connection = GetConnection();
            return connection.Database.GetContainer(containerName);
        }


        public async Task<object> UpsertItemAsync<T>(string containerName, 
            T item, string partitionKey, bool applyRetryPolicy = true)
        {
            Container container = GetContainer(containerName);
            var rst = applyRetryPolicy
                ? await ExecuteWithRetryAsync(() => 
                container.UpsertItemAsync(item, 
                new PartitionKey(partitionKey)))
                : await container.UpsertItemAsync(item, 
                new PartitionKey(partitionKey));

            _logger?.LogInformation($"Upsert item request Charge : " +
                $"{rst.RequestCharge} RUs");
            return rst;
        }

        public async Task<object> UpsertItemAsync<T>(string containerName,
            T item, string partitionKey)
        {
            Container container = GetContainer(containerName);
            var rst = await container.UpsertItemAsync(item,
                new PartitionKey(partitionKey));

            _logger?.LogInformation($"Upsert item request Charge : " +
                $"{rst.RequestCharge} RUs");
            return rst;
        }



        public async Task<object> CreateItemAsync<T>(string containerName, 
            T item, string partitionKey, bool applyRetryPolicy = true)
        {
            Container container = GetContainer(containerName);
            var rst = applyRetryPolicy
                ? await ExecuteWithRetryAsync(() => 
                container.CreateItemAsync(item, 
                new PartitionKey(partitionKey)))
                : await container.CreateItemAsync(item, 
                new PartitionKey(partitionKey));
            _logger?.LogInformation($"Create item request Charge : " +
                $"{rst.RequestCharge} RUs");
            return rst;
        }

        public async Task<object> CreateItemAsync<T>(string containerName,
            T item, string partitionKey)
        {
            Container container = GetContainer(containerName);
            var rst = await container.CreateItemAsync(item,
                new PartitionKey(partitionKey));
            _logger?.LogInformation($"Create item request Charge : " +
                $"{rst.RequestCharge} RUs");
            return rst;
        }



        public async Task<object> DeleteItemAsync<T>(string containerName,
            string id, string partitionKey)
        {
            Container container = GetContainer(containerName);
            var rst = await container.DeleteItemAsync<T>(id,
                new PartitionKey(partitionKey));
            _logger?.LogInformation($"Delete item request Charge : " +
                $"{rst.RequestCharge} RUs");
            return rst;
        }

        public async Task<ListResponse<T>> ReadItemsByQueryAsync<T>(string containerName,
            QueryDefinition query, string partitionKey, ILogger logger = null, bool applyRetryPolicy = true)
        {
            var requestCharge = Constants.ZeroInDouble;
            try
            {
                if (logger != null)
                    _logger = logger;
                if (applyRetryPolicy)
                    return await ExecuteWithRetryAsync(() => ReadItemByQueryInternalAsync<T>(containerName, query, partitionKey, requestCharge));
                else
                    return await ReadItemByQueryInternalAsync<T>(containerName, query, partitionKey, requestCharge);

            }
            catch (Exception ex)
            {
                _logger?.LogError(Utils.GetErrorQueryMessage(query, requestCharge));
                _logger?.LogError(ex, Utils.GetExMessage(ex, GetType().Name, nameof(ReadItemsByQueryAsync)));
                throw;
            }
        }
        public async Task<IList<T>> ExecuteStoreProcedureAsync<T>(string storeId,
            string containerName, string partitionKey, dynamic[] items, ILogger logger = null, bool applyRetryPolicy = true)
        {
            var requestCharge = Constants.ZeroInDouble;
            try
            {
                if (logger != null)
                    _logger = logger;

                if (applyRetryPolicy)
                    return await ExecuteWithRetryAsync(() => ExecuteStoreProcedureInternalAsync<T>(storeId, containerName, partitionKey, requestCharge, items));
                else
                    return await ExecuteStoreProcedureInternalAsync<T>(storeId, containerName, partitionKey, requestCharge, items);

            }
            catch (Exception ex)
            {
                _logger?.LogError(Utils.GetErrorStoreProcedureMessage(storeId, items));
                _logger?.LogError(ex, Utils.GetExMessage(ex, GetType().Name, nameof(ExecuteStoreProcedureAsync)));
                throw;
            }
        }

        public Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action, Action<CosmosException> retryAction = null)
        {
            return Policy
                .Handle<CosmosException>(e => e.StatusCode == HttpStatusCode.TooManyRequests)
                .RetryForeverAsync(onRetry: async exception =>
                {
                    var ex = exception as CosmosException;

                    retryAction?.Invoke(ex);

                    await Task.Delay(ex?.RetryAfter ?? TimeSpan.FromMilliseconds(500));
                })
                .ExecuteAsync(action);
        }
        #endregion



        #region private

        private async Task<List<T>> ReadItemByQueryInternalAsync<T>(
            string containerName,
            QueryDefinition query)
        {
            Container container = GetContainer(containerName);
            var feedIterator = container.GetItemQueryIterator<T>(query);
            var requestCharge = Constants.ZeroInDouble;
            var result = new List<T>();
            while (feedIterator.HasMoreResults)
            {
                var feedIteratorResult = await feedIterator.ReadNextAsync();
                requestCharge += feedIteratorResult.RequestCharge;
                result.AddRange(feedIteratorResult);
            }
            _logger?.LogInformation($"Cosmos query : {query.QueryText}\n" +
                $"Request Charge : {requestCharge} RUs\nParameters: " +
                $"{Utils.GetQueryParameters(query)}");
            return result;
        }


        private async Task<ListResponse<T>> ReadItemByQueryInternalAsync<T>(string containerName,
            QueryDefinition query, string partitionKey, double requestCharge)
        {

            Container container = GetContainer(containerName);

            FeedIterator<T> feedIterator;

            if (!string.IsNullOrEmpty(partitionKey))
            {
                feedIterator = container.GetItemQueryIterator<T>(
                query,
                null,
                new QueryRequestOptions() { PartitionKey = new PartitionKey(partitionKey) });
            }
            else
            {
                feedIterator = container.GetItemQueryIterator<T>(query);
            }

            var result = new List<T>();
            while (feedIterator.HasMoreResults)
            {
                var feedIteratorResult = await feedIterator.ReadNextAsync();
                requestCharge += feedIteratorResult.RequestCharge;
                result.AddRange(feedIteratorResult);
            }
            _logger?.LogInformation($"Cosmos query : {query.QueryText}\nRequest Charge : {requestCharge} RUs\nParameters: {Utils.GetQueryParameters(query)}");

            var rst = new ListResponse<T>(result);
            rst.RequestCharge = requestCharge;
            return rst;
        }

        private CosmosConnection GetConnection()
        {
            var rst = new CosmosConnection
            {
                Environment = AppSettings.Environment,
                CosmosClient = new CosmosClient(AppSettings.ConnectionString, 
                                     Options.DefaultCosmosClientOptions)
            };
            rst.Database = rst.CosmosClient.GetDatabase(AppSettings.DatabaseName);
            return rst;
        }

        private async Task<List<T>> ExecuteStoreProcedureInternalAsync<T>(string storeId,
            string containerName, string partitionKey, double requestCharge, dynamic[] newItems)
        {

            Container container = GetContainer(containerName);
            return await container.Scripts.ExecuteStoredProcedureAsync<List<T>>(storeId, new PartitionKey(partitionKey), new[] { newItems });
        }
        #endregion
    }
    #endregion
}
