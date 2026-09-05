using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MasterLogin
    {
        [Key]
        public int Id { get; set; }

        public int? EntityId { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }
        public string? HashPassword { get; set; }
        public DateTime? Date { get; set; }

        public int? RoleId { get; set; }
        public int? IsActive { get; set; }



    }
}
