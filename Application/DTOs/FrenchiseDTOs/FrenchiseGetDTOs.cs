namespace Application.DTOs
{
    public class FrenchiseGetDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }
        public string? InstituteName { get; set; }

        public int? ReferenceId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }

        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public DateTime? Dob { get; set; }

        public string? DOB1 { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? Email { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
        public string? StateName { get; set; }
        public string? EmployeeName { get; set; }

        public string? AadhaarCardimage { get; set; }

        public string? PanCardimage { get; set; }

        public string? PassportPhoto { get; set; }

        public string? AuthorizedSignature { get; set; }

        public string? InstituteCertificate { get; set; }

        public string? BuildingPhoto1 { get; set; }

        public string? BuildingPhoto2 { get; set; }

        public int? InstituteTypeId { get; set; }
        public int? NumberofClassrooms { get; set; }
        public int? ComputerLabAvailable { get; set; }
        public int? NumberofComputers { get; set; }
        public string? Affiliation { get; set; }
        public string? Website { get; set; }
        public string? TotalArea { get; set; }
        public string? OtherFacilities { get; set; }
        public string? AadhaarCardNumber { get; set; }
        public string? PANCardNumber { get; set; }
        public string? Remark { get; set; }
        public string? Experience { get; set; }


    }

    public class OrderList
    {
        public string SrNo { get; set; }
        public int EId { get; set; }
        public int OId { get; set; }
        public string FeeName { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string OrderId { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PendingAmount { get; set; }
        public string PayStatus { get; set; }
        public string ODate { get; set; }
        public string Address { get; set; }
        public string PrintDate { get; set; }
        public string PrintPayDate { get; set; }
        public string AmountW { get; set; }

        public int ProductId { get; set; }
        public string GetwayPrm { get; set; }

    }
    public class OfflinePayPaymentDTOs
    {
      
        public int EId { get; set; }
         
        public decimal Amount { get; set; }
        public string PayStatus { get; set; }
        public string ODate { get; set; }
        public string Address { get; set; }
        public string PrintDate { get; set; }
        public string PrintPayDate { get; set; }
        public string AmountW { get; set; }

        public int ProductId { get; set; }
        public string GetwayPrm { get; set; }

    }

    public class OfflinePaymentRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string remark { get; set; }
        public DateTime? PaidDate { get; set; }
        public int courseid { get; set; }
    }

}
