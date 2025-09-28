using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OpenTVDB.API.Extensions;

// Untestable without setting up multiple databases
[ExcludeFromCodeCoverage]
public static class MaterializedViewExtensions
{
    public static async Task RefreshMaterializedView(this DbContext context, string table)
    {
        if (context.Database.ProviderName != "Npgsql.EntityFrameworkCore.PostgreSQL") return;

        await context.Database.ExecuteSqlRawAsync($"REFRESH MATERIALIZED VIEW CONCURRENTLY View_{table};");
    }
}
