using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Pages.Activities
{
    public class DetailsModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public DetailsModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        public Activity Activity { get; set; } = default!;

        // ✅ Reviews anonime pentru activitatea curentă
        public IList<Review> Reviews { get; set; } = new List<Review>();
        public double? AverageRating { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // Include Department ca să poți afișa Activity.Department.Name fără null
            var activity = await _context.Activity
                .Include(a => a.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activity == null)
                return NotFound();

            Activity = activity;

            // ✅ Încărcăm review-urile (prin Reservation -> ActivityId)
            Reviews = await _context.Review
                .Include(r => r.Reservation)
                .Where(r => r.Reservation != null && r.Reservation.ActivityId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            AverageRating = Reviews.Count == 0 ? null : Reviews.Average(r => r.Rating);

            return Page();
        }
    }
}
