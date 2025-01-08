
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Purchasing.API.Helpers;
using Purchasing.Application.Services;
using Purchasing.Domain.Interfaces;
using Purchasing.Infrastructure.Data;
using Purchasing.Infrastructure.Repositories;

namespace Purchasing.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PurchasingDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddScoped<IItemService, ItemService>();

            //builder.Services.AddScoped<PurchaseOrderService>();
            //builder.Services.TryAddScoped<ItemService>();
            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Apply migrations and update the database automatically
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<PurchasingDbContext>();

                    // Apply migrations
                    logger.LogInformation("Applying migrations...");
                    context.Database.Migrate();

                    // Optional: Seed or update the database after migration
                    //logger.LogInformation("Database migration completed. Updating database...");
                    //SeedOrUpdateDatabase(context, logger);

                    logger.LogInformation("Database update completed successfully.");
                }
                catch (Exception ex)
                {
                    // Log the exception with ILogger
                    logger.LogError(ex, "An error occurred while applying migrations or updating the database.");
                }


                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
        }
    }
}