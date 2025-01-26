using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace MdGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<TokenDelegatingHandler>();

            builder.Services.AddOcelot()
                .AddDelegatingHandler<TokenDelegatingHandler>();

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options => { });
            builder.Services.AddSwaggerGen(c =>
            {
                // Add security definition for bearer token
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter 'Bearer' followed by your token in the Authorization header",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add security requirement to use the token globally
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ocelot API Gateway V1");
                    c.RoutePrefix = string.Empty;  // Set the Swagger UI at the root URL
                });
            }
            app.UseRouting();
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();


            app.MapControllers();
            app.UseOcelot().Wait();

            app.Run();
        }
    }

}
