using BasicAPICosmosDb.Enums;

namespace BasicAPICosmosDb.Models
{
    public class Deployment
    {
        public string id { get; set; }
        public string EntityId { get; set; }
        public string Tenant { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public RepoStatus VersionStatus { get; set; }
        public RepoStatus ConfigStatus { get; set; }
        public RepoStatus SchemaStatus { get; set; }
        public string? Environment { get; set; }
        public DeploymentStatus Status { get; set; }
        public string? Comment { get; set; }
        public string? DeploymentApproval { get; set; }
        public string? DevOpsApproval { get; set; }
        public string? RequestApproval { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime DeploymentDate { get; set; }
        public int CurrentTimeZone { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Latest { get; set; }
        public bool HasApprove { get; set; }
        public bool VersionCertified { get; set; }
        public DeploymentUsing DeploymentUsing { get; set; }
        public List<VersionProduct>? Products { get; set; }
        public List<Pipeline>? Pipelines { get; set; }
        public bool HasSwap { get; set; }
        public string? SwapBuildId { get; set; }
        public string? SwapStatus { get; set; }
        public string? SwapResult { get; set; }
        public DateTime SwappedDate { get; set; }
        public string? SwappedBy { get; set; }
    }


    public class Pipeline
    {


        public string Project { get; set; }
        public string DefinitionId { get; set; }
        public string Name { get; set; }
        public string AppName { get; set; }
        public DeploymentUsing DeploymentUsing { get; set; }
        public string Branch { get; set; }
        public string Version { get; set; }
        public string Repo { get; set; }
        public string Url { get; set; }
        public string BuildId { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string CommitId { get; set; }
        public string CommitUrl { get; set; }
        public string Environment { get; set; }
        public string FilePath { get; set; }

        public Pipeline()
        {
        }
        
        public Pipeline(Pipeline input)
        {
            DefinitionId = input.DefinitionId;
            Name = input.Name;
            Branch = input.Branch;
            Version = input.Version;
            Repo = input.Repo;
            Url = input.Url;
            BuildId = input.BuildId;
            Status = input.Status;
            Result = input.Result;
        }
    }
    public class InsertDeploymentOutput
    {
        public string Id { get; set; }
    }

    public class GetDeploymentsInput
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string EnvFilter { get; set; }
        public string ServiceFilter { get; set; }
        public string StatusFilter { get; set; }
        public string UserFilter { get; set; }
        public bool CheckStatus { get; set; }
        public string VersionFilter { get; set; }
        public string ContentFilter { get; set; }
    }


    public class GetDeploymentsOutput
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public Deployment[] Values { get; set; }

        public GetDeploymentsOutput() { }
        public GetDeploymentsOutput(IList<Deployment> values) 
        {
            Values = values.ToArray();
        }


    }

}
