using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BasicProfile
    {
        [Key]
        public int Id { get; set; }
   
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int? EntityId { get; set; }


    }
    public class StudentNotes
    {
        [Key]
        public int Id { get; set; }
   
        public int? EntityId { get; set; }
        public string? Title { get; set; }
        public string? NoteContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IsActive { get; set; }


    }
}
