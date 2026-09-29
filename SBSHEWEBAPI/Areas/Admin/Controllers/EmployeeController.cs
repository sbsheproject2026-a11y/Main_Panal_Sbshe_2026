using Application.DTOs;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }


        [HttpGet("employee-getall")]

        public async Task<IActionResult> Get(int referenceId = 1, int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {
                var data = await _unitOfWork.Employee.GetAll(referenceId, pageNo, pageSize, search);

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


        [HttpPost("employee-create")]
        public async Task<IActionResult> Create([FromForm] EmployeePostDTOs model)
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

                string randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
                // Image Upload
                if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
                {
                    string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = $"{randomString}_{Path.GetFileName(model.SelfImage1.FileName)}";

                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.SelfImage1.CopyToAsync(stream);
                    }

                    // Database me image ka path save karo
                    model.SelfImage = fileName  ;
                }

                int num = RandomNumberGenerator.GetInt32(10000, 99999);

                model.EntityTypeId = 34;
                model.RoleId = 33;
                model.ContactTypeId = 18;
                model.WhatsAppContactTypeId = 20;
                model.AddressTypeId = 16;
                model.FatherEntityTypeId = 14;
                model.ReferenceId = 1;
                model.UserName = $"{num}@SBSHE";
                model.Password = $"SBSHE@{num}";

                model.DesignationId = model.DesignationId == 0 ? null : model.DesignationId;
                model.GenderId = model.GenderId == 0 ? null : model.GenderId;
                model.DepartmentId = model.DepartmentId == 0 ? null : model.DepartmentId;
                model.EmployeeTypeId = model.EmployeeTypeId == 0 ? null : model.EmployeeTypeId;

                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

                var result = await _unitOfWork.Employee.Create(model);

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


        [HttpGet("employee-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Employee.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("employee-update")]
        public async Task<IActionResult> Update(EmployeePostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
            string randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
            // Image Upload
            if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
            {
                string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Old Image Delete
                if (!string.IsNullOrEmpty(model.SelfImage))
                {
                    string oldFilePath = Path.Combine(folderPath, model.SelfImage);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // New Image Save
               
                string fileName = $"{randomString}_{Path.GetFileName(model.SelfImage1.FileName)}";
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.SelfImage1.CopyToAsync(stream);
                }

                // Save New Image Name
                model.SelfImage = fileName;
            }

            model.DesignationId = model.DesignationId == 0 ? null : model.DesignationId;
            model.GenderId = model.GenderId == 0 ? null : model.GenderId;
            model.DepartmentId = model.DepartmentId == 0 ? null : model.DepartmentId;
            model.EmployeeTypeId = model.EmployeeTypeId == 0 ? null : model.EmployeeTypeId;

            var result = await _unitOfWork.Employee.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("employee-delete/{id}")]
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

            var result = await _unitOfWork.Employee.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


        [HttpPost("employee-assignfrenchise")]
        public async Task<IActionResult> AssignFrenchise(EmployeeFrenchiseAssignDTOs model)
        {
            var result = await _unitOfWork.Employee.AssignFrenchise(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("employee-getassignfrenchise")]
        public async Task<IActionResult> GetAssignFrenchise(int? referenceId = null, int? pageNo = null, int? pageSize = null, string? search = null)
        {
            try
            {
                pageNo ??= 1;
                pageSize ??= 100;

                var (data, totalCount) = await _unitOfWork.Employee.GetAssignFrenchise(referenceId, pageNo, pageSize, search);

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

         
        [HttpDelete("employee-deleteassignfrenchise/{id}")]
        public async Task<IActionResult> DeleteAssignFrenchise(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Employee.DeleteAssignFrenchise(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }



        [HttpPut("changepassword-update")]
        public async Task<IActionResult> PasswordUpdate(UpdatePasswordDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
           // int num = RandomNumberGenerator.GetInt32(100000, 999999);
            // model.Password = $"SBSHE@{num}";
            model.Password = model.Password;
            model.HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

            var result = await _unitOfWork.Employee.PasswordUpdate(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
    }
}

