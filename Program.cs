using ZadaniaBlazor.Components;
using ZadaniaBlazor.Components.Account;
using ZadaniaBlazor.Data;
using ZadaniaBlazor.Data.Context;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// dodanie serwisu do kontenera
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
// udostępnienie informacji o stanie uwierzytelnienia użytkownika w całej strukturze komponentów aplikacji
builder.Services.AddCascadingAuthenticationState();
// konfiguracja systemu uwierzytelniania - ustawienie domyślnych schematów logowania oraz
// rejestracja mechanizmu obsługi ciasteczek tożsamości (Identity)
builder.Services.AddAuthentication(options =>
{
   options.DefaultScheme = IdentityConstants.ApplicationScheme;
   options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

// db - logowanie
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions => {sqlOptions.EnableRetryOnFailure();}
        )
    );
// przechwytywanie błędów bazy danych w trybie deweloperskim
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// konfiguracja silnika tożsamości - rejestracja usług zarządzania użytkownikami,
// logowania i generowania tokenów zabezpieczających w oparciu o Entity Framework
builder.Services.AddIdentityCore<ApplicationUser>(options =>  {
        options.SignIn.RequireConfirmedAccount = true; // Blokuje możliwość zalogowania się, dopóki użytkownik nie potwierdzi konta
   options.Stores.SchemaVersion = IdentitySchemaVersions.Version3; // Wymusza specyficzny układ tabel w bazie danych
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
// atrapa usługi e-mail - tymczasowa implementacja umożliwiająca działanie systemu Identity bez skonfigurowanego serwera pocztowego
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
// system ochrony przed CSRF - generowanie i weryfikacja tokenów zabezpieczających formularze oraz żądania stanowe przed nieautoryzowanym wywołaniem
builder.Services.AddAntiforgery();
// -- dodanie usług 
// usługa pomocnicza - zarządzanie przekierowaniami,zachowanie stanu (ReturnUrl), integracja z komponentami Account
builder.Services.AddScoped<IdentityRedirectManager>();
// rejestruje niestandardowego dostawcę stanu uwierzytelniania ASP.NET Core Identity
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
// obsługa claims
builder.Services.AddScoped<UserClaimsService>();
// bezpieczne przechowywanie danych w pamięci sesji przeglądarki (sessionStorage)
builder.Services.AddScoped<ProtectedSessionStorage>();
// do obsługi rest api zadania
builder.Services.AddScoped<ZadaniaService>();
// fabryka klientów HTTP
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{ 
   var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
   db.Database.CanConnect(); 
}

// konfiguracja żądań HTTP pipeline.
if (app.Environment.IsDevelopment())
{
   // endpoint migracji - interfejs umożliwiający aktualizację schematu bazy danych bezpośrednio z przeglądarki w trybie deweloperskim
   app.UseMigrationsEndPoint();
}
else
{
   app.UseExceptionHandler("/Error", createScopeForErrors: true);
   // Domyślna wartość HSTS to 30 dni. Możesz chcieć zmienić tę wartość w scenariuszach produkcyjnych, patrz https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// -- Middleware
// wymuszenie szyfrowania - automatyczne przekierowanie ruchu nieszyfrowanego (HTTP) na bezpieczny protokół HTTPS
app.UseHttpsRedirection();
// obsługa zasobów statycznych - umożliwienie wysyłanie plików CSS, JS, obrazów i innych mediów z folderu wwwroot do przeglądarki
app.UseStaticFiles();
//
app.UseRouting();
// identyfikacja użytkownika - proces rozpoznawania tożsamości na podstawie przesłanych poświadczeń i przypisywania ich do bieżącego kontekstu żądania
app.UseAuthentication();
// zarządzanie uprawnieniami - proces weryfikacji dostępu zidentyfikowanego użytkownika do chronionych zasobów i funkcji aplikacji
app.UseAuthorization();
// middleware ochrony - aktywna weryfikacja tokenów bezpieczeństwa przy żądaniach modyfikujących dane, blokująca ataki typu CSRF
app.UseAntiforgery();
// silnik renderowania - definicja głównego wejścia aplikacji (App.razor) oraz włączenie interaktywności opartej
// na stałym połączeniu serwerowym (SignalR)
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
// optymalizacja zasobów - zaawansowane zarządzanie plikami statycznymi z obsługą kompresji, cache'owania i unikalnych wersji plików
app.MapStaticAssets();
// punkty końcowe tożsamości - pomost między interaktywnymi komponentami Blazor a tradycyjnymi
// mechanizmami logowania i wylogowania systemu Identity
app.MapAdditionalIdentityEndpoints();

app.Run();
