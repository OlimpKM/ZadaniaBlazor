using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ZadaniaBlazor.Data
{
    public class ZadaniaService
    {
      private readonly HttpClient _httpClient;
      private readonly string _url;

      // Wstrzykujemy IConfiguration, aby pobrać adres URL
      public ZadaniaService(IConfiguration configuration, HttpClient httpClient)
      {
         _httpClient = httpClient;
         // Pobieramy wartość z appsettings.json
         _url = configuration["ExternalServices:ZadaniaApi"] ?? throw new ArgumentNullException("Brak adresu API w konfiguracji");
      }

      public async Task<List<Zadanie>> PobierzZadania(string token)
      {
         try
         {
            // Przygotowanie zapytania
            var request = new HttpRequestMessage(HttpMethod.Get, _url);

            // Dodanie tokena
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Wysyłka
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
               return await response.Content.ReadFromJsonAsync<List<Zadanie>>() ?? new List<Zadanie>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
               Console.WriteLine("Błąd: Brak autoryzacji (401). Token może być nieważny.");
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Błąd połączenia: {ex.Message}");
         }

         return new List<Zadanie>();
      }

      public async Task<bool> ZapiszZadanie(Zadanie zadanie, string token, bool czyNowe)
      {
         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         HttpResponseMessage response;

         if (czyNowe)
            response = await _httpClient.PostAsJsonAsync($"{_url}", zadanie);
         else
            response = await _httpClient.PutAsJsonAsync($"{_url}/{zadanie.Id}", zadanie);

         return response.IsSuccessStatusCode;
      }

      public async Task<bool> ZmienStatus(int id, bool czyWykonane, string token)
      {
         // Pobieramy zadanie, zmieniamy status i wysyłamy PUT
         var zadania = await PobierzZadania(token);
         var zadanie = zadania.FirstOrDefault(z => z.Id == id);
         if (zadanie == null) return false;

         zadanie.CzyWykonane = czyWykonane;
         return await ZapiszZadanie(zadanie, token, false);
      }

   }
}
