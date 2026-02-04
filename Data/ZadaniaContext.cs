using ZadaniaBlazor.Data;
using Microsoft.EntityFrameworkCore;

namespace RestAPI_zadania.Data
{
   public class ZadaniaContext : DbContext
   {
      public ZadaniaContext(DbContextOptions<ZadaniaContext> options) : base(options) { }
      public DbSet<Zadanie> Zadania { get; set; }
   }
}