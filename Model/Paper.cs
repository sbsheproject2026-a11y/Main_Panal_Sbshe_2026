using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Paper
    {
        [Key]
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

    }
    public class PaperAttempt
    {
        [Key]
        public int Id { get; set; }
        public int? PaperId { get; set; }
        public int? UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? IsActive { get; set; }

    }
    public class PaperAttemptDocument
    {
        [Key]
        public int Id { get; set; }
        public int? PaperId { get; set; }
        public int? UserId { get; set; }
        public int? PaperAttemptId { get; set; }
        public string? Path { get; set; }
        public int? IsActive { get; set; }

    }
    public class Questiones
    {
        [Key]
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? Descirption { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public int? QuestionCategory { get; set; }
        public int? OrderBy { get; set; }
        public int? PaperId { get; set; }
        public int? QuestionMarks { get; set; }
        public int? IsActive { get; set; }

    }




}
