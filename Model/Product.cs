using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
     
        public string? Code { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? CourseNameHindi { get; set; }
        public int? ProductCategoryId { get; set; }

        public DateTime? Date { get; set; }

        public string? Eligbilty { get; set; }
        public int? Duration { get; set; }
        public int? DurationTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
        public int? IsDelete { get; set; }



    }
}
