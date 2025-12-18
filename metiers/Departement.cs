using System.ComponentModel.DataAnnotations;

namespace metiers
{
    public class Departement
    {
        [Key]
        public int DepartementID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartementName { get; set; }
    }
}
