using YungChing_MVC.Validations;

namespace YungChing_MVC
{
    public static class Register
    {
        public static void RegisterWeb(this IServiceCollection service)
        {
            service.AddSingleton<CommonValidator>();
        }
    }
}
