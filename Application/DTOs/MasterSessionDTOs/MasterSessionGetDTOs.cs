using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class MasterSessionGetDTOs
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string? ShortName { get; set; }

    
        public DateTime? StartDate { get; set; }
   
        public DateTime? EndDate { get; set; }
 
        public DateTime? IssueDate { get; set; }
        public string? StartDate1 { get; set; }
   
        public string? EndDate1 { get; set; }
 
        public string? IssueDate1 { get; set; }

         
        public int MasterSessionId { get; set; }
        public int? IsActive { get; set; }
        public int? Status { get; set; }
    }
}
