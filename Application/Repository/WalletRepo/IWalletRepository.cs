using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository 
{
    public interface IWalletRepository
    {

        Task<List<WalletGetDTOs>> GetAll(int UserId, int Amountid);
        Task<List<WalletGetDTOs>> GetAllPending(int UserId, int Amountid);

        Task<string> Create(WalletPostDTOs model);
       
        Task<string> WalletPaymentConfirm(int Id, int Status);
        

    }
}
