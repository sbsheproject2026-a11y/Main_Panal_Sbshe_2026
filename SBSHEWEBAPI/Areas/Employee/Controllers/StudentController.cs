using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public StudentController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

        [Authorize]
        [HttpGet("student-getall")]

        public async Task<IActionResult> Get(int? referenceId, int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {
                int finalUserId;

                if (referenceId.HasValue && referenceId.Value > 0)
                {
                    finalUserId = referenceId.Value;
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
                var data = await _unitOfWork.Student.GetAll(finalUserId, pageNo, pageSize, search);

                return Ok(new
                {
                    Success = true,
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TotalRecords = data.Count,
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

       

        [Authorize]
        [HttpPost("student-create")]
        public async Task<IActionResult> Create([FromForm] StudentPostDTOs model)
        {
            try
            {
                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                var userId = claims.GetValueOrDefault("UserId");

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

                if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
                    model.SelfImage = await UploadFile(model.SelfImage1);

                if (model.SignatureImage1 != null && model.SignatureImage1.Length > 0)
                    model.SignatureImage = await UploadFile(model.SignatureImage1);

                if (model.AadhaarCardFrant1 != null && model.AadhaarCardFrant1.Length > 0)
                    model.AadhaarCardFrant = await UploadFile(model.AadhaarCardFrant1);

                if (model.AadhaarCardBack1 != null && model.AadhaarCardBack1.Length > 0)
                    model.AadhaarCardBack = await UploadFile(model.AadhaarCardBack1);


                if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
                {
                    foreach (var academic in model.AcademicDetails)
                    {
                        if (academic.File != null &&
                            academic.File.Length > 0)
                        {
                            academic.FileName = await UploadFile(academic.File);
                        }
                    }
                }

                int num = RandomNumberGenerator.GetInt32(10000, 99999);

                model.ReferenceId = model.ReferenceId == 0 ? Convert.ToInt32(userId) : model.ReferenceId;

                model.EmployeeId = Convert.ToInt32(userId);

                model.UserName = model.Email;

                model.Password = model.DOB?.ToString("ddMMyyyy") ?? "";

                model.StudyModeId = model.StudyModeId == 0 ? 89 : model.StudyModeId;
                model.GenderId = model.GenderId == 0 ? null : model.GenderId;
                model.CasteCategory = model.CasteCategory == 0 ? null : model.CasteCategory;
                


                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

                var result = (await _unitOfWork.Student.Create(model)).Split(",");
                var msg = result[0];
                var entityId = result[1];

                if (msg == "Successfully")
                {

                    if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
                    {
                        foreach (var academic in model.AcademicDetails)
                        {
                            await _unitOfWork.Student.CreateAcademicDetail(Convert.ToInt32(entityId), academic);
                        }
                    }
                }


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




        [HttpPost("create-academicdetails")]
        public async Task<IActionResult> Createacademicdetails([FromForm] AcademicDetailDTO model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid request"
                    });
                }

                if (!model.EntityId.HasValue || model.EntityId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Student Id is required"
                    });
                }

                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "Uploads"
                );

                Directory.CreateDirectory(folderPath);

                async Task<string> UploadFile(IFormFile file)
                {
                    string fileName = $"{Guid.NewGuid():N}"[..8] + "_" + Path.GetFileName(file.FileName);

                    string filePath = Path.Combine(folderPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await file.CopyToAsync(stream);

                    return fileName;
                }

                if (model.File != null &&
                    model.File.Length > 0)
                {
                    model.FileName =
                        await UploadFile(model.File);
                }

                await _unitOfWork.Student.CreateAcademicDetail(
                    model.EntityId.Value,
                    model
                );

                return Ok(new
                {
                    success = true,
                    message = "Academic details created successfully"
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





        [HttpGet("student-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _unitOfWork.Student.GetById(id);

                if (data == null)
                {
                    return NotFound("Student not found");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "Error while fetching student data.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }





        [HttpGet("student-academic-details/{id}")]
        public async Task<IActionResult> AcademicDetailsByEntityIddata(int id)
        {
            try
            {

                var academicDetailsData =  await _unitOfWork.Student.GetAcademicDetailsById(id);



                return Ok(academicDetailsData);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "Error while fetching student data.",
                        error = ex.Message
                    }
                );
            }
        }



     

        [HttpPut("student-update")]
        public async Task<IActionResult> Update(StudentPostDTOs model)
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

            async Task<string> UploadFile(IFormFile file, string oldFile)
            {
                if (file == null || file.Length == 0)
                    return oldFile;

                // Old file delete
                if (!string.IsNullOrEmpty(oldFile))
                {
                    string oldPath = Path.Combine(folderPath, oldFile);

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // New file name
                string fileName =
                    $"{Guid.NewGuid():N}"[..8] + "_" +
                    Path.GetFileName(file.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                // Save new file
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;
            }

            // Upload / Update Images

            if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
            {
                model.SelfImage = await UploadFile(
                    model.SelfImage1,
                    model.SelfImage
                );
            }

            if (model.SignatureImage1 != null && model.SignatureImage1.Length > 0)
            {
                model.SignatureImage = await UploadFile(
                    model.SignatureImage1,
                    model.SignatureImage
                );
            }

            if (model.AadhaarCardFrant1 != null && model.AadhaarCardFrant1.Length > 0)
            {
                model.AadhaarCardFrant = await UploadFile(
                    model.AadhaarCardFrant1,
                    model.AadhaarCardFrant
                );
            }

            if (model.AadhaarCardBack1 != null && model.AadhaarCardBack1.Length > 0)
            {
                model.AadhaarCardBack = await UploadFile(
                    model.AadhaarCardBack1,
                    model.AadhaarCardBack
                );
            }





            var result = await _unitOfWork.Student.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("student-delete/{id}")]
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

            var result = await _unitOfWork.Student.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }



    }
}

