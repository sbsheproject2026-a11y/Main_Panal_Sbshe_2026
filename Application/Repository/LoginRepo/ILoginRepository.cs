using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface ILoginRepository
    {

         

        Task<string> Login(LoginDTOs model);
         

    }
}
