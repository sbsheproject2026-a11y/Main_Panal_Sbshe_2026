using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeFranchiseAssignController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public EmployeeFranchiseAssignController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

        [Authorize]
        [HttpGet("emp-franchise-list")]

        //public async Task<IActionResult> Get(int? pageNo = null, int? pageSize = null, string? search = null)
        //{
        //    try
        //    {
        //        pageNo ??= 1;
        //        pageSize ??= 100;
        //        var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
        //        var userId = claims.GetValueOrDefault("UserId");
        //        var data = await _unitOfWork.Frenchise.GetAll(Convert.ToInt32(userId), pageNo, pageSize, search);

        //        return Ok(new
        //        {
        //            Success = true,
        //            PageNo = pageNo,
        //            PageSize = pageSize,
        //            Data = data
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            Success = false,
        //            Message = ex.Message
        //        });
        //    }
        //}

        public async Task<IActionResult> Get(int? pageNo = null, int? pageSize = null, string? search = null)
        {
            try
            {
                pageNo ??= 1;
                pageSize ??= 100;
                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                var userId = claims.GetValueOrDefault("UserId");
                var (data, totalCount) = await _unitOfWork.Employee.GetAssignFrenchise(Convert.ToInt32(userId), pageNo, pageSize, search);

                return Ok(new
                {
                    Success = true,
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TotalRecords = totalCount,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }




    }
}

