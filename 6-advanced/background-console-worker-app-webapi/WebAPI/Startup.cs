// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using IntegratedCacheUtils;
using IntegratedCacheUtils.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System;
using System.Diagnostics;
using WebAPI.Services;

namespace WebAPI
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
            // Add Sql Server as Token cache store
            services.AddDbContext<IntegratedTokenCacheDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TokenCacheDbConnStr")));

            // Configure the dependency injection for IMsalAccountActivityStore to use a SQL Server to store the entity MsalAccountActivity.
            // You might want to customize this class, or implement our own, with logic that fits your business need.
            services.AddScoped<IMsalAccountActivityStore, SqlServerMsalAccountActivityStore>();

            // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
            // Configures the web api to call another web api (Ms Graph) using OBO
            // Sets the IMsalTokenCacheProvider to be the IntegratedTokenCacheAdapter
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddIntegratedUserTokenCache();

            // Add a custom GraphServiceClient which leverages OBO keys for persistence.
            services.AddCustomMicrosoftGraphClient(Configuration);

            // Add Sql Server as distributed Token cache store
            // This config should match that of the BackgroundWorker
            services.AddDistributedSqlServerCache(options =>
            {
                /*
                    dotnet tool install --global dotnet-sql-cache
                    dotnet sql-cache create "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MsalTokenCacheDatabase;Integrated Security=True;" dbo TokenCache    
                */
                options.ConnectionString = Configuration.GetConnectionString("TokenCacheDbConnStr");
                options.SchemaName = "dbo";
                options.TableName = "TokenCache";
                //Once expired, the cache entry is automatically deleted by Microsoft.Extensions.Caching.SqlServer library
                options.DefaultSlidingExpiration = TimeSpan.FromHours(2);
            });

            // Add Redis as distributed Token cache store
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = Configuration.GetConnectionString("TokenCacheRedisConnStr");
            //    options.InstanceName = Configuration.GetConnectionString("TokenCacheRedisInstaceName");
            //});

            services.AddControllers();

            // Allowing CORS for all domains and methods for the purpose of sample
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("WWW-Authenticate");
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IntegratedTokenCacheDbContext>();
                context.Database.Migrate();
                // Comment the line below if you are using Redis distributed token cache
                SqlCacheExtensions.ConfigureSqlCacheFromCommand(Configuration);
            }

            if (env.IsDevelopment())
            {
                // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
                // PII hiding in log files is enabled by default for GDPR concerns.
                // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
                // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class SqlCacheExtensions
    {
        public static void ConfigureSqlCacheFromCommand(IConfiguration configuration)
        {
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c dotnet sql-cache create \"{configuration.GetConnectionString("TokenCacheDbConnStr")}\" dbo TokenCache",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal,
                    RedirectStandardInput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            string input = process.StandardError.ReadToEnd();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
    }
}
