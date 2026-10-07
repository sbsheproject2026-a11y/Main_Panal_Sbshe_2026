namespace Application.DTOs
{
    public class StudentGetDTOs
    {
        public int? Id { get; set; }
        public int? PuId { get; set; }
        public int? Amount { get; set; }
        public string? Code { get; set; }
        public string? EnrollmentNo { get; set; }
        public string? Rollno { get; set; }
        public string? RollNo1 { get; set; }

        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? StudentNameHindi { get; set; }

        public int? ReferenceId { get; set; }

        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public string? FatherName1 { get; set; }
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
        public int? EntityTypeId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? IdNumber { get; set; }
        public string? InstituteName { get; set; }
        public string? Relacition { get; set; }
        public string? EntityType { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
        public string? StateName { get; set; }
        public string? SelfImageShow { get; set; }
        public string? SignatureImageShow { get; set; }
        public string? AadhaarCardFrantShow { get; set; }
        public string? AadhaarCardBackShow { get; set; }
        public string? CourseName { get; set; }
        public string? CourseNameHindi { get; set; }

        public string? SelfImage { get; set; }


        public string? SignatureImage { get; set; }

        public string? AadhaarCardFrant { get; set; }

        public string? AadhaarCardBack { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string? AdmissionDate1 { get; set; }
        public string? IssueDate { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? IssueYear { get; set; }
        public string? SrnoDiploma { get; set; }
        public string? MigrationSrno { get; set; }
        public string? DepartmentName { get; set; }
        public string? CertificateName { get; set; }
        public DateTime? DOB { get; set; }
        public string? DOB1 { get; set; }
        public string? Session { get; set; }
        public string? StudentYear { get; set; }
        public string? CentreName { get; set; }
        public string? CourseCode { get; set; }
        public string? Duration { get; set; }
        public string? BranchAddress { get; set; }
        public int? CasteCategory { get; set; }
        public int? CourseId { get; set; }
        public int? ClassId { get; set; }
        public int? ExamSessionId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public int? CasteCategoryId { get; set; }
        public int? CourseCategoryId { get; set; }
        public int? CourseTypeId { get; set; }
        public int? TheoryMarks { get; set; }
        public int? PracticalMarks { get; set; }
        public int? AssignmentMarks { get; set; }
        public int? MaxMarks { get; set; }
        public int? TotalMarks { get; set; }
        public string? Grade { get; set; }
        public string? Result { get; set; }
        public List<AcademicDetailGetDTO>? AcademicDetails { get; set; }

    }
    public class GetStudentMarksGetDTOs
    {

        public string? Code { get; set; }

        public string? Rollno { get; set; }

        public string? Name { get; set; }

        public string? CourseName { get; set; }
        public string? Grade { get; set; }

        public int? TheoryMarks { get; set; }
        public int? PracticalMarks { get; set; }
        public int? AssignmentMarks { get; set; }
        public int? MaxMarks { get; set; }
        public int? TotalMarks { get; set; }
        public int? PuId { get; set; }
        public int? SubjectId { get; set; }

        public string? TheoryMarks1 { get; set; }
        public string? PracticalMarks1 { get; set; }
        public string? AssignmentMarks1 { get; set; }
        public string? MaxMarks1 { get; set; }
        public string? TotalMarks1 { get; set; }


    }

    public class GetStudentSubjectMarksGetDTOs
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Grade { get; set; }
        public string? TheoryMarks { get; set; }
        public string? PracticalMarks { get; set; }
        public string? AssignmentMarks { get; set; }
        public string? MaxMarks { get; set; }
        public string? TotalMarks { get; set; }


    }

    public class StudentMarksDto
    {
        public List<StudentMarkItemDto> Marks { get; set; } = new();
    }

    public class StudentMarkItemDto
    {
        public int Pid { get; set; }
        public int Sid { get; set; }

        public int TheoryMarks { get; set; }
        public int PracticalMarks { get; set; }
        public int AssignmentMarks { get; set; }
    }
    public class StudentClassDto
    {
        
        public int ExamSessionId { get; set; }
        public List<int> PUid { get; set; } = new List<int>();
    }




}
