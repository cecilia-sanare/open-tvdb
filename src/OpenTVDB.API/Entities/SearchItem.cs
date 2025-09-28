using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Enums;

namespace OpenTVDB.API.Entities;

[PrimaryKey(nameof(Id), nameof(Type))]
public class SearchItem
{
    [Description("The item id (series or movie id)")]
    public required Guid Id { get; set; }

    [Description("The item type")]
    public required SearchItemType Type { get; set; }

    [MaxLength(100)]
    [Description("The url slug of the item")]
    public required string Slug { get; set; }

    [MaxLength(100)]
    [Description("The title of the item")]
    public required string Title { get; set; }
}
