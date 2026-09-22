using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IWebsiteContentRepository
    {

        Task<List<FrenchiseGetDTOs>> GetAll(int id);

        Task<List<WebsiteContentGetDTOs>> GetAllData();

        Task<string> Create(WebsiteContentPostDTOs model);
        Task<WebsiteContentGetDTOs?> GetbyId(int Id);
        Task<string> Update(WebsiteContentPostDTOs model);




    }
}
