using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Services;
using Hotspot.Tools.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hotspot
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
            services.AddControllersWithViews();
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddIdentity<EmployeeUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;

            }).AddEntityFrameworkStores<HotspotContext>()
            .AddTokenProvider<DataProtectorTokenProvider<EmployeeUser>>(TokenOptions.DefaultProvider);

            string connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<HotspotContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ISeller, SellerService>();
            services.AddScoped<IBatch, BatchService>();
            services.AddScoped<ISale, SaleService>();
            services.AddScoped<ILocale, LocaleService>();
            services.AddScoped<ICashFlow, CashFlowService>();
            services.AddScoped<ILog, LogService>();
            services.AddScoped<IHistory, HistoryService>();
            services.AddScoped<ISpecialTicket, SpecialTicketService>();
            services.AddScoped<ICourtesy, CourtesyService>();
            services.AddScoped<IEmployeeUser, EmployeeUserService>();
            services.AddScoped<ITicket, TicketService>();
            services.AddScoped<IGenerator, GeneratorService>();
            services.AddScoped<ITicketsConfiguration, TicketsConfigurationService>();
            services.AddScoped<ICatalogTicketItem, CatalogTicketItemService>();
            services.AddScoped<ICatalogBatch, CatalogBatchService>();
            services.AddScoped<ICatalogTicket, CatalogTicketService>();

            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            UserManager<EmployeeUser> userManager, RoleManager<IdentityRole> roleManager, 
            ILocale _localeService)
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

            //Migrating Database
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<HotspotContext>();
                context.Database.Migrate();
            }

            //Creating Initial Roles
            CreateRolesAndUsers(userManager, roleManager);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void CreateRolesAndUsers(UserManager<EmployeeUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Creating Roles
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole newRole = new IdentityRole();
                newRole.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(newRole).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                IdentityRole newRole = new IdentityRole();
                newRole.Name = "Employee";
                IdentityResult roleResult = roleManager.CreateAsync(newRole).Result;
            }

            //Creating Users
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                EmployeeUser newUser = new EmployeeUser()
                {
                    UserName = "Admin",
                    Name = "Administrator"
                };

                IdentityResult result = userManager.CreateAsync(newUser, "121214478").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, "Administrator").Wait();
                }
            }

            if (userManager.FindByNameAsync("Funcionario").Result == null)
            {
                EmployeeUser newUser = new EmployeeUser()
                {
                    UserName = "Funcionario",
                    Name = "Funcionario"
                };

                IdentityResult result = userManager.CreateAsync(newUser, "noc@2019").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, "Employee").Wait();
                }
            }
        }
    }
}
