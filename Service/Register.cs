using Microsoft.Extensions.DependencyInjection;
using Service.BasicData;

namespace Service
{
    public static class Register
    {
        public static void RegisterService(this IServiceCollection service)
        {
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<ProductServiceValidator>();

        }
    }
}
