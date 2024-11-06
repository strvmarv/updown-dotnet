using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UpdownDotNetTests
{
    public class BaseTest
    {
        protected ILogger Logger;
        protected ILoggerFactory LoggerFactory;

        protected BaseTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(c =>
                {
                    c.SetMinimumLevel(LogLevel.Trace);
                    c.AddConsole();
                    c.AddDebug();
                })
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Logger = LoggerFactory.CreateLogger<BaseTest>();
        }
    }
}
