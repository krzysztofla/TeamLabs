namespace TeamLabs.Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json")
                       .AddEnvironmentVariables();
               });
    }
    
}
