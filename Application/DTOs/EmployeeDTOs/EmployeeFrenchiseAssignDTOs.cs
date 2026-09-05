using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs
{
    public class EmployeeFrenchiseAssignDTOs
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public List<int> FrenchiseIds { get; set; }
        public int IsActive { get; set; }
    }
}
