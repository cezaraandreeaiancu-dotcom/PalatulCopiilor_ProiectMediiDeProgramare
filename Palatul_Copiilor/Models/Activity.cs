using System.ComponentModel.DataAnnotations;

namespace Palatul_Copiilor.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required, StringLength(160)]
        public string Title { get; set; } = string.Empty;

        [StringLength(600)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartAt { get; set; }

      
        public int DepartmentId { get; set; }
        [Display(Name = "Departament")]
        public Department? Department { get; set; }
        public int? TeacherId { get; set; }

        [Display(Name = "Profesor")]
        public Teacher? Teacher { get; set; }


        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();




    }
}
