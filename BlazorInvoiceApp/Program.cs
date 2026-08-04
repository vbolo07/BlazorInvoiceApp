using AutoMapper;
using BlazorInvoiceApp.Components;
using BlazorInvoiceApp.Components.Account;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<DialogService>();
builder.Services.AddRadzenComponents();

//add repository collection service
builder.Services.AddTransient<IRepositoryCollection, RepositoryCollection>();

//add mapper configuration based on the profile
var mapperConfig = new MapperConfiguration(mc => 
{ 
    mc.AddProfile(new AutoMapperProfile()); 
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

//var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
//var connectionString1 = $"Data Source={dbHost};Initial Catalog={dbName};User ID=root;Password={dbPassword}";
//var connectionString = $"server={dbHost};user=root;password={dbPassword};database={dbName}";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddHealthChecks();
   
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

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// This is only for container startup to wait for SQL Server
var retries = 10;
var delay = TimeSpan.FromSeconds(5);

for (int attempt = 1; attempt <= retries; attempt++)
{
    try
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();

        await ApplicationDbInitializer.SeedAsync(scope.ServiceProvider);

        Console.WriteLine("Database migration and seeding completed.");

        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Database not ready. Attempt {attempt}/{retries}. Error: {ex.Message}");

        if (attempt == retries)
        {
            throw;
        }

        await Task.Delay(delay);
    }
}
app.MapHealthChecks("/health");
app.Run();
