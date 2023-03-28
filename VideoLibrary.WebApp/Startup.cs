using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoLibrary;
using VideoLibrary.Core.Services;
using VideoLibrary.DataAccess;
using VideoLibrary.DataAccess.Services;
using VideoLibrary.DataAccess.Services.Interfaces;
using VideoLibrary.IdentityServer.Database;
using VideoLibrary.IdentityServer.Database.Tables;
using VideoLibrary.IdentityServer.Services;
using VideoLibrary.Interfaces;

namespace VideoLibrary_WebApp
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
            SetupIdentityServer(services);
            SetUpScoped(services);
            SetUpMisc(services);
            SetUpApplicationCookie(services);
            SetUpCors(services);

            // Add framework services.
            services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        private void SetupIdentityServer(IServiceCollection services)
        {
            services.AddDbContext<IdentityDatabase>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60)));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.Strict;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Name = "fedexcc.session";
                options.LoginPath = "/";
                options.LogoutPath = "/Authentication/Logout";
                options.AccessDeniedPath = "/Error/403";
            });

            services.AddIdentity<FedExCCUser, FedExCCRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<IdentityDatabase>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/";
                options.UserInteraction.LogoutUrl = "/Authentication/Logout";
                options.Authentication.CookieSameSiteMode = SameSiteMode.Strict;
            })
            .AddConfigurationStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60)))
            .AddOperationalStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60)))
            .AddAspNetIdentity<FedExCCUser>()
            .AddProfileService<IdentityProfileService>()
            .AddDeveloperSigningCredential();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        }

        private void SetUpScoped(IServiceCollection services)
        {
            services.AddDbContext<VideoLibraryDatabase>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60)));
            services.AddScoped<IErrorHandlerService, ErrorHandlerService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailerService, MailerService>();
        }

        private void SetUpMisc(IServiceCollection services)
        {
            services.AddSession();
            services.AddHealthChecks();
        }

        private void SetUpApplicationCookie(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Name = "FedExCC.session";
                options.LoginPath = "/";
                options.LogoutPath = "/Authentication/Logout";
                options.AccessDeniedPath = "/Error/403";
            });
        }

        private void SetUpCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseDeveloperExceptionPage();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            // Required for SSL Offloading must be before UseIdentityServer
            ForwardedHeadersOptions fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
                RequireHeaderSymmetry = false
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllHeaders");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Video}/{action=Index}/{id?}");
            });
        }
    }

}
