using CoreController.Commands;

namespace CoreController.Extensions
{
    public static class CommandRegistrator
    {
        public static IServiceCollection AddCommandRegistry(this IServiceCollection services)
        {
            services.AddSingleton<ICommand, WakeOnLanCommand>();
            services.AddSingleton<ICommand, WorkModeCommand>();

            services.AddSingleton<CommandRegistry>();
            return services;
        }
    }
}
