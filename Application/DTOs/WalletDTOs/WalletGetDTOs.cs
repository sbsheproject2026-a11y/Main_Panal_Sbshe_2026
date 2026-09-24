using System.ComponentModel.DataAnnotations;

namespace Application.DTOs 
{
    public class WalletGetDTOs
    {
        public int? Id { get; set; }
        public int? FundId { get; set; }
        public string? balance { get; set; }
        public decimal? dr { get; set; }
        public decimal? cr { get; set; }

        public string? Remark { get; set; }
        public string? Branchname { get; set; }
        public string? Slip { get; set; }
        public DateTime? EntryDate { get; set; }
       
       
        public string? TransactionType { get; set; }
        public string? TransactionNumber { get; set; }
        public string? Status { get; set; }
        public string? SlipUpload { get; set; }
    }
}
