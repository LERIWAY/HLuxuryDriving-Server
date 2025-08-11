using HLuxuryDriving_Server.Data;
using HLuxuryDriving_Server.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel (dur�e de vie des connexions)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

// Persistance des cl�s antiforgery pour �viter les erreurs entre d�ploiements
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/var/dpkeys"))
    .SetApplicationName("HLuxuryDriving");

// Razor / Blazor / SignalR
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(90);   // Le serveur attendra jusqu'� 90s sans ping
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);       // Ping client toutes les 15s
    })
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true; // Pour voir les erreurs c�t� Blazor
    });

// Autres services
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));

var app = builder.Build();

// Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // HSTS pour la s�curit�
}

// On ne redirige pas vers HTTPS car c�est NGINX qui le g�re d�j�
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// Blazor Hub & fallback page
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
