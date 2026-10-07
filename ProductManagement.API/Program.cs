using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ProductManagement.Business;
using ProductManagement.Common.Settings;
using ProductManagement.Data;
using ProductManagement.Data.Context;
using ProductManagement.Data.Seeder;
using ProductManagement.DataAccess;
using System.Reflection;
using System.Xml;

namespace ProductManagement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLog4Net();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AppSettings>(builder.Configuration);
            builder.Services.Configure<DbContextSettings>(builder.Configuration.GetSection("ConnectionStrings"));

            builder.Services.AddBusiness();
            builder.Services.AddDataAccess(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product Management API",
                    Description = "REST API for managing products and stock."
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await context.Database.MigrateAsync();
                await DatabaseSeeder.SeedAsync(context);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API v1");
                    options.DocumentTitle = "Product Management API";
                });
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }

        private static void ConfigureLog4Net()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}