using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IStateRepository
    {

        Task<List<LocationGetDTOs>> GetAll(int typeid);

        Task<string> Create(LocationPostDTOs model);
        Task<LocationGetDTOs> GetById(int id);
        Task<string> Update(LocationPostDTOs model);
        Task<string> Delete(int Id);
        

    }
}
