
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesContract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured");
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
