using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;
using Microsoft.AspNetCore.Authorization;




namespace Palatul_Copiilor.Pages.Activities
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public CreateModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Activity Activity { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reîncarcă dropdown-urile dacă există erori
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
                ViewData["TeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
                return Page();
            }

            _context.Activity.Add(Activity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
