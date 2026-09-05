using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class StudentMarks
    {
        [Key]
        public int Id { get; set; }
      
        
        public DateTime? CreatedAt { get; set; }

        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public int? TheoryMarks { get; set; }
        public int? PracticalMarks { get; set; }
        public int? AssignmentMarks { get; set; }
   
        public int? IsActive { get; set; }
   




    }
}
