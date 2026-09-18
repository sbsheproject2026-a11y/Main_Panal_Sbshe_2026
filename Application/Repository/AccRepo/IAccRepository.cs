using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IAccRepository
    {

        Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAll(int? ReferenceId, int? pageNo, int? pageSize, string? search);

        Task<string> Create(AccRegistrationPostDTOs model);
        Task<FrenchiseGetDTOs> GetById(int id);
        Task<string> Update(AccRegistrationPostDTOs model);
        Task<string> Delete(int Id);
        
         
    }
}
