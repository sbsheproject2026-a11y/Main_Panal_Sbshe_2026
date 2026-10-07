using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MasterTypeDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Code { get; set; }
        public DateTime? Date { get; set; }
        public string? FilePath { get; set; }

        public int? ParentId { get; set; }
        [Required]
        public int MasterTypeId { get; set; }
        public int? SrNo { get; set; }
        public int? IsActive { get; set; }
        public int? IsDelete { get; set; }



    }
}
