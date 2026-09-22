using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductPersonalDetail
    {
        [Key]
        public int Id { get; set; }
     
        
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TotalClass { get; set; }
        public int? ClassNo { get; set; }



    }
}
