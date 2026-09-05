using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class MasterSessionPostDTOs
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public int MasterSessionId { get; set; }
        public int? IsActive { get; set; }
        public int? Status { get; set; }
    }
}
