using ZadaniaBlazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class UserClaimsService
{
   private readonly IServiceScopeFactory _scopeFactory;
   private readonly AuthenticationStateProvider _authStateProvider;

   public UserClaimsService(IServiceScopeFactory scopeFactory, AuthenticationStateProvider authStateProvider)
   {
      _scopeFactory = scopeFactory;
      _authStateProvider = authStateProvider;
   }

   /// <summary>
   /// Pobiera Claims dla aktualnie zalogowanego użytkownika w bezpiecznym scope.
   /// </summary>
   public async Task<IList<Claim>> GetUserClaimsSafeAsync()
   {
      var authState = await _authStateProvider.GetAuthenticationStateAsync();
      var userPrincipal = authState.User;

      if (userPrincipal.Identity?.IsAuthenticated != true)
         return Array.Empty<Claim>();

      using var scope = _scopeFactory.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var user = await userManager.GetUserAsync(userPrincipal);

      if (user == null)
         return Array.Empty<Claim>();

      return await userManager.GetClaimsAsync(user);
   }

   /// <summary>
   /// Pobiera konkretny claim dla zalogowanego użytkownika.
   /// </summary>
   public async Task<string?> GetClaimValueAsync(string claimType)
   {
      var claims = await GetUserClaimsSafeAsync();
      return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
   }

   /// <summary>
   /// Ustawia konkretny claim dla zalogowanego użytkownika.
   /// </summary>
   public async Task SetClaimValueAsync(string claimType, string claimValue)
   {
      var authState = await _authStateProvider.GetAuthenticationStateAsync();
      var userPrincipal = authState.User;

      if (userPrincipal.Identity?.IsAuthenticated != true)
         return;

      using var scope = _scopeFactory.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var user = await userManager.GetUserAsync(userPrincipal);

      if (user == null)
         return;

      var claims = await userManager.GetClaimsAsync(user);
      var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);

      if (existingClaim != null)
      {
         await userManager.ReplaceClaimAsync(
             user,
             existingClaim,
             new Claim(claimType, claimValue)
         );
      }
      else
      {
         await userManager.AddClaimAsync(
             user,
             new Claim(claimType, claimValue)
         );
      }
   }

   /// <summary>
   /// Usuwamy konkretny claim zalogowanego użytkownika.
   /// </summary>
   public async Task RemoveClaimAsync(string claimType)
   {
      var authState = await _authStateProvider.GetAuthenticationStateAsync();
      var userPrincipal = authState.User;

      if (userPrincipal.Identity?.IsAuthenticated != true)
         return;

      using var scope = _scopeFactory.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var user = await userManager.GetUserAsync(userPrincipal);

      if (user == null)
         return;

      var claims = await userManager.GetClaimsAsync(user);
      var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);

      if (existingClaim != null)
      {
         await userManager.RemoveClaimAsync(user, existingClaim);
      }
   }
}
