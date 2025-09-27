using System.ComponentModel;

namespace OpenTVDB.API.QueryParams;

public class SeriesSearchQueryParams
{
    [Description("The title of the series")]
    public string? Query { get; set; }
}
