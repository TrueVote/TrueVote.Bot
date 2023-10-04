using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrueVote.Bot.Bots;
using TrueVote.Bot.Services;

namespace TrueVote.Bot
{
    public class Startup
    {
        public static async Task Main()
        {
            var serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var queueName = Environment.GetEnvironmentVariable("ServiceBusApiEventQueueName");

            if (string.IsNullOrEmpty(serviceBusConnectionString) || string.IsNullOrEmpty(queueName))
            {
                Console.WriteLine("Service Bus configuration is missing or incomplete.");
                return;
            }

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(ConfigureServices(serviceBusConnectionString, queueName))
                .Build();

            using var serviceScope = host.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            var receiver = serviceProvider.GetRequiredService<IAzureServiceBusReceiver>();

            await receiver.StartListeningAsync();

            host.Run();

            await receiver.CloseAsync();
        }

        private static Action<IServiceCollection> ConfigureServices(string serviceBusConnectionString, string queueName)
        {
            return services =>
            {
                var telegramBot = new TelegramBot();

                services.AddLogging();
                services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));
                services.TryAddSingleton<ILoggerFactory, LoggerFactory>();
                services.TryAddSingleton(telegramBot);
                services.TryAddScoped<Timer, Timer>();

                services.TryAddSingleton<IAzureServiceBusReceiver>(_ => new AzureServiceBusReceiver(telegramBot, serviceBusConnectionString, queueName));
            };
        }
    }
}
