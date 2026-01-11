using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Pages.Reservations
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public CreateModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        // doar pentru afișare (titlul activității)
        public string ActivityTitle { get; set; } = "";

        public IActionResult OnGet(int? activityId)
        {
            if (activityId == null)
                return NotFound();

            var activity = _context.Activity.FirstOrDefault(a => a.Id == activityId.Value);
            if (activity == null)
                return NotFound();

            ActivityTitle = activity.Title;

            Reservation = new Reservation
            {
                ActivityId = activity.Id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // completări automate
            Reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Reservation.ParticipantName = User.Identity?.Name;
            Reservation.ReservedAt = DateTime.Now;

            // reafișăm titlul activității în caz că rămânem pe pagină
            ActivityTitle = await _context.Activity
                .Where(a => a.Id == Reservation.ActivityId)
                .Select(a => a.Title)
                .FirstOrDefaultAsync() ?? "";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ✅ verificare anti-duplicat (user + activity)
            var exists = await _context.Reservation.AnyAsync(r =>
                r.UserId == Reservation.UserId &&
                r.ActivityId == Reservation.ActivityId);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Ai deja o rezervare pentru această activitate.");
                return Page();
            }

            try
            {
                _context.Reservation.Add(Reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // fallback dacă ai și index unic în DB (sau concurență)
                ModelState.AddModelError(string.Empty, "Rezervarea există deja pentru această activitate.");
                return Page();
            }

            return RedirectToPage("/Reservations/Index");
        }
    }
}
