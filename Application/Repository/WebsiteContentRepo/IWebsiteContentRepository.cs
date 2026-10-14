using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;


namespace Application.Repository 
{
    public interface IWebsiteContentRepository
    {

        Task<List<FrenchiseGetDTOs>> GetAll(int id);

        Task<List<WebsiteContentGetDTOs>> GetAllData();

        Task<string> Create(WebsiteContentPostDTOs model);
        Task<WebsiteContentGetDTOs?> GetbyId(int Id);
        Task<string> Update(WebsiteContentPostDTOs model);
        Task<string> Delete(int Id);
        Task<List<MasterTypeDetailGetDTOs>> GetWebsiteMaster(int id);
        Task<List<WebsiteCourseGetDTOs>> GetCourseData(int id);




    }
}
