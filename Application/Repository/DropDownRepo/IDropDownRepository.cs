using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Application.Repository
{
    public interface IDropDownRepository
    {

        Task<List<DropDownDTOs>> GetMasterType();
        Task<List<EntityUserResult>> getEntityUser(int Id);
        Task<List<EntityUserResult>> getEmployeeFranchiselist(int Id);
        Task<List<EntityUserResult>> getCourseCategory(int Id);
        Task<List<EntityUserResult>> getCourseStudent(int Id, int courseTypeId);
        Task<List<CommissionAmount>> SetFranchiseCourseAmount(int Id,int FId);
        Task<List<CommissionAmount>> SetCourseCommission(int Id);
        Task<List<EntityUserResult>> getEmployeedropList(int Id);
        Task<List<EntityUserResult>> getFranchise(int Id);
        //Task<List<DashboardStatsDTO>> getDashboard(int? Id);
        Task<List<DropDownDTOs>> GetMasterDetailType(int Id);
        Task<List<DropDownDTOs>> GetMasterDetailTypeParent();
        Task<List<EntityWalletResult>> getwalletbalance(int Id);
        Task<List<DropDownDTOs>> GetParentCourse();
        Task<DropDownDTOs> GetMasterDetail(int Id);
        Task<List<DropDownDTOs>> GetCourseClass();

        Task<List<DropDownDTOs>> GetState();
        Task<List<DropDownDTOs>> GetMasterSession();
        Task<List<DropDownDTOs>> GetDistrict(int Id);
        Task<List<DropDownDTOs>> GetCity(int Id);
        Task<List<DropDownDTOs>> GetOnlineSession();
        Task<List<EntityUserResult>> getCourseOnlineStudent(int Id, int courseTypeId);


    }
}
