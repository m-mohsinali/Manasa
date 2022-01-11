using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Award.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Award.Web.Common.Utils;
using Award.Web.Common.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Award.Core.Interfaces;
using Award.Infrastructure.Services;
using Award.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Award.Core.Entities;
using Rotativa.AspNetCore;
using Award.Web.Models;
using Award.Core.ViewModel;

namespace Award
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AwardDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AwardDbContextConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AwardDbContext>();

            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.AddControllers();
            services.AddControllersWithViews();
            IMvcBuilder builder = services.AddRazorPages();


#if DEBUG
            if (Env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IFileInfo, Award.Web.Common.Utils.FileInfo>();
            services.AddScoped<ILdapAuthenticationService, LdapAuthenticationService>();
            services.AddScoped<ISignInManager, SignInManager>();
            services.AddScoped<IManasaEmployeeRepository, ManasaEmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAsyncRepository<Role>, EfRepository<Role>>();
            services.AddScoped<IAsyncRepository<User>, EfRepository<User>>();
            services.AddScoped<IAsyncRepository<UserRole>, EfRepository<UserRole>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSession, UserSession>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAsyncRepository<Sector>, EfRepository<Sector>>();
            services.AddScoped<IAsyncRepository<QualitySectorManager>, EfRepository<QualitySectorManager>>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = new PathString("/Account/Signin");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });


            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
            //    options.HttpOnly = HttpOnlyPolicy.None;
            //    options.Secure = _environment.IsDevelopment()
            //      ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            //});

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                new CultureInfo("en-GB"),
                new CultureInfo("ar-AE")
                //new CultureInfo("fr")
                };

                options.DefaultRequestCulture = new RequestCulture("ar-AE");
                // Formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                options.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddMvc()
        .AddSessionStateTempDataProvider();
            services.AddSession();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //////////////////// Languages
            ////var supportedCultures = new[]
            ////    {
            ////    new CultureInfo("en-GB"),
            ////    new CultureInfo("ar-AE"),
            ////    new CultureInfo("fr")
            ////    };

            ////app.UseRequestLocalization(new RequestLocalizationOptions
            ////{
            ////    DefaultRequestCulture = new RequestCulture("ar-AE"),
            ////    // Formatting numbers, dates, etc.
            ////    SupportedCultures = supportedCultures,
            ////    // UI strings that we have localized.
            ////    SupportedUICultures = supportedCultures
            ////});
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
            //////////////////////////////
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

        }
    }
}
