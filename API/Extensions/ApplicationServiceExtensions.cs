using API.Helpers;
using API.Interfaces;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationsServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            builder.Services.AddDbContext<DataContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            return;
        }
    }
}