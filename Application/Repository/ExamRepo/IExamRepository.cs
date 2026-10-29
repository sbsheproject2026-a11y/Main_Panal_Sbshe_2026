using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IExamRepository
    {

        Task<List<ExamGetDTOs>> GetAll(int id);

        Task<string> CreatePaper(ExamPostDTOs model);
        Task<ExamGetDTOs> GetById(int id);
        Task<string> Update(ExamPostDTOs model);
        Task<string> Delete(int Id);
        Task<List<ExamGetDTOs>> StudentExamDetails(int id);
        Task<List<QuestionesGetDTOs>> GetExamQuestionDetails(int id);




    }
}
