using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTVDB.API.Entities;

public class Series : AuditEntity
{
    [Description("The unique identifier of the series")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }

    [MaxLength(100)]
    [Description("The title of the series")]
    public required string Title { get; set; }

    [MaxLength(100)]
    [Description("The slug of the series")]
    public required string Slug { get; set; }
}
