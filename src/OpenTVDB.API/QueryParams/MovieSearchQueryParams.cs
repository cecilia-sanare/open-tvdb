using System.ComponentModel;

namespace OpenTVDB.API.QueryParams;

public class MovieSearchQueryParams
{
    [Description("The title of the movie")]
    public string? Query { get; set; }
}
