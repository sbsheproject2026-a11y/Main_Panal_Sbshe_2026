using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DropDownController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public DropDownController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }


        [HttpGet("dropdown-entityuser/{id}")]
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
        
        [HttpGet("dropdown-franchise/{id}")]
        public async Task<IActionResult> getFranchise(int Id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.getFranchise(Id);

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
        //[HttpGet("dashboard")]
        //public async Task<IActionResult> getDashboard()
        //{
        //    try
        //    {
                 

        //        var data = await _unitOfWork.DropDown.getDashboard(0);

        //        return Ok(new
        //        {
        //            Success = true,
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
        
        
        [HttpGet("dropdown-employee-list/{id}")]
        public async Task<IActionResult> getEmployeedropList(int Id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.getEmployeedropList(Id);

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


        [HttpGet("dropdown-getdistrict/{id}")]
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

        [HttpGet("dropdown-getcity/{id}")]
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

        [HttpGet("dropdown-getmastertypes/{id}")]
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
       
        [HttpGet("dropdown-getmastertypesdetail-parent")]
        public async Task<IActionResult> GetMasterDetailTypeParent()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetMasterDetailTypeParent();

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

        [HttpGet("dropdown-getparentCourse")]
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
        [HttpGet("dropdown-getcourseclass")]
        public async Task<IActionResult> GetCourseClass()
        {
            try
            {
                var data = await _unitOfWork.DropDown.GetCourseClass();

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


        [HttpGet("dropdown-getstate")]
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

        [HttpGet("dropdown-getmastertype")]
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

 
    

        [HttpGet("dropdown-setfranchisecourse-amount/{id}")]
        public async Task<IActionResult> SetFranchiseCourseAmount(int id, [FromQuery] int? Fid)
        {
            try
            {
                var data = await _unitOfWork.DropDown.SetFranchiseCourseAmount(id, Fid ?? 0);

                return Ok(new { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
        [HttpGet("dropdown-set-commission-amount/{id}")]
        public async Task<IActionResult> SetFranchiseCourseAmount(int id)
        {
            try
            {
                var data = await _unitOfWork.DropDown.SetCourseCommission(id);

                return Ok(new { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}

