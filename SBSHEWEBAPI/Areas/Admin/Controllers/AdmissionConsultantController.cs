using Application.DTOs;

using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdmissionConsultantController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AdmissionConsultantController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpGet("acc-getall")]

        public async Task<IActionResult> Get(int referenceId = 1, int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {
                var data = await _unitOfWork.AccRegistration.GetAll(referenceId, pageNo, pageSize, search);

                return Ok(new
                {
                    Success = true,
                    PageNo = pageNo,
                    PageSize = pageSize,
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

        [HttpPost("frenchise-create")]
        public async Task<IActionResult> Create(FrenchisePostDTOs model)
        {
            try
            {
                int num = RandomNumberGenerator.GetInt32(100000, 999999);

                model.EntityTypeId = 9;
                model.RoleId = 6;
                model.ContactTypeId = 18;
                model.WhatsAppContactTypeId = 19;
                model.AddressTypeId = 16;
                model.FatherEntityTypeId = 14;
                model.UserName = model.Email;
                model.ReferenceId = 1;
                model.Password = $"SBSHE@{num}";

                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

                var result = await _unitOfWork.Frenchise.Create(model);

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }


        [HttpGet("frenchise-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Frenchise.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("frenchise-update")]
        public async Task<IActionResult> Update(FrenchisePostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Frenchise.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("frenchise-delete/{id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Frenchise.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


    }
}

