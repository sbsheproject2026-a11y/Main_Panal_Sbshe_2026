using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IFrenchiseRepository
    {

        Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAll(int? ReferenceId, int? pageNo, int? pageSize, string? search);

        Task<string> Create(FrenchisePostDTOs model);
        Task<string> CreateInstituteDepartmentDetails(int entityId, FranchiseDepartmentDTOs model);
        Task<FrenchiseGetDTOs> GetById(int id);
        Task<FrenchiseGetDTOs> GetDetaiId(int id);
        Task<string> Update(FrenchisePostDTOs model);
        Task<string> UpdateDocument(FrenchisePostDTOs model);
        Task<string> UpdatePassword(FranchiseUpdatePasswordDTOs model);
        Task<string> Delete(int Id);
        Task<List<FrenchiseGetDTOs>> GetFranchiseDepartmentDetail(int id);
        Task<string> DeleteDepartment(int Id);



    }
}
