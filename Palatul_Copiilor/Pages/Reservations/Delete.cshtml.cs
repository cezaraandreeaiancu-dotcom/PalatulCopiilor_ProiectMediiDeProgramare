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
    [Authorize] // Admin + Participant au voie să ajungă aici, dar verificăm ownership-ul
    public class DeleteModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public DeleteModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = await _context.Reservation
                .Include(r => r.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
                return NotFound();

            // Participant poate șterge doar rezervările lui
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (reservation.UserId != userId)
                    return Forbid();
            }

            Reservation = reservation;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            // Participant poate șterge doar rezervările lui
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (reservation.UserId != userId)
                    return Forbid();
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
