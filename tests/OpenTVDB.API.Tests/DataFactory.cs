using Bogus;
using OpenTVDB.API.Entities;
using OpenTVDB.API.Enums;

namespace OpenTVDB.API.Tests;

public static class DataFactory
{
    private static Faker<Series> SeriesFaker => new Faker<Series>()
        .RuleFor(x => x.Title, f => f.Random.Words(4))
        .RuleFor(x => x.Slug, f => f.Random.Words(4).ToLower().Replace(' ', '-'));

    private static Faker<Movie> MovieFaker => new Faker<Movie>()
        .RuleFor(x => x.Title, f => f.Random.Words(4))
        .RuleFor(x => x.Slug, f => f.Random.Words(4).ToLower().Replace(' ', '-'));

    private static Faker<SearchItem> SearchItemFaker => new Faker<SearchItem>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Type, f => f.PickRandom(SearchItemType.Series, SearchItemType.Movie))
        .RuleFor(x => x.Title, f => f.Random.Words(4))
        .RuleFor(x => x.Slug, f => f.Random.Words(4).ToLower().Replace(' ', '-'));

    public static Series Series() => One(SeriesFaker, null);
    public static Series Series(Action<Series> customizer) => One(SeriesFaker, customizer);

    public static Movie Movie() => One(MovieFaker, null);
    public static Movie Movie(Action<Movie> customizer) => One(MovieFaker, customizer);

    public static SearchItem SearchItem() => One(SearchItemFaker, null);
    public static SearchItem SearchItem(Action<SearchItem> customizer) => One(SearchItemFaker, customizer);

    private static T One<T>(Faker<T> faker, Action<T>? customizer) where T : class
    {
        var series = faker.Generate();
        customizer?.Invoke(series);
        return series;
    }

    public static List<T> Many<T>(Func<T> supplier, int count) where T : class
    {
        List<T> items = new();
        for (var i = 0; i < count; i++)
        {
            items.Add(supplier());
        }

        return items;
    }
}
