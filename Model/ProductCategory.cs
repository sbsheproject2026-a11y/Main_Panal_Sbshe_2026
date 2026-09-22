using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public int? SrNo { get; set; }
        public int? ProductTypeId { get; set; }

        public string? Code { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }
         
        public DateTime? Date { get; set; }


        public int? IsActive { get; set; }
        public int? IsHighlight { get; set; }
        public int? IsDelete { get; set; }



    }
}
