using Quartz;

namespace SteamClone.API.Jobs
{
    public class LogsCleanJob : IJob
    {
        private readonly IWebHostEnvironment _env;

        public LogsCleanJob(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var logsPath = Path.Combine(_env.ContentRootPath, "Logs");

            if (!Directory.Exists(logsPath))
            {
                return Task.CompletedTask;
            }

            var files = Directory.GetFiles(logsPath, "*.log");

            foreach (var filePath in files)
            {
                var file = new FileInfo(filePath);

                if (DateTime.Now - file.LastWriteTime > TimeSpan.FromDays(7))
                {
                    file.Delete();
                }
            }

            return Task.CompletedTask;
        }
    }
}
