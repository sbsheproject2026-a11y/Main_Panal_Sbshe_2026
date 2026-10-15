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
    public class DropDownFranchiseController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public DropDownFranchiseController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

  

        
        [HttpGet("dropdown-franchise-getmastertypes/{id}")]
        public async Task<IActionResult> GetMasterDetailType(int id)
        {
            try
            {
             
                var data = await _unitOfWork.DropDown.GetStudyCentreDepartment(id);

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

     
        [HttpGet("dropdown-franchise-getCourse/{id}")]
        public async Task<IActionResult> GetCourse(int id,int courseTypeId)
        {
            try
            {
                 
                var data = await _unitOfWork.DropDown.getCourseStudyCentreStudent(id, courseTypeId);

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

