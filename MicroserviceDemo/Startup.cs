using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Catalog.Reponsitory.Organizations;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Consul;
using Core.SQL.SQLServer;
using Framework.Entities.Systems;
using ServiceRegistry.Consul.Entities;
using ServiceRegistry.Consul.Services;

namespace Catalog
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
            //ConfigureJwtAuthService(services);

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddOptions();
            services.Configure<ConfigDB>(Configuration.GetSection("ConfigDB"));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConnectSQL, ConnectSQL>();

            services.AddTransient<IMSV_OrganizationService, MSV_OrganizationService>();
            services.AddTransient<IMSV_ProductCategoryService, MSV_ProductCategoryService>();
            services.AddTransient<IMSV_ProductService, MSV_ProductService>();

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

        public void ConfigureJwtAuthService(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //o.Authority = "";
                //o.Audience = "";
                o.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
