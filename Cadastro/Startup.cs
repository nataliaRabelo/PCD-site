using Cadastro.Domain.Entities;
using Cadastro.Infrastructure.Data.Common;
using Cadastro.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Cadastro
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
            services.AddRepositories().AddServices();

            services.AddAutoMapper(typeof(Startup));

            //services.AddDbContext<RegisterContext>(c =>
            //c.UseInMemoryDatabase("Register"));

            //services.AddDbContextPool<RegisterContext>(options =>
            //options.UseSqlServer(Configuration["ConnecitonStrings:AWSDB"]));

            //var connectionString = Configuration["ConnecitonStrings:AWSDB"];
            //services.AddDbContext<RegisterContext>(options =>
            //options.UseNpgsql(connectionString)
            //);
            services.AddDbContext<RegisterContext>(options =>
                        options.UseNpgsql(Configuration.GetConnectionString("AWSDB")));
                    

            services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false; // não é preciso confirmar email para criar uma conta
            })
            .AddRoles<IdentityRole>()
            //.AddUserStore<UserStore>()
            .AddDefaultTokenProviders()
            .AddDefaultUI()
            .AddEntityFrameworkStores<RegisterContext>();

            //.AddIdentityCore<User>()
            //    .AddUserStore<UserStore>()
            //    .AddDefaultTokenProviders()
            //    .AddSignInManager<SignInManager<User>>();



            services.AddRazorPages();
            services.AddAuthentication();

            services.AddAuthorization();
            services.ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = x =>
                    {
                        x.Response.Redirect("Identity/Account/Login");
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddControllersWithViews();
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
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
}
