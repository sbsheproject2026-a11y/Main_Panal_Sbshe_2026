using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IEnquiriesRepository
    {

        Task<List<EnquiriesGetDTOs>> GetAll(int id);

        Task<string> Create(EnquiriesPostDTOs model);
   
        Task<string> Delete(int Id);

    }
}
