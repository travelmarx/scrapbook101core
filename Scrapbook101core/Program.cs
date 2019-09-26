using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Scrapbook101core
{
    /// <summary>
    /// Defines the entry point into the Scrapbook ASP.NET Core web application. 
    /// ASP.NET core apps configure and launch a host. The host is responsible for the app startup
    /// and lifetime management. For more information,  
    /// <see href="https://docs.microsoft.com/aspnet/core/fundamentals/host/web-host?view=aspnetcore-3.0">ASP.NET Core Web Host</see>.
    /// </summary>
    /// <remarks>
    /// Program remarks.
    /// </remarks>
    public class Program
    {
        /// <summary>
        /// Defines the entry point into the C# application. This is the first method invoked.
        /// </summary>
        /// <param name="args">An optional array of arguments to pass to the program.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// In the pattern used here, this method creates a builder object. For more information, see
        /// <see href="https://docs.microsoft.com/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2">ASP.NET Core Web Host</see>.
        /// </summary>
        /// <param name="args">An optional array of arguments to pass to the builder.</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                IHostingEnvironment env = builderContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
           .UseStartup<Startup>();
    }
}
