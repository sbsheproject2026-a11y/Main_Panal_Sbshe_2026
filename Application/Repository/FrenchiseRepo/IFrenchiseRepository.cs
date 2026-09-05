using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IFrenchiseRepository
    {

        Task<List<FrenchiseGetDTOs>> GetAll(int ReferenceId, int pageNo, int pageSize, string? search);

        Task<string> Create(FrenchisePostDTOs model);
        Task<FrenchiseGetDTOs> GetById(int id);
        Task<string> Update(FrenchisePostDTOs model);
        Task<string> Delete(int Id);
        
         
    }
}
