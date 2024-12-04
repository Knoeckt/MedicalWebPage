using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicalWebPage.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateTimeCollected { get; set; } = DateTime.Now;

        // Foreign key relationship to Identity User
        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
        [Required]
        public double Volume { get; set; }
        [Required]
        public double Temperature { get; set; }
        [Required]
        public List<int> RGB { get; set; } = [];
    }
}
