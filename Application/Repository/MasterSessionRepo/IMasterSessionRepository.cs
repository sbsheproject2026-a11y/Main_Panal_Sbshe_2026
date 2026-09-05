using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IMasterSessionRepository
    {

        Task<List<MasterSessionGetDTOs>> GetAll();

        Task<string> Create(MasterSessionPostDTOs model);
        Task<MasterSessionGetDTOs> GetById(int id);
        Task<string> Update(MasterSessionPostDTOs model);
        Task<string> Delete(int Id);
 

    }
}
