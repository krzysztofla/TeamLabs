using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TeamLabs.Gateway.API.Infrastructure;
using TeamLabs.Gateway.API.Models;

namespace TeamLabs.Gateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<RoutesMiddleware>();
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAd");
            services.AddAzureClients(builder => {
                builder.AddServiceBusClient(Configuration.GetConnectionString("AzureServiceBus"));
            });
            services.AddHttpClient();
            services.AddOcelot(Configuration);
            services.Configure<RoutesOptions>(Configuration.GetSection("ExchangeRoutes"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<RoutesMiddleware>();
            app.UseOcelot(GenerateOcelotConfiguration()).GetAwaiter().GetResult();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static OcelotPipelineConfiguration GenerateOcelotConfiguration() 
        => new OcelotPipelineConfiguration() 
        {
            AuthenticationMiddleware = async (context, next) => {
                if(!context.DownstreamReRoute.IsAuthenticated) {
                    await next.Invoke();
                    return;
                }

                
                var authResult = await context.HttpContext.AuthenticateAzureFunctionAsync(); //returns true if user is authenticated
                if(authResult.Item1) {
                    await next.Invoke();
                }
            }
        };
    }
}