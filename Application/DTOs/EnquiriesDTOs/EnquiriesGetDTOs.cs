using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MasterTypeDTOs
{
    public class EnquiriesGetDTOs
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Course { get; set; }
        public int? CourseId { get; set; }
        public string? Address { get; set; }
        public string? EnquiryDate { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedDate { get; set; }
        public int? TypeId { get; set; }
        public int? IsActive { get; set; }
    }
}
