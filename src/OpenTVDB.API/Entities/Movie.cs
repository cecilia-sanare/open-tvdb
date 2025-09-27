using System.ComponentModel.DataAnnotations.Schema;
using NJsonSchema.Annotations;

namespace OpenTVDB.API.Entities;

public class Movie : AuditEntity
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid? Id { get; set; }

  [JsonSchemaIgnore]
  public string? Title { get; set; }
}
