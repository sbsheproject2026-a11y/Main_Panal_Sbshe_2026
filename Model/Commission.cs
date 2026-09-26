using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }

   
        public int? DepartmentId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }

        public int? IsActive { get; set; }




    }
    public class FranchiseCommission
    {
        [Key]
        public int Id { get; set; }

        public int? FranchiseId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }

        public int? IsActive { get; set; }




    }
}
