using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
     
        public string? Name { get; set; }

        public string? Code { get; set; }
        public DateTime? Date { get; set; }

        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
        public int? IsDelete { get; set; }



    }
}
