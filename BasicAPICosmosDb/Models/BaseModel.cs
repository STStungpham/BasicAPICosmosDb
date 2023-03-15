using System;

namespace BasicAPICosmosDb.Models
{
    public class BaseModel
    {
        public string? id { get; set; }
        public string? EntityId { get; set; }     
        public string? Type { get; set; }
        public bool? Latest { get; set; }
        public string? Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? LastModifyBy { get; set; }
        public DateTime? LastModify { get; set; }
    }
}
