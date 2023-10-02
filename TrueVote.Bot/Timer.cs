using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TrueVote.Bot
{
    public class TimerInfo
    {
        public required ScheduleStatus ScheduleStatus { get; set; }
        public bool IsPastDue { get; set; }
    }

    public class ScheduleStatus
    {
        public DateTime Last { get; set; }
        public DateTime Next { get; set; }
        public DateTime LastUpdated { get; set; }
    }

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
            _logger.LogInformation($"Timer trigger executed function Timer->Run() at: {DateTime.Now.ToUniversalTime} UTC");
            _logger.LogInformation($"Timer trigger scheduled next run for for: {timerInfo.ScheduleStatus.Next.ToUniversalTime} UTC");
        }
    }
}
