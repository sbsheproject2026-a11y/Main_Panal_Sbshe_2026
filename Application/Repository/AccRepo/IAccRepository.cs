using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IAccRepository
    {

        Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAll(int typeid,int? ReferenceId, int? pageNo, int? pageSize, string? search);
        Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAllPendingList(int typeid, int? referenceId, int? pageNo, int? pageSize, string? search);
        Task<string> Create(AccRegistrationPostDTOs model);
        Task<FrenchiseGetDTOs> GetById(int id);
        Task<string> Update(AccRegistrationPostDTOs model);
        Task<string> Delete(int Id);
        Task<string> ConfirmFranchise(int id, int sts);



    }
}
