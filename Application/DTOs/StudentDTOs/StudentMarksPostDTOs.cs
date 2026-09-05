using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class StudentMarksPostDTOs
    {
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
