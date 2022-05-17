using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.DbContexts;
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Shared;
using PhramacyApp.Models.Contexts;
using PhramacyApp.Permission;
using PhramacyApp.Services;
using StoreManager.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;


namespace PhramacyApp
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
            
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddNotyf(o =>
            {
                o.DurationInSeconds = 10;
                o.IsDismissable = true;
                o.HasRippleEffect = true;
            });
            services.AddRazorPages();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationConnection")));
            //services.AddSingleton<ApplicationDbContext>();
            //services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseSqlServer(Configuration.GetConnectionString("ApplicationConnection"))
                .EnableSensitiveDataLogging(true)
                );
            //services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            //services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
            //services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            //}
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();


            services.AddMvc(o =>
            {
                //Add Authentication to all Controllers by default.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddInfrastructure();
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllersWithViews().AddNewtonsoftJson();
            //services.AddRazorPages().AddNewtonsoftJson();

            //services.AddControllers()
            //.AddJsonOptions(options =>
            //   options.JsonSerializerOptions.PropertyNamingPolicy = null);
            //JSON Serializer
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                   Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            #region Registering ResourcesPath

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            #endregion Registering ResourcesPath
            services.AddMvc()
   .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
   .AddDataAnnotationsLocalization(options =>
   {
       options.DataAnnotationLocalizerProvider = (type, factory) =>
           factory.Create(typeof(SharedResource));
   });
            services.AddOptions();
            services.AddHttpContextAccessor();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
        new CultureInfo("en"),
         new CultureInfo("ar"),
        new CultureInfo("fr")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            services.AddControllersWithViews().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            //services.AddSingleton(typeof(IConverter),new SynchronizedConverter(new PdfTools()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseNotyf();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Dashboard}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
