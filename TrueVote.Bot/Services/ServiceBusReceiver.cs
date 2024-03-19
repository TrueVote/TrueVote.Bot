#pragma warning disable IDE0058 // Expression value is never used

using Azure.Messaging.ServiceBus;
using TrueVote.Bot.Bots;

namespace TrueVote.Bot.Services
{
    public interface IAzureServiceBusReceiver
    {
        Task StartListeningAsync();
        Task CloseAsync();
    }

    public class AzureServiceBusReceiver : IAzureServiceBusReceiver
    {
        private readonly TelegramBot _telegramBot;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public AzureServiceBusReceiver(TelegramBot telegramBot, string connectionString, string queueName)
        {
            _telegramBot = telegramBot;
            _serviceBusClient = new ServiceBusClient(connectionString);
            _serviceBusProcessor = _serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            });
        }

        public async Task StartListeningAsync()
        {
            _serviceBusProcessor.ProcessMessageAsync += async args =>
            {
                var messageBody = args.Message.Body.ToString();
                Console.WriteLine($"Received message: {messageBody}");
                await _telegramBot.SendChannelMessageAsync(messageBody);

                await args.CompleteMessageAsync(args.Message);
            };

            _serviceBusProcessor.ProcessErrorAsync += ExceptionHandler;

            // Start processing messages
            await _serviceBusProcessor.StartProcessingAsync();
        }

        public async Task CloseAsync()
        {
            // Stop processing messages and close the processor
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.CloseAsync();

            // Close the Service Bus client and dispose of resources
            await _serviceBusClient.DisposeAsync();
        }

        private Task ExceptionHandler(ProcessErrorEventArgs args)
        {
            // Handle exceptions that occur during message processing here
            // You can log the error, retry, or take other appropriate actions
            Console.WriteLine($"An error occurred: {args.Exception}");

            return Task.CompletedTask;
        }
    }
}
#pragma warning restore IDE0058 // Expression value is never used
