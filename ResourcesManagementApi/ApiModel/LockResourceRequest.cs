using System.ComponentModel.DataAnnotations;

namespace ResourcesManagementApi.ApiModel;

public class LockResourceRequest
{
    [Required]
    public ResourceLockKind? LockKind { get; set; }
}
