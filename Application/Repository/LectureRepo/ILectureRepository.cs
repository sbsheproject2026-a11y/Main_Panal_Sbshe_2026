using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface ILectureRepository
    {

        Task<List<LectureGetDTOs>> GetAll(int id);

        Task<string> CreateSubject(LecturePostDTOs model);
        Task<LectureGetDTOs> GetById(int id);
        Task<string> Update(LecturePostDTOs model);
        Task<string> Delete(int Id);

      

    }
}
