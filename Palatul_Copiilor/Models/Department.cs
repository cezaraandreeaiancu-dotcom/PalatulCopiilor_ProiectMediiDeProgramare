using System.ComponentModel.DataAnnotations;

namespace Palatul_Copiilor.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Display(Name="Nume Departament")]
        public string Name { get; set; } = string.Empty;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
