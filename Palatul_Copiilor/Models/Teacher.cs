using System.ComponentModel.DataAnnotations;

namespace Palatul_Copiilor.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [StringLength(120)]
        [Display(Name = "Nume profesor")]
        public string FullName { get; set; } = string.Empty;


        [EmailAddress]
        [StringLength(120)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(30)]
        public string? Phone { get; set; }

        // Un profesor -> mai multe activități
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
