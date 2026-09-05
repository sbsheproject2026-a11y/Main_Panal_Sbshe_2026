using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class AcademicDetails
    {
        [Key]
        public int Id { get; set; }
        public int? LevelTypeId { get; set; }
        public int? EntityId { get; set; }
   
        public string? SchoolCollege { get; set; }
        public string? RollNo { get; set; }
        public string? BoardUniversity { get; set; }
        public string? PercentageCgpa { get; set; }
        public string? FileName { get; set; }
        


    }
}
