using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Model;

namespace Application.Repository 
{
    public interface IExamRepository
    {

        Task<List<ExamGetDTOs>> GetAll(int id);

        Task<string> CreatePaper(ExamPostDTOs model);
        Task<ExamGetDTOs> GetById(int id);
        Task<string> Update(ExamPostDTOs model);
        Task<string> Delete(int Id);
        Task<List<ExamGetDTOs>> StudentExamDetails(int courseId, int userId);
        Task<List<QuestionesGetDTOs>> GetExamQuestionDetails(int id);
        Task<PaperAttempt> ExamStart(int Id, int UserId);
        Task<PaperAttempt> ExamEnd(SubmitExamDto model);
        Task<List<StudentGetDTOs>> GetStudentAllResult(int referenceId);
        Task<List<StudentGetDTOs>> GetSubmitStudentAllResult(int referenceId);




    }
}
