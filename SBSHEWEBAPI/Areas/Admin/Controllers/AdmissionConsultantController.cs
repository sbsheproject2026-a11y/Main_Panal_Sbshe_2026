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
        private readonly IWebHostEnvironment _environment;

        public AdmissionConsultantController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }
        [HttpGet("acc-getall")]

        public async Task<IActionResult> Get(int? referenceId = 1, int? pageNo=null, int? pageSize=null, string? search = null)
        {
            try
            {
                pageNo ??= 1;
                pageSize ??= 100;

                var (data, totalCount) = await _unitOfWork.AccRegistration.GetAll(referenceId, pageNo, pageSize, search);

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

        [HttpPost("frenchise-create")]
        public async Task<IActionResult> Create(AccRegistrationPostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
                    });
                }

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

            
                var result = (await _unitOfWork.AccRegistration.Create(model)).Split(",");
                var msg = result[0];
                var entityId = result[1];
                return Ok(new
                {
                    success = true,
                    message = msg
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


        [HttpGet("acc-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.AccRegistration.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("acc-update")]

        public async Task<IActionResult> Update( [FromForm] AccRegistrationPostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");
            Directory.CreateDirectory(folderPath);

            async Task<string> UploadFile(IFormFile file)
            {
                string fileName = $"{Guid.NewGuid():N}"[..8] + "_" +
                                  Path.GetFileName(file.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;
            }

            if (model.PassportPhoto1 != null && model.PassportPhoto1.Length > 0)
                model.PassportPhoto = await UploadFile(model.PassportPhoto1);

            if (model.AuthorizedSignature1 != null && model.AuthorizedSignature1.Length > 0)
                model.AuthorizedSignature = await UploadFile(model.AuthorizedSignature1);

            if (model.AadhaarCard1 != null && model.AadhaarCard1.Length > 0)
                model.AadhaarCardimage = await UploadFile(model.AadhaarCard1);

            if (model.PanCard1 != null && model.PanCard1.Length > 0)
                model.PanCardimage = await UploadFile(model.PanCard1);

            if (model.InstituteCertificate1 != null && model.InstituteCertificate1.Length > 0)
                model.InstituteCertificate = await UploadFile(model.InstituteCertificate1);

            if (model.BuildingPhoto11 != null && model.BuildingPhoto11.Length > 0)
                model.BuildingPhoto1 = await UploadFile(model.BuildingPhoto11);

            if (model.BuildingPhoto22 != null && model.BuildingPhoto22.Length > 0)
                model.BuildingPhoto2 = await UploadFile(model.BuildingPhoto22);

            var result = await _unitOfWork.AccRegistration.Update(model);

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

            var result = await _unitOfWork.AccRegistration.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


    }
}

