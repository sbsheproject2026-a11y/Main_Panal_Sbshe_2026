using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IEnquiriesRepository
    {

        Task<List<EnquiriesGetDTOs>> GetAll(int id);

        Task<string> Create(EnquiriesPostDTOs model);
   
        Task<string> Delete(int Id);
        Task<List<RTIApplicationsGetDTOs>> GetRTIAll();
        Task<string> RTICreate(RTIApplicationsDTOs model);
        Task<bool> UpdateRTIStatus(UpdateRTIStatusDTO model);
        Task<RTIApplicationsGetDTOs> GetRTIByTraickId(string id);
        Task<string> DeleteRTI(int Id);

    }
}
