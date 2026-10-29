namespace Application.DTOs
{
    public class ExamGetDTOs
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public int? TypeId { get; set; }
        public int? CategoryId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public DateTime? PaperDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? TotalQuestions { get; set; }
        public int? TotalMark { get; set; }
        public int? ParentId { get; set; }
        public int? CousreId { get; set; }
        public int? SubjectId { get; set; }
        public int? IsActive { get; set; }
        public string? TotalTimeInMinute { get; set; }
        public List<QuestionesGetDTOs>? Questiones { get; set; }

    }
    public class QuestionesGetDTOs
    {

        public int Id { get; set; }
        public string? Question { get; set; }

        public int? QuestionMarks { get; set; }

        public int? PaperId { get; set; }
        public int? IsActive { get; set; }

    }

}
