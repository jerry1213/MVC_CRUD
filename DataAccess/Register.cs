using DataAccess.Models;
using DataAccess.MSSQL_LocalDB.BasicData;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class Register
    {

        public static void RegisterDataAccess(this IServiceCollection service, WebApplicationBuilder builder)
        {
            service.AddDbContext<NorthwindContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
            service.AddScoped<IProductDataAccess, ProductDataAccess>();

        }

    }
}
