using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository
{
    public interface ICourseRepository
    {

        // Task<List<CourseGetDTOs>> GetAll(int ReferenceId, int pageNo, int pageSize, string? search);
        //Task<List<CourseGetDTOs>> GetAll();
        Task<List<CourseGetDTOs>> GetAll(int? studyModeId, int? departmentId, int? categoryId, int? parentId, string? search);

        Task<string> Create(CoursePostDTOs model);
        Task<CourseGetDTOs> GetById(int id);
        Task<string> Update(CoursePostDTOs model);
        Task<string> Delete(int Id);

        //ProductAmount  Working Start//
        Task<string> SetFranchiseAmount(CommissionDtos item);
        Task<string> SetCourseCommission(CommissionDtos item);
        Task<string> CreateCourseAmount(List<ProductAmountDtos> model);
        Task<List<GetProductAmountDtos>> GetProductAmount(int id);

        Task<GetProductAmountDtos> GetProductAmountById(int id);

        Task<string> UpdateProductAmount(ProductAmountDtos model);
        Task<string> DeleteProductAmount(int Id);

        //ProductAmount  Working End//

        Task<string> CreateCourseMaterial(ProductMaterialDtos model);

        Task<List<ProductMaterialGetDtos>> getCourseMaterial(int id);
        Task<string> DeleteCourseMaterial(int Id);
        Task<List<CourseGetDTOs>> GetStudentCourseDetailById(int id);
        Task<ProductMaterialGetDtos?> getEditCourseMaterial(int id);
        Task<string> UpdateCourseMaterial(ProductMaterialDtos model);






    }
}
