using ArcadiaWebForm.Models;
using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

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
                builder.AddUserSecrets<Startup>();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private IApplicationBuilder _app;

        public static string ClientId;
        public static string ClientSecret;
        public static string Authority;
        public static string ResourceId;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAccessTokenHandler, AccessTokenHandler>();
            services.AddAutoMapper();
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ProfileAttribute>();

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
                    o.Filters.Add(typeof(ProfileAttribute));
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
            _app = app;
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var useRemote = Configuration.GetValue<bool>("UseRemote");
            if (useRemote)
            {
                ClientId = Configuration["Authentication:AzureAd:ClientId"];
                ClientSecret = Configuration["Authentication:AzureAd:SecretKey"];
                Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"];
                ResourceId = Configuration["Authentication:AzureAd:ResourceId"];

                app.UseCookieAuthentication(new CookieAuthenticationOptions());

                app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                {
                    ClientId = ClientId,
                    Authority = Authority,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    GetClaimsFromUserInfoEndpoint = false,
                    Events = new OpenIdConnectEvents
                    {
                        OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                    }
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

        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            var request = context.HttpContext.Request;
            var userObjectId = (context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);
            var clientCredential = new ClientCredential(ClientId, ClientSecret);
            var authContext = new AuthenticationContext(Authority, new InMemoryTokenCache(_app.ApplicationServices.GetService<IMemoryCache>(), userObjectId));

            var result = await authContext.AcquireTokenByAuthorizationCodeAsync(context.ProtocolMessage.Code, new Uri(currentUri), clientCredential, ResourceId);

            context.HandleCodeRedemption(result.AccessToken, result.IdToken);
        }
    }
}
