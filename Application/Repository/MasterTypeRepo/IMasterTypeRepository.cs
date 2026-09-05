using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IMasterTypeRepository
    {

        Task<List<MasterTypeGetDTOs>> GetAll();

        Task<string> Create(MasterTypePostDTOs model);
        Task<MasterTypeGetDTOs> GetById(int id);
        Task<string> Update(MasterTypePostDTOs model);
        Task<string> Delete(int Id);

    }
}
