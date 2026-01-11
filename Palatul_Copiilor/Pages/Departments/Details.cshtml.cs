using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;

namespace Palatul_Copiilor.Pages.Departments
{
    public class DetailsModel : PageModel
    {
        private readonly Palatul_Copiilor.Data.Palatul_CopiilorContext _context;

        public DetailsModel(Palatul_Copiilor.Data.Palatul_CopiilorContext context)
        {
            _context = context;
        }

        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }
    }
}
