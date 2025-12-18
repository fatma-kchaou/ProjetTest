using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class Salle
    {
       

        [Key]
        public int SalleID { get; set; }
        [Required(ErrorMessage = "SalleName is required")]
        [StringLength(100, MinimumLength = 2)]
        public string SalleName { get; set; }   // e.g. Salle 1, Salle A
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<SalleReservation> Reservations { get; set; }
    }
}
