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
}
