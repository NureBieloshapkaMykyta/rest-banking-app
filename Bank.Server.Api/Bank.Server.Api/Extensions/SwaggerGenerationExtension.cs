using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Bank.Server.Api.Extensions;

public static class SwaggerGenerationExtension
{
    public static IServiceCollection AddSwaggerGeneration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var userAuthenticationSchema =
    GetJwtBearerOpenApiSecuritySchemeByAuthenticationSchema("User");

            options.AddSecurityDefinition("User", userAuthenticationSchema);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    userAuthenticationSchema,
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    private static OpenApiSecurityScheme GetJwtBearerOpenApiSecuritySchemeByAuthenticationSchema(string authenticationSchema)
    {
        return new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = authenticationSchema
            },
            Name = authenticationSchema,
            Description = $"JWT Authorization for {authenticationSchema}. Example: \"Bearer {{token}}\"",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        };
    }

    private static string GetSwaggerXmlCommentsDocumentPath()
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        return xmlPath;
    }
}