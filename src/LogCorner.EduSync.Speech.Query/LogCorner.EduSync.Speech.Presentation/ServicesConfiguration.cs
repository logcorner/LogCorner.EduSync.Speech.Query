using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace LogCorner.EduSync.Speech.Presentation
{
    public static class ServicesConfiguration
    {
        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (!bool.TryParse(configuration["isAuthenticationEnabled"], out var isAuthenticationEnabled) || !isAuthenticationEnabled)
            {
                return;
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options =>
                    {
                        configuration.Bind("AzureAdB2C", options);

                        options.TokenValidationParameters.NameClaimType = "name";
                    },
                    options => { configuration.Bind("AzureAdB2C", options); });
        }

        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var tenantName = configuration["SwaggerUI:TenantName"];
            var signUpSignInPolicyId = configuration["AzureAdB2C:SignUpSignInPolicyId"];
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LogCorner Micro Service Event Driven Architecture - Query HTTP API",
                    Version = "v1",
                    Description = "The Speech Micro Service Query HTTP API"
                });

                if (!bool.TryParse(configuration["isAuthenticationEnabled"], out var isAuthenticationEnabled) || !isAuthenticationEnabled)
                {
                    return;
                }

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                new Uri(
                                    $"https://{tenantName}.b2clogin.com/{tenantName}.onmicrosoft.com/{signUpSignInPolicyId}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri(
                                $"https://{tenantName}.b2clogin.com/{tenantName}.onmicrosoft.com/{signUpSignInPolicyId}/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                                {
                                    {
                                        $"https://{tenantName}.onmicrosoft.com/query/api/Speech.List",
                                        "List of Speeches"
                                    },
                                }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            },
                            new[] { $"https://{tenantName}.onmicrosoft.com/query/api/Speech.List" }
                        }
                    });
            });
        }
    }
}