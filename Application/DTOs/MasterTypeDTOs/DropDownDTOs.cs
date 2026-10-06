using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class DropDownDTOs
    {
        public int Id { get; set; }
       
        public string? Name { get; set; }

        
    }
    public class DropDownDTO1s
    {
        public int Id { get; set; }
       
        public string? Name { get; set; }
        public string? Amount { get; set; }

        
    }
    public class UpdatePasswordDTOs
    {

        public int Id { get; set; }
        public string? HashPassword { get; set; }
        public string? Password { get; set; }

        
    }

    
}
