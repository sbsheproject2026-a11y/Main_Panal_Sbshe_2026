using Application.DTOs;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.MasterTypeDTOs;

namespace Application.Repository
{
    public interface IEmployeeRepository
    {

        Task<List<EmployeeGetDTOs>> GetAll(int ReferenceId, int pageNo, int pageSize, string? search);
        Task<List<EmployeeGetDTOs>> GetAssignFrenchise(int ReferenceId, int pageNo, int pageSize, string? search);

        Task<string> Create(EmployeePostDTOs model);
        Task<EmployeeGetDTOs> GetById(int id);
        Task<string> Update(EmployeePostDTOs model);
        Task<string> Delete(int Id);
        Task<string> DeleteAssignFrenchise(int Id);
        Task<string> AssignFrenchise(EmployeeFrenchiseAssignDTOs model);

        Task<string> PasswordUpdate(UpdatePasswordDTOs model);


    }
}
