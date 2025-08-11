using HLuxuryDriving_Server.Data;
using HLuxuryDriving_Server.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel (durée de vie des connexions)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

// Persistance des clés antiforgery pour éviter les erreurs entre déploiements
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/var/dpkeys"))
    .SetApplicationName("HLuxuryDriving");

// Razor / Blazor / SignalR
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(90);   // Le serveur attendra jusqu'à 90s sans ping
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);       // Ping client toutes les 15s
    })
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true; // Pour voir les erreurs côté Blazor
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
    app.UseHsts(); // HSTS pour la sécurité
}

// On ne redirige pas vers HTTPS car c’est NGINX qui le gère déjà
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// Blazor Hub & fallback page
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
