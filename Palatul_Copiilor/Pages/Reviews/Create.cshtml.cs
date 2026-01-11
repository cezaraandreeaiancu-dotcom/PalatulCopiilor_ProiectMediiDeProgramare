using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Pages.Reviews
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
        public Review Review { get; set; } = new Review();

        public async Task<IActionResult> OnGetAsync(int? reservationId)
        {
            if (reservationId == null)
                return NotFound();

            var reservation = await _context.Reservation
                .Include(r => r.Review)
                .FirstOrDefaultAsync(r => r.Id == reservationId.Value);

            if (reservation == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // doar owner-ul rezervării poate lăsa review
            if (!User.IsInRole("Admin") && reservation.UserId != userId)
                return Forbid();

            // o singură recenzie per rezervare
            if (reservation.Review != null)
                return RedirectToPage("/Reservations/Index");

            Review = new Review { ReservationId = reservation.Id };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // dacă hidden nu a venit, nu avem ce salva
            if (Review.ReservationId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Rezervarea nu este validă.");
                return Page();
            }

            // verificăm în DB că rezervarea există și aparține userului
            var reservation = await _context.Reservation
                .Include(r => r.Review)
                .FirstOrDefaultAsync(r => r.Id == Review.ReservationId);

            if (reservation == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Admin") && reservation.UserId != userId)
                return Forbid();

            if (reservation.Review != null)
            {
                ModelState.AddModelError(string.Empty, "Ai trimis deja un review pentru această rezervare.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Review.Add(Review);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Nu s-a putut salva review-ul. (Poate există deja.)");
                return Page();
            }

            return RedirectToPage("/Reservations/Index");
        }
    }
}
