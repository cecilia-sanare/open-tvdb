using System.ComponentModel.DataAnnotations;
using OpenTVDB.API.Annotations;

namespace OpenTVDB.API.Entities;

public abstract class AuditEntity
{
    [SchemaIgnore]
    public DateTime? Updated { get; set; }

    [Required]
    [SchemaIgnore]
    public DateTime Created { get; set; }
}