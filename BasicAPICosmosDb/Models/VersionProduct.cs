using System.ComponentModel;
using BasicAPICosmosDb.Enums;

namespace BasicAPICosmosDb.Models
{
    public class VersionProduct : BaseModel
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? PreviousEntityId { get; set; }
        public string? Version { get; set; }
        public RepoStatus Status { get; set; }
        public List<Component>? Components { get; set; }
        public bool WillBeDeploy { get; set; }
    }
}
