using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TrueVote.Bot
{
    public class Timer
    {
        private readonly ILogger _logger;

        public Timer(ILogger logger)
        {
            _logger = logger;
        }

        [Function("Timer")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation($"Timer trigger function {timerInfo.ScheduleStatus} executed at: {DateTime.Now.ToUniversalTime():dddd, MMM dd, yyyy HH:mm:ss} UTC");
        }
    }
}
