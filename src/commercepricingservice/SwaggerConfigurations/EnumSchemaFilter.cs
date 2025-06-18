using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace commercepricingservice.SwaggerConfigurations
{
    /// <summary>
    /// Enum Schema Filter
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        //Newtonsoft.Json's StringEnumConverter doesn't seem to be getting picked up by swagger
        //This is a workaround so that enum values will display as strings on the swagger docs
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Type = "string";
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(n => model.Enum.Add(new OpenApiString(n)));
            }
        }
    }
}
