
namespace BasicAPICosmosDb.Enums
{
    public enum DeploymentStatus
    {
        RequestPending = 0,
        ResourcePending = 2,
        DeploymentReview = 4,
        DeploymentApproved = 256,
        Rejected = 8,
        Running = 16,
        NeedUpdate = 32,
        DeployPending = 64,
        Done = 128,
        Failed = 512,
        SwapPending = 1024,
        Swapping = 2048
    }

    public enum RepoStatus
    {
        Missing = 0,
        OK = 1
    }

    public enum ResponseStatus
    {
        Error = 0,
        OK = 1,
        Warning = 2
    }

    public enum DeploymentUsing
    {
        Tag = 0,
        Branch = 1
    }


}
