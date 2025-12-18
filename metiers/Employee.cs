using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public int Phone { get; set; }
        [StringLength(50)]
        public string? Address { get; set; }

        public int DepartementID { get; set; }
        [ForeignKey("DepartementID")]   
        public virtual Departement Departement { get; set; }
    }
}
