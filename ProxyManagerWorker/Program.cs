using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManagerWorker
{
    class MyLogger : ILogger
    {
        private readonly NLog.Logger Logger;
        public MyLogger()
        {
            this.Logger = NLog.LogManager.GetCurrentClassLogger();
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    this.Logger.Debug(formatter(state, exception));
                    break;
                case LogLevel.Information:
                    this.Logger.Info(formatter(state, exception));
                    break;
                case LogLevel.Warning:
                    this.Logger.Warn(formatter(state, exception));
                    break;
                case LogLevel.Error:
                    this.Logger.Error(formatter(state, exception));
                    break;
                case LogLevel.Critical:
                    this.Logger.Warn(formatter(state, exception));
                    break;
                case LogLevel.Trace:
                    this.Logger.Trace(formatter(state, exception));
                    break;
                default:
                    this.Logger.Info(formatter(state, exception));
                    break;
            }
            //Console.WriteLine(formatter(state,exception));
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            LocalInfo.Create(configuration, new MyLogger());
            LocalInfo.Get().StartLoop();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
