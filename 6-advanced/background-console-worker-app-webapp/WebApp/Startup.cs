using IntegratedCacheUtils;
using IntegratedCacheUtils.Stores;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Diagnostics;

namespace WebApp
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://learn.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
            });

            // Sign-in users with the Microsoft identity platform
            // Configures the web app to call a web api (Ms Graph)
            // Sets the IMsalTokenCacheProvider to be the IntegratedTokenCacheAdapter
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration, "AzureAd", subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true)
                    .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddMicrosoftGraph(Configuration.GetSection("GraphAPI"))
                    .AddIntegratedUserTokenCache();

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

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
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