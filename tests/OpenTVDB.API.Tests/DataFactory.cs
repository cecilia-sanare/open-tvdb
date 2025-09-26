using Bogus;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Tests;

public static class DataFactory
{
    private static Faker<Series> SeriesFaker => new Faker<Series>()
        .RuleFor(x => x.Title, f => f.Random.Words(4));

    public static Series Series() => One(SeriesFaker, null);
    public static Series Series(Action<Series> customizer) => One(SeriesFaker, customizer);
    
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