using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowRecordID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
      

        public int LivreID { get; set; }
        [ForeignKey("LivreID")]
        public virtual Livre Livre { get; set; }
        public string UserId { get; set; }
    }
}
