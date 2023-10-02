using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrueVote.Bot.Bots;

namespace TrueVote.Bot
{
    public class Startup
    {
        public static void Main()
        {
            var host = new HostBuilder().ConfigureFunctionsWorkerDefaults().ConfigureServices(s =>
            {
                s.TryAddSingleton<ILoggerFactory, LoggerFactory>();
                s.TryAddSingleton(new TelegramBot());
                s.TryAddScoped<Timer, Timer>();

                ConfigureServices(s).BuildServiceProvider(true);
            }).Build();

            host.Run();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));

            return services;
        }
    }
}
