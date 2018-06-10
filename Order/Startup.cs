using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.SQL.SQLServer;
using Framework.Entities.Systems;
using IdentityServer4.AccessTokenValidation;
using Order.Reponsitory;

namespace Order
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
            services.AddAuthentication(IdentityServerAuthenticationDefaults.JwtAuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:5000"; // Auth Server
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "auth_api"; // API Resource Id
                    });

            services.AddMvc();

            services.AddOptions();
            services.Configure<ConfigDB>(Configuration.GetSection("ConfigDB"));
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IConnectSQL, ConnectSQL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseMvc();
        }
    }
}
