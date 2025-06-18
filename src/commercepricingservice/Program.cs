using Microsoft.AspNetCore;

namespace commercepricingservice
{
    /// <summary>
    /// Program class            
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configure the startup            
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder
        /// </summary>
        /// <param name="args">arguments array</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
