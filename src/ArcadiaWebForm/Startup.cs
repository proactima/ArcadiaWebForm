using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArcadiaWebForm
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAccessTokenHandler, AccessTokenHandler>();
            services.AddAutoMapper();
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            var useRemote = Configuration.GetValue<bool>("UseRemote");
            if (useRemote)
            {
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });

                services.AddScoped<ICallApi, ApiCaller>();

                services.AddMvc(o =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                          .RequireAuthenticatedUser()
                          .Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                });

                services.AddAuthentication(
                    SharedOptions => SharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                services.AddScoped<ICallApi, FakeApiCaller>();
                services.AddMvc();
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var useRemote = Configuration.GetValue<bool>("UseRemote");
            if (useRemote)
            {
                app.UseCookieAuthentication();

                app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                {
                    ClientId = Configuration["Authentication:AzureAd:ClientId"],
                    Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"],
                    CallbackPath = Configuration["Authentication:AzureAd:CallbackPath"]
                });
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
