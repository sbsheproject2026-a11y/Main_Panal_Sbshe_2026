using Application.DTOs;

namespace Application.Repository
{
    public interface IStudentRepository
    {

        Task<(List<StudentGetDTOs> StudentList, int TotalCount)> GetAll(int referenceId, int? pageNo, int? pageSize, string? search, int? studyModeId);
        Task<List<StudentGetDTOs>> GetSendToConfirm(int ReferenceId, int pageNo, int pageSize, string? search);
        Task<List<StudentGetDTOs>> GetConfirmToAdmin( int pageNo, int pageSize, string? search, int studyModeId);
        Task<(List<StudentGetDTOs> StudentList, int TotalCount)> GetConfirmAddmissions(int? referenceId, int? pageNo, int? pageSize, string? search, int? studyModeId);
        Task<string> Create(StudentPostDTOs model);
        Task<string> CreateNew(StudentPostDTOs model);
        Task<string> CreateAcademicDetail(int entityId, AcademicDetailDTO model);
        Task<StudentGetDTOs> GetById(int id);
        Task<StudentGetDTOs> GetStudentDetailsPrint(int id);
        Task<string> Update(StudentPostDTOs model);
        Task<string> Delete(int Id);
        Task<string> insSendForConfirm(int Id, int FranchiseId, int ParentFranchiseId, int TotalAmount, string unic6);
        Task<string> ConfirmToAdmin(int Id);

        Task<List<StudentGetDTOs>> UnSetMarksStudents(int? referenceId, int pageNo, int pageSize, string? search);
        Task<List<GetStudentMarksGetDTOs>> getStudentSubjectMarks(string rollno);

        Task<string> SaveStudentMarks(StudentMarksDto model);

        Task<List<StudentGetDTOs>> GetUpgradeStudentList(int pageNo, int pageSize, string? search);
        Task<List<StudentGetDTOs>> GetOnlineUpgradeStudentList(int pageNo, int pageSize, string? search);
     

        Task<string> AddClassStudent(StudentClassDto model);
        Task<string> OnlineAddClassStudentNew(StudentClassDto model);
        Task<List<AcademicDetailGetDTO>> GetAcademicDetailsById(int id);
        Task<string> viewbalview(int FranchiseId, int aa);
        Task<List<StudentGetDTOs>> GetStudentProfileData(int? referenceId);
        Task<string> CreateBookDeliveryAddress(ApplyStudentPostDTOs model);
        Task<string> GetGenerateUserCode(int Id, int Code);
        Task<string> StudentUpgradeRequest(int Id);

    }
}
