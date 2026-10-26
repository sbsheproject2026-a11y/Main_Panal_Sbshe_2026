using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class DropDownDTOs
    {
        public int Id { get; set; }
       
        public string? Name { get; set; }

        
    }
    

    public class AmountDetailRequest
    {
        public int UserId { get; set; }
        public int OId { get; set; }
    }

    public class AfterPaymentRequest
    {
        public string R_orderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
        public string PaymentStatus { get; set; }
        public int UserId { get; set; }
        public int OId { get; set; }
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
