using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Studentenbeheer.Data;
using Microsoft.AspNetCore.Identity;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Studentenbeheer.Services;
using NETCore.MailKit.Infrastructure.Internal;
using Studentenbeheer.Controllers;

var builder = WebApplication.CreateBuilder(args);

// de connectionString moet manueel toegewezen worden om de nieuwe datacontext te koppelen aan onze bestaande databank
var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<StudentenbeheerUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>(); builder.Services.AddDbContext<IdentityContext>(options =>
     options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<MailKitOptions>(options =>
{
    options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
    options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
    options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
    options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
    options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
    options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
    // Set it to TRUE to enable ssl or tls, FALSE otherwise
    options.Security = false;
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;


    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
}
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Toegevoegd op met de identity-pages te kunnen werken
    app.MapRazorPages();
app.UseMiddleware<SessionUser>();

using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<StudentenbeheerUser>>();
    SeedDatabase.Initialize(services, userManager);
    }
    app.Run();
