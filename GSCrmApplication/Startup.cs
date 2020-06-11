using GSCrmLibrary.Services.Info;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using GSCrmApplication.Data;

namespace GSCrmApplication
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<GSAppContext>(/*options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("GSCrmTable"))*/);
            services.AddSingleton<IAppletInfo, AppletInfo>();
            services.AddSingleton<IAppletInfoUI, AppletInfoUI>();
            services.AddSingleton<IViewInfo, ViewInfo>();
            services.AddSingleton<IViewInfoUI, ViewInfoUI>();
            services.AddSingleton<IScreenInfo, ScreenInfo>();
            services.AddSingleton<IScreenInfoUI, ScreenInfoUI>();
            services.AddSingleton<IApplicationInfo, ApplicationInfo>();
            services.AddSingleton<IApplicationInfoUI, ApplicationInfoUI>();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}