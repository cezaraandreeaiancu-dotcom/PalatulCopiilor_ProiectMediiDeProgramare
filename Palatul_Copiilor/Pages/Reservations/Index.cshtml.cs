using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Pages.Reservations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public IndexModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservation { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var query = _context.Reservation
                .Include(r => r.Activity)
                .Include(r => r.Review)
                .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                query = query.Where(r => r.UserId == userId);
            }

            Reservation = await query.ToListAsync();
        }
    }
}
