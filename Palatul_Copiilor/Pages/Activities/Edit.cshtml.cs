using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;
using Microsoft.AspNetCore.Authorization;




namespace Palatul_Copiilor.Pages.Activities
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Palatul_CopiilorContext _context;

        public EditModel(Palatul_CopiilorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Activity Activity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            Activity = activity;

            // dropdown Department (cu selected)
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", Activity.DepartmentId);

            // dropdown Teacher (cu selected)
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", Activity.TeacherId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // IMPORTANT: reîncarcă dropdown-urile la erori
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", Activity.DepartmentId);
                ViewData["TeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", Activity.TeacherId);
                return Page();
            }

            _context.Attach(Activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(Activity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}
