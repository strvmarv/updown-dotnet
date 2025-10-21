using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UpdownDotNetTests
{
    /// <summary>
    /// Base class for tests with logging support.
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        protected ILogger Logger { get; }
        
        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class.
        /// </summary>
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

            LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            Logger = LoggerFactory.CreateLogger<BaseTest>();
        }
    }
}
