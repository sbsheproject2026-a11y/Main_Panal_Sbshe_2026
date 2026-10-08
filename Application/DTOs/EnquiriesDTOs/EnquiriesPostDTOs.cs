using Microsoft.AspNetCore.Http;

namespace Application.DTOs.MasterTypeDTOs
{
    public class EnquiriesPostDTOs
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Course { get; set; }
        public int? CourseId { get; set; }
        public string? Address { get; set; }

        public string? Status { get; set; }
        public string? Remarks { get; set; }

        public int? TypeId { get; set; }
        public int? IsActive { get; set; }
    }

    public class RTIApplicationsDTOs
    {

        public long? Id { get; set; }

        // RTI Tracking
        public string? TrackingId { get; set; }

        // Applicant Info
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        // RTI Details
        public string? Subject { get; set; }
        public string? InfoRequired { get; set; }

        // Applicant Type (FK → MasterTypeDetail)
        public int? ApplicantTypeId { get; set; }


        // Status (FK → MasterTypeDetail)
        public int? StatusId { get; set; }

        public string? Remarks { get; set; }


        public string? Code { get; set; }

        // Common
     

        public int? TypeId { get; set; }
        public IFormFile? Path { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Path1 { get; set; }
        public string? FilePath1 { get; set; }
    }
    public class RTIApplicationsGetDTOs
    {

        public long? Id { get; set; }

        // RTI Tracking
        public string? TrackingId { get; set; }

        // Applicant Info
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? StatusName { get; set; }
        public string? ApplicantType { get; set; }

        // RTI Details
        public string? Subject { get; set; }
        public string? InfoRequired { get; set; }

        // Applicant Type (FK → MasterTypeDetail)
        public int? ApplicantTypeId { get; set; }


        // Status (FK → MasterTypeDetail)
        public int? StatusId { get; set; }

        public string? Remarks { get; set; }


        public string? Code { get; set; }

        // Common
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int? TypeId { get; set; }
        public IFormFile? Path { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Path1 { get; set; }
        public string? FilePath1 { get; set; }
    }

    public class UpdateRTIStatusDTO
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public string? Remarks { get; set; }
    }


}
