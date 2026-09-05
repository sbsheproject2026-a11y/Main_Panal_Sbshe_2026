using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MasterSession
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public string? ShortName { get; set; }
         
        
        public DateTime? StartDate { get; set; }
    
        public DateTime? EndDate { get; set; }
        
        public DateTime? IssueDate { get; set; }

         
        public int MasterSessionId { get; set; }
        public int? IsActive { get; set; }
        public int? Status { get; set; }



    }
}
