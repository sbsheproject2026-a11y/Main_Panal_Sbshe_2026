using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class LecturePostDTOs
    {
        public int Id { get; set; }

     
        public string? Title { get; set; }

        public string? Code { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int? TheoryMarks { get; set; }
        public int? PracticalMarks { get; set; }
        public int? AssignmentMarks { get; set; }
        public int? MinMarks { get; set; }
        public int? MaxMarks { get; set; }
        public int? IsActive { get; set; }
        public int? CourseId { get; set; }
        public List<SubjectRequest>? Subjects { get; set; } = new List<SubjectRequest>();
    }

    public class SubjectRequest
    {
        public string SubjectName { get; set; }  

        public string SubjectCode { get; set; } 
    }

}
