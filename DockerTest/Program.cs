using DockerTest.Components;
using DockerTest.Persistence;
using DockerTest.Persistence.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var databaseOptions = builder.Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
    
    if (databaseOptions != null)
    {
        databaseOptions.Username = Environment.GetEnvironmentVariable("DOCKERTEST_DB_USERNAME") 
            ?? throw new Exception("Environment Variable \"DOCKERTEST_DB_USERNAME\" is invalid");
        
        databaseOptions.Password = Environment.GetEnvironmentVariable("DOCKERTEST_DB_PASSWORD") 
            ?? throw new Exception("Environment Variable \"DOCKERTEST_DB_PASSWORD\" is invalid");
        
        databaseOptions.Server = Environment.GetEnvironmentVariable("DOCKERTEST_DB_SERVER") 
            ?? throw new Exception("Environment Variable \"DOCKERTEST_DB_SERVER\" is invalid");

        var connectionString = string.Format(
            databaseOptions.ConnectionString,
            databaseOptions.Server,
            databaseOptions.Username,
            databaseOptions.Password);

        options.UseSqlServer(connectionString);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
