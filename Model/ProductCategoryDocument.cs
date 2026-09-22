using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductCategoryDocument
    {
        [Key]
        public int Id { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? DocExtensionId { get; set; }
        public int? SrNo { get; set; }

        public string? Title { get; set; }
        public string? AltTag { get; set; }
        public DateTime? Date { get; set; }
        public string? Path { get; set; }

        public int? IsActive { get; set; }


    }
}
