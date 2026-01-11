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



namespace Palatul_Copiilor.Pages.Teachers
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Palatul_Copiilor.Data.Palatul_CopiilorContext _context;

        public CreateModel(Palatul_Copiilor.Data.Palatul_CopiilorContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Teacher.Add(Teacher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
