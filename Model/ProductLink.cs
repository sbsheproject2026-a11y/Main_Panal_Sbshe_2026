using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductLink
    {
        [Key]
        public int Id { get; set; }
     
        
        public int? ProductId { get; set; }
        public int? ProductCategoryId { get; set; }
         
 


    }
}
