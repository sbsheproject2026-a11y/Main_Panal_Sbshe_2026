using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductCategoryDetail
    {
        [Key]
        public int Id { get; set; }
        public int? ProductCategoryId { get; set; }

        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Descrption { get; set; }

        public string? SEOTitle { get; set; }

  

        public int? IsActive { get; set; }
   



    }
}
