using System.ComponentModel.DataAnnotations.Schema;

namespace ZadaniaBlazor.Data
{
   public class Zadanie
   {
      public int Id { get; set; }
      public string Status { get; set; } = string.Empty;
      public string Tytul { get; set; } = string.Empty;
      public string Tresc { get; set; } = string.Empty;
      public bool CzyWykonane { get; set; }
      public DateTime DataUtworzenia { get; set; } = DateTime.Now;
   }
}
