using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class LectureGetDTOs
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
    }
}
