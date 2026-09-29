using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class DropDownEmployeeController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public DropDownEmployeeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

 

        [HttpGet("dropdown-emp-entityuser/{id}")]
        public async Task<IActionResult> getEntityUser(int Id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.getEntityUser(Id);

                return Ok(new
                {
                    Success = true,
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

        [Authorize]
        [HttpGet("dropdown-emp-wallet-balance")]
        public async Task<IActionResult> getwalletbalance([FromQuery] int? userId)
        {
            try
            {
                // ✅ Agar query me userId aaya to wahi use karo
                // ✅ Nahi to token se lo
                int finalUserId;

                if (userId.HasValue && userId.Value > 0)
                {
                    finalUserId = userId.Value;
                }
                else
                {
                    var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    var tokenUserId = claims.GetValueOrDefault("UserId");

                    if (string.IsNullOrEmpty(tokenUserId))
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            Message = "UserId not found"
                        });
                    }

                    finalUserId = Convert.ToInt32(tokenUserId);
                }
                var data = await _unitOfWork.DropDown.getwalletbalance(Convert.ToInt32(finalUserId));

                return Ok(new
                {
                    Success = true,
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
       
        
        [HttpGet("dropdown-emp-wallet-balance1")]
        public async Task<IActionResult> getwalletbalance1(int userId)
        {
            try
            {
                
                var data = await _unitOfWork.DropDown.getwalletbalance(userId);

                return Ok(new
                {
                    Success = true,
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
       
        [Authorize]
        [HttpGet("dropdown-emp-getEmployeeFranchiselist")]
        public async Task<IActionResult> getEmployeeFranchiselist()
        {
            try
            {
                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                var userId = claims.GetValueOrDefault("UserId");
                var data = await _unitOfWork.DropDown.getEmployeeFranchiselist(Convert.ToInt32(userId));

                return Ok(new
                {
                    Success = true,
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
       
        
        [HttpGet("dropdown-emp-getCourseCategory/{id}")]
        public async Task<IActionResult> getCourseCategory(int Id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.getCourseCategory(Id);

                return Ok(new
                {
                    Success = true,
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


        [HttpGet("dropdown-emp-getdistrict/{id}")]
        public async Task<IActionResult> GetDistrict(int id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetDistrict(id);

                return Ok(new
                {
                    Success = true,
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

        [HttpGet("dropdown-emp-getcity/{id}")]
        public async Task<IActionResult> GetCity(int id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetCity(id);

                return Ok(new
                {
                    Success = true,
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

        [HttpGet("dropdown-emp-getmastertypes/{id}")]
        public async Task<IActionResult> GetMasterDetailType(int id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetMasterDetailType(id);

                return Ok(new
                {
                    Success = true,
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

        [HttpGet("dropdown-emp-getparentCourse")]
        public async Task<IActionResult> GetParentCourse()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetParentCourse();

                return Ok(new
                {
                    Success = true,
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
        [HttpGet("dropdown-emp-getCourse/{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.getCourseStudent(id);

                return Ok(new
                {
                    Success = true,
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


        [HttpGet("dropdown-emp-getstate")]
        public async Task<IActionResult> GetState()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetState();

                return Ok(new
                {
                    Success = true,
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

        [HttpGet("dropdown-emp-getmastersession")]
        public async Task<IActionResult> GetMasterSession()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetMasterSession();

                return Ok(new
                {
                    Success = true,
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



        [HttpGet("dropdown-emp-getmastertype")]
        public async Task<IActionResult> GetMasterType()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetMasterType();

                return Ok(new
                {
                    Success = true,
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

