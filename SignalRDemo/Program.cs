using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace SignalRDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(log => 
            {
                log.ClearProviders();
                log.AddConsole();
                log.AddDebug();
                log.AddFilter<DebugLoggerProvider>((cate,level)=>
                {
                    if(cate.Contains("SignalRDemo") && level >= LogLevel.Information)
                    {
                        return true;
                    }
                    return false;
                });
            })
            .UseStartup<Startup>();
    }
}
