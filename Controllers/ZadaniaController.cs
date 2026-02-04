using ZadaniaBlazor.Components.Pages;
using ZadaniaBlazor.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_zadania.Data;

namespace ZadaniaBlazor.Controllers
{
   [Authorize] // Wymaga tokena dla każdej metody w tej klasie
   [Route("api/[controller]")]
   [ApiController]
   public class ZadaniaController : ControllerBase
   {
      private readonly ZadaniaContext _context;

      public ZadaniaController(ZadaniaContext context)
      {
         _context = context;
      }

      // 1. POBIERANIE LISTY ZADAŃ (Tylko Twoje własne)
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Zadanie>>> GetZadania()
      {
         return await _context.Zadania
             .ToListAsync();
      }

      // Dodaj tę metodę do klasy, aby PostZadanie miało poprawny link
      [HttpGet("{id}")]
      public async Task<ActionResult<Zadanie>> GetZadanie(int id)
      {
         var currentUser = User.Identity?.Name;
         var zadanie = await _context.Zadania
             .FirstOrDefaultAsync(z => z.Id == id);

         if (zadanie == null) return NotFound();
         return zadanie;
      }

      // 2. DODAWANIE NOWEGO ZADANIA
      [HttpPost]
      public async Task<ActionResult<Zadanie>> PostZadanie(Zadanie zadanie)
      {
         // Automatycznie przypisujemy właściciela z tokena
         // Dzięki temu użytkownik nie może "podrobić" właściciela w JSONie
         _context.Zadania.Add(zadanie);
         await _context.SaveChangesAsync();

         return CreatedAtAction(nameof(GetZadania), new { id = zadanie.Id }, zadanie);
      }

      // 3. AKTUALIZACJA ZADANIA (np. oznaczenie jako wykonane)
      [HttpPut("{id}")]
      public async Task<IActionResult> PutZadanie(int id, Zadanie zadanie)
      {
         if (id != zadanie.Id) return BadRequest();

         var currentUser = User.Identity?.Name;

         // Sprawdzamy czy zadanie istnieje i czy należy do zalogowanej osoby
         var exists = await _context.Zadania
             .AnyAsync(z => z.Id == id);

         if (!exists) return NotFound("Nie masz uprawnień do edycji tego zadania.");

         _context.Entry(zadanie).State = EntityState.Modified;
         await _context.SaveChangesAsync();

         return NoContent();
      }

      // 4. USUWANIE ZADANIA
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteZadanie(int id)
      {
         var currentUser = User.Identity?.Name;

         var zadanie = await _context.Zadania
             .FirstOrDefaultAsync(z => z.Id == id);

         if (zadanie == null) return NotFound();

         _context.Zadania.Remove(zadanie);
         await _context.SaveChangesAsync();

         return NoContent();
      }
   }
}