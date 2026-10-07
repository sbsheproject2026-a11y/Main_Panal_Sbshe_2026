using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class StudentPostDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }
        public string? StudentNameHindi { get; set; }

        public int? ReferenceId { get; set; }
        public int? StudyModeId { get; set; }
        public int? EmployeeId { get; set; }

        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public string? FatherNameHindi { get; set; }
        public string? MotherName { get; set; }
        public string? MotherNameHindi { get; set; }
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? Email { get; set; }
        public int? GenderId { get; set; }
        public int? IsActive { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? IdNumber { get; set; }
        public IFormFile? SelfImage1 { get; set; }
        public string? SelfImage { get; set; }

        public IFormFile? SignatureImage1 { get; set; }
        public string? SignatureImage { get; set; }
        public IFormFile? AadhaarCardFrant1 { get; set; }
        public string? AadhaarCardFrant { get; set; }
        public IFormFile? AadhaarCardBack1 { get; set; }
        public string? AadhaarCardBack { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DOB { get; set; }
        public string? DOB1 { get; set; }
        public int? CasteCategory { get; set; }
        public int? CourseId { get; set; }
        public int? ClassId { get; set; }
        public int? ExamSessionId { get; set; }

        public List<AcademicDetailDTO>? AcademicDetails { get; set; }
    }

    public class AcademicDetailDTO
    {
        public int Index { get; set; }
        public int? Id { get; set; }
        public int? LevelTypeId { get; set; }
        public int? EntityId { get; set; }
 
 
        public string? SchoolCollege { get; set; }
        public string? RollNo { get; set; }
        public string? BoardUniversity { get; set; }
        public string? PercentageCgpa { get; set; }
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
    }
    public class AcademicDetailGetDTO
    {
 
        public int? Id { get; set; }
        public int? LevelTypeId { get; set; }
        public int? EntityId { get; set; }
 
 
        public string? SchoolCollege { get; set; }
        public string? RollNo { get; set; }
        public string? BoardUniversity { get; set; }
        public string? PercentageCgpa { get; set; }
    
        public string? FileName { get; set; }
    }

    public class SendToConfirmDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Amount { get; set; }
    }
}
