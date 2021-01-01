using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Mvc.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Helpdesk.Data.AppContext;
using Helpdesk.Data.Repositories;
using Helpdesk.Service.Services;
using Autofac.Extensions.DependencyInjection;
using Web.Mvc.Framework.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Hangfire;
using Web.Mvc.Framework.Service;
using Web.Mvc.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc(config =>
            {
                // using Microsoft.AspNetCore.Mvc.Authorization;
                // using Microsoft.AspNetCore.Authorization;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //.AddRazorOptions(options => options.AllowRecompilingViewsOnFileChange = false);

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Web.Mvc")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<CustomIdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMemoryCache();

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton(_ => Configuration);

            var builder = new ContainerBuilder(); //Autofac

            builder.Populate(services);
            //builder.RegisterType<MyType>().As<IMyType>();

            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Helpdesk.Service"))
                  .Where(t => t.Name.EndsWith("Service"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                  .Where(t => t.Name.EndsWith("UnitOfWork"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                 .Where(t => t.Name.EndsWith("ApplicationDbContext"))
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            //builder.Register(c => new CustomRepository(Configuration.GetConnectionString("ApplicationContextConnection"))).As<ICustomRepository>();

            builder.RegisterType<ApplicationContext>().As<ApplicationContext>().SingleInstance();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ApplicationUserRoleMappingService>().As<IApplicationUserRoleMappingService>();
            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>();
            builder.RegisterType<CompanyAgentMappingService>().As<ICompanyAgentMappingService>();
            builder.RegisterType<CompanyService>().As<ICompanyService>();
            builder.RegisterType<CompanyUserRoleMappingService>().As<ICompanyUserRoleMappingService>();
            builder.RegisterType<CompanyUserService>().As<ICompanyUserService>();
            builder.RegisterType<DemandService>().As<IDemandService>();
            builder.RegisterType<AppVersionService>().As<IAppVersionService>().SingleInstance();

            builder.RegisterModule(new AutoMapperConfiguration());

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbContextOptions<ApplicationDbContext> option)
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

            app.UseAuthentication();

            app.UseCookiePolicy();
            app.UseSession();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=User}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureHangfire(app);

            //CreateUserRoles(option);
        }

        private static void ConfigureHangfire(IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {
                    //new HangfireAuthFilter()
                    new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter()
                },
            });
        }

        //private void CreateUserRoles(DbContextOptions<ApplicationDbContext> option)
        //{
        //    var context = new ApplicationDbContext(option);
        //    context.Roles.Add(new IdentityRole()
        //    {
        //        Name = "Admin",
        //        NormalizedName = "ADMIN"
        //    });
        //    context.Roles.Add(new IdentityRole()
        //    {
        //        Name = "Agent",
        //        NormalizedName = "AGENT"
        //    });
        //    context.Roles.Add(new IdentityRole()
        //    {
        //        Name = "Company Admin",
        //        NormalizedName = "COMPANY ADMIN"
        //    });
        //    context.Roles.Add(new IdentityRole()
        //    {
        //        Name = "Company User",
        //        NormalizedName = "COMPANY USER"
        //    });
        //    context.SaveChanges();

        //}
    }
}