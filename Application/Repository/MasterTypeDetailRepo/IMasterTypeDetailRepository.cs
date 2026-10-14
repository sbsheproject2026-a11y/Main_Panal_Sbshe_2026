using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IMasterTypeDetailRepository
    {

        Task<List<MasterTypeDetailGetDTOs>> GetAll();

        Task<string> Create(MasterTypeDetailPostDTOs model);
        Task<MasterTypeDetailGetDTOs> GetById(int id);
        Task<string> Update(MasterTypeDetailPostDTOs model);
        Task<string> Delete(int Id);
 




    }
}
