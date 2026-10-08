using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class RTIApplications
    {
        [Key]
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
        public int? IsActive { get; set; }
     
        public string? Remarks { get; set; }

    
        public string? Code { get; set; }

        // Common
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }

    public class RTIDocuments
    {
        [Key]
        public long? Id { get; set; }
        public int? TypeId { get; set; }

        // FK → RTIApplications
        public long? RTIApplicationsId { get; set; }

        // Document Info
        
        public string? FilePath { get; set; }
        public string? LetterHeadImage { get; set; }


        // Common
        public DateTime? CreatedAt { get; set; }
        public DateTime? UploadedAt { get; set; }

    }


}
