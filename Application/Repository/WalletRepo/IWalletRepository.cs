using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IWalletRepository
    {

        Task<List<LocationGetDTOs>> GetAll(int typeid);

        Task<string> Create(LocationPostDTOs model);
       
        Task<string> Delete(int Id);
        

    }
}
