using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using SMMT_Test.Components;
using SMMT_Test.Components.Account;
using SMMT_Test.Data;
using SMMT_Test.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<Radzen.ThemeService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// N.B. Azure App Settings Connection String overwrites the value in app.settings.json file, but it is in plain text.
// The Connection String could be stored in Azure Key Vault which is encrypted and it provides a centralised way to manage secrets. 
// For App Service to access the Key Vault we need to enable Managed Identity, which is like Windows Service Account on-premises so credentials are not stored.
// Managed Identity uses Microsoft Entra ID which can grant access to other resources such as SQL Server, in this case we do not need to store username and password in the Key Vault, because it is not needed.
//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Database migration should be done as part of deployment and in a controlled way.
// THIS IS TO PREVENT FOR HAVING MULTIPLE UPDATES AT THE SAME TIME.
// Production database migration approaches include:
// Using migrations to create SQL scripts and using the SQL scripts in deployment. (script-migration command can be added in the pipeline during the deployment)
// OR
// Running dotnet ef database update from a controlled environment.
if (app.Environment.IsDevelopment())
    ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context != null)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                Log.Information("Migration has started.");
                context.Database.Migrate();
                Log.Information("Migration has ended.");
            }

            //Call static method to create lookup values and sample data
            SeedData.Initialize(services);
        }
    }
}
