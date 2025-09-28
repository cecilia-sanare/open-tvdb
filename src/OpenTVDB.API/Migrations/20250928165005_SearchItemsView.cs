using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTVDB.API.Migrations
{
    /// <inheritdoc />
    public partial class SearchItemsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder builder)
        {
            if (builder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                builder.Sql(@"
                    CREATE VIEW View_SearchItems
                    WITH SCHEMABINDING
                    AS (
                        SELECT Id, 0 AS Type, Slug, Title
                        FROM Movies
                        UNION ALL
                        SELECT Id, 1 AS Type, Slug, Title
                        FROM Series
                    );

                    CREATE UNIQUE CLUSTERED INDEX IX_View_SearchItems_Title
                    ON dbo.View_SearchItems (Title);
                ");
            }
            else if (builder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                // PostgreSQL: Materialized view (manual refresh required)
                builder.Sql(@"
                    CREATE MATERIALIZED VIEW View_SearchItems
                    AS (
                        SELECT Id, 0 AS Type, Slug, Title
                        FROM Movies
                        UNION ALL
                        SELECT Id, 1 AS Type, Slug, Title
                        FROM Series
                    );

                    CREATE UNIQUE INDEX IX_View_SearchItems_Id_Type
                    ON View_SearchItems (Id, Type);

                    CREATE INDEX IX_View_SearchItems_Title
                    ON View_SearchItems (Title);
                ");
            }
            else if (builder.ActiveProvider == "Oracle.EntityFrameworkCore")
            {
                // Oracle: Materialized view with immediate build and manual refresh
                builder.Sql(@"
                    CREATE MATERIALIZED VIEW View_SearchItems
                    BUILD IMMEDIATE
                    REFRESH COMPLETE ON DEMAND
                    AS (
                        SELECT Id, 0 AS Type, Slug, Title
                        FROM Movies
                        UNION ALL
                        SELECT Id, 1 AS Type, Slug, Title
                        FROM Series
                    );

                    CREATE INDEX IX_View_SearchItems_Title
                    ON View_SearchItems (Title);
                ");
            }
            else
            {
                // Fallback: Create normal view for other providers (MySQL, SQLite, etc.)
                builder.Sql(@"
                    CREATE VIEW View_SearchItems AS
                    SELECT Id, 0 AS Type, Slug, Title
                    FROM Movies
                    UNION ALL
                    SELECT Id, 1 AS Type, Slug, Title
                    FROM Series;
                ");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
                {
                    migrationBuilder.Sql("DROP INDEX IX_View_SearchItems_Title ON dbo.View_SearchItems;");
                    migrationBuilder.Sql("DROP VIEW dbo.View_SearchItems;");
                }
                else if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
                {
                    migrationBuilder.Sql("DROP INDEX IX_View_SearchItems_Id_Type ON dbo.View_SearchItems;");
                    migrationBuilder.Sql("DROP INDEX IX_View_SearchItems_Title ON dbo.View_SearchItems;");
                    migrationBuilder.Sql("DROP MATERIALIZED VIEW View_SearchItems;");
                }
                else if (migrationBuilder.ActiveProvider == "Oracle.EntityFrameworkCore")
                {
                    migrationBuilder.Sql("DROP INDEX IX_View_SearchItems_Title ON dbo.View_SearchItems;");
                    migrationBuilder.Sql("DROP MATERIALIZED VIEW View_SearchItems;");
                }
                else
                {
                    migrationBuilder.Sql("DROP VIEW IF EXISTS View_SearchItems;");
                }
        }
    }
}
