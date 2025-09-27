using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTVDB.API.Entities;

public class Movie : AuditEntity
{
    [Description("The unique identifier of the movie")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }

    [MaxLength(100)]
    [Description("The title of the movie")]
    public required string Title { get; set; }
}
