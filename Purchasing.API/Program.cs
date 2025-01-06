
using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddScoped<PurchaseOrderService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
