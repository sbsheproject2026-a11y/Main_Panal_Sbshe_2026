using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmployeeFrenchiseAssign
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int FrenchiseId { get; set; }
        public int IsActive { get; set; }
    }
}
