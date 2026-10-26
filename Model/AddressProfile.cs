using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class AddressProfile
    {
        [Key]
        public int Id { get; set; }
   
        public string? LandMark { get; set; }
        public string? Pincode { get; set; }
        public string? Address { get; set; }
        public int? EntityId { get; set; }
        public int? AddressTypeId { get; set; }
        public int? LocationId { get; set; }
        public int? IsActive { get; set; }


    }
}
