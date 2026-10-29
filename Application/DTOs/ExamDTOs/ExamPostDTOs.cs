namespace Application.DTOs
{
    public class ExamPostDTOs
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public DateTime? PaperDate { get; set; }

        public int? TotalQuestions { get; set; }
        public int? TotalMark { get; set; }
        public int? ParentId { get; set; }
        public int? CousreId { get; set; }
        public int? SubjectId { get; set; }
        public int? IsActive { get; set; }
        public string? TotalTimeInMinute { get; set; }
        public List<QuestionesPostDTOs>? Questiones { get; set; }

    }
    public class QuestionesPostDTOs
    {

        public string? Question { get; set; }
        public int? QuestionMarks { get; set; }


    }



}
