using System;
using System.ComponentModel.DataAnnotations;

namespace Palatul_Copiilor.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
       
        public int ActivityId { get; set; }
        [Display(Name = "Activitate")]

        public Activity? Activity { get; set; }

        [Required]
        [Display(Name = "Data rezervării")]
        public DateTime ReservedAt { get; set; } = DateTime.Now;

        [Display(Name = "Email Participant")]
        public string? ParticipantName { get; set; }

        // legătura cu utilizatorul logat (Identity)
        
        public string UserId { get; set; } = string.Empty;
        public Review? Review { get; set; }
    }
}

