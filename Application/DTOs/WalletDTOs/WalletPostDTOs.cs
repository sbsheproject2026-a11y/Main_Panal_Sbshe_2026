
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{ 
    public class WalletPostDTOs
    {
      
        public decimal? TotalAmount { get; set; }

        public string? Remark { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? FranchiseId { get; set; }
        public int? PayMode { get; set; }
        public int? LedgerHeadId { get; set; }
        public int? Status { get; set; }
        public int? ProductId { get; set; }
        public int? AccountId { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? BankNameId { get; set; }
        public DateTime? BankConfirmDate { get; set; }
        public string? TransactionNo { get; set; }
        public string? SlipUpload { get; set; }
        public IFormFile? SlipUpload1 { get; set; }
       
    }
    public class ApplyStudentPostDTOs
    {
      
        public decimal? Amount { get; set; }

        public string? Remark { get; set; }
        public string? Address { get; set; }
       
        public int? UserId { get; set; }
      
        public int? CourseId { get; set; }
        public int? Quantity { get; set; }
       
    }
}
