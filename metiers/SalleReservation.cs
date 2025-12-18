using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class SalleReservation
    {
        [Key]
        public int SalleReservationID { get; set; }

        [Required(ErrorMessage = "SalleReservationName is required")]
        [StringLength(100, MinimumLength = 2)]

        public string SalleReservationName { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = false;
        public int SalleId { get; set; }
        [ForeignKey("SalleId")]
       public virtual Salle Salle { get; set; }
        
        public string? UserId { get; set; }

    }


}
