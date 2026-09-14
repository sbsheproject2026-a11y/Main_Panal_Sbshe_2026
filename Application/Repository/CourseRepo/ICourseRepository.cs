using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository
{
    public interface ICourseRepository
    {

        Task<List<CourseGetDTOs>> GetAll(int ReferenceId, int pageNo, int pageSize, string? search);

        Task<string> Create(CoursePostDTOs model);
        Task<CourseGetDTOs> GetById(int id);
        Task<string> Update(CoursePostDTOs model);
        Task<string> Delete(int Id);
        Task<string> CreateCourseAmount(List<ProductAmountDtos> model);




    }
}
