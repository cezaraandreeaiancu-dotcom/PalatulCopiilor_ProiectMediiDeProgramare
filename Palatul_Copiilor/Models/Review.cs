using System.ComponentModel.DataAnnotations;

namespace Palatul_Copiilor.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
