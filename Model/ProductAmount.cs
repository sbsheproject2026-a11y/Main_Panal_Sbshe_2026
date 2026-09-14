using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProductAmount
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public DateTime? Date { get; set; }

        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public int? AmountTypeId { get; set; }
        public int? IsActive { get; set; }




    }
}
