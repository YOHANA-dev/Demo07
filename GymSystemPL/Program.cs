using GymSystemBLL;
using GymSystemBLL.Services.AttachmentService;
using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemDAL.Data.Contexts;
using GymSystemDAL.Data.DataSeed;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Classes;
using GymSystemDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystemPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            #region Dependency Injection

            // 1. Make DbContext Class 'public'

            builder.Services.AddDbContext<GymSystemDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionsStrings")["DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            #endregion

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.Password.RequiredLength = 6;
                Config.Password.RequireLowercase = true;
                Config.Password.RequireUppercase = true;
                Config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymSystemDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            #region Data Seed

            var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymSystemDbContext>();

            // Check if There is Migrations Pending or Not

            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
            {
                dbContext.Database.Migrate();
            }

            GymDbContextSeeding.SeedData(dbContext);

            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            IdentityDbContextSeeding.SeedData(roleManager, userManager);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
