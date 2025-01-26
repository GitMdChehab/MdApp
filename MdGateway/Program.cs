using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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
            .AddJwtBearer("Bearer", options =>
            {
                
            });
            builder.Services.AddControllers();

            // Add Ocelot services
            builder.Services.AddOcelot();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization(); 


            app.MapControllers();
            app.UseOcelot().Wait();

            app.Run();
        }
    }

}
