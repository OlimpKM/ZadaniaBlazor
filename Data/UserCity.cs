using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZadaniaBlazor.Data
{
   public class UserCity
   {
      [Key]
      public int Id { get; set; }

      [Required]
      public string UserId { get; set; } // ID z tabeli AspNetUsers

      // To powiązanie wymusi powstanie klucza obcego (ForeignKey) w bazie
      [ForeignKey("UserId")]
      public virtual ApplicationUser User { get; set; }

      [Required]
      [MaxLength(100)]
      public string CityName { get; set; }

      public bool IsDefault { get; set; }

      // Przechowywanie JSON-ów pogodowych (NVARCHAR(MAX) w bazie)
      public string? CurrentWeatherJson { get; set; }
      public string? ForecastWeatherJson { get; set; }

      // Data ostatniego pobrania danych z OpenWeather
      public DateTime LastUpdate { get; set; }
   }

}
