using Application.DTOs; 
 

namespace Application.Repository 
{
    public interface IWebsiteContentRepository
    {

        Task<List<FrenchiseGetDTOs>> GetAll(int id);

        
         
    }
}
