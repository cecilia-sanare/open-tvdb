using System.Reflection;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace OpenTVDB.API;

public class CustomSchemaIgnoreDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        foreach (var (typeName, schema) in context.Document.Components.Schemas)
        {

            // Try to get Type by name, assuming it's in a known assembly/namespace
            var type = Type.GetType("OpenTVDB.API.Entities." + typeName);

            if (type == null)
                continue;

            var propertiesToRemove = schema.Properties
                .Where(property =>
                    type.GetProperty(
                        property.Key,
                BindingFlags.Public | 
                            BindingFlags.Instance | 
                            BindingFlags.IgnoreCase
                    )?.GetCustomAttribute<Annotations.SchemaIgnore>() != null
                );

            foreach (var propName in propertiesToRemove)
            {
                schema.Properties.Remove(propName);
            }
        }
    }
}