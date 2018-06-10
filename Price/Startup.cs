using Consul;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Price.Library.Prices;
using System;
using Core.SQL.SQLServer;
using Framework.Entities.Systems;
using Price.Reponsitory.Prices;
using ServiceRegistry.Consul.Entities;
using ServiceRegistry.Consul.Services;

namespace Price
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

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddOptions();
            services.Configure<ConfigDB>(Configuration.GetSection("ConfigDB"));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConnectSQL, ConnectSQL>();

            services.AddTransient<IMSV_PriceService, MSV_PriceService>();

            // Config Consul
            services.AddSingleton<IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfig>(Configuration.GetSection("consulConfig"));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = Configuration["consulConfig:address"];
                consulConfig.Address = new Uri(address);
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseMvc();
        }
    }
}
