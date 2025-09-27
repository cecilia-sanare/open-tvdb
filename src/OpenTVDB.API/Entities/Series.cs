using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTVDB.API.Entities;

public class Series : AuditEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }

    [MaxLength(100)]
    public required string Title { get; set; }
}
