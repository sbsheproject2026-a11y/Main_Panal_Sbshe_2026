using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Code { get; set; }
        public DateTime? Date { get; set; }
        public int  TypeId { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
       
    }
}
