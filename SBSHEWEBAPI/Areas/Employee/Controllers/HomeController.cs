using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;

        public HomeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _appEnvironment = appEnvironment;
        }


        [HttpGet("emp-frenchise-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Frenchise.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }


        [HttpPut("emp-frenchise-update")]
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

            if (model.PassportPhoto1 != null && model.PassportPhoto1.Length > 0)
                model.PassportPhoto = await UploadFile(model.PassportPhoto1, model.PassportPhoto);

            if (model.AuthorizedSignature1 != null && model.AuthorizedSignature1.Length > 0)
                model.AuthorizedSignature = await UploadFile(model.AuthorizedSignature1, model.AuthorizedSignature);

            if (model.AadhaarCard1 != null && model.AadhaarCard1.Length > 0)
                model.AadhaarCardimage = await UploadFile(model.AadhaarCard1, model.AadhaarCardimage);

            if (model.PanCard1 != null && model.PanCard1.Length > 0)
                model.PanCardimage = await UploadFile(model.PanCard1, model.PanCardimage);

            if (model.InstituteCertificate1 != null && model.InstituteCertificate1.Length > 0)
                model.InstituteCertificate = await UploadFile(model.InstituteCertificate1, model.InstituteCertificate);

            if (model.BuildingPhoto11 != null && model.BuildingPhoto11.Length > 0)
                model.BuildingPhoto1 = await UploadFile(model.BuildingPhoto11, model.BuildingPhoto1);

            if (model.BuildingPhoto22 != null && model.BuildingPhoto22.Length > 0)
                model.BuildingPhoto2 = await UploadFile(model.BuildingPhoto22, model.BuildingPhoto2);


            var result = await _unitOfWork.Frenchise.UpdateDocument(model);


            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        
        [HttpPut("emp-update-password")]
        public async Task<IActionResult> UpdatePassword(FranchiseUpdatePasswordDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
            model.UserName = model.UserName;
            model.Password = model.Password;

            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

            var result = await _unitOfWork.Frenchise.UpdatePassword(model);


            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        
        [HttpPost("franchise-applyforbook")]
        public async Task<IActionResult> ApplyForBooks([FromForm] ApplyStudentPostDTOs model)
        {
            try
            {
                //int finalUserId;


                //var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                //var tokenUserId = claims.GetValueOrDefault("UserId");

                //if (string.IsNullOrEmpty(tokenUserId))
                //{
                //    return BadRequest(new
                //    {
                //        Success = false,
                //        Message = "UserId not found"
                //    });
                //}

                //model.UserId = Convert.ToInt32(tokenUserId);

                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request data is required."
                    });
                }


                if (!model.UserId.HasValue || model.UserId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Franchise is required."
                    });
                }
                if (!model.Amount.HasValue || model.Amount.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Amount is required."
                    });
                }
               // model.CourseId = 68;

                var resultaddress = (await _unitOfWork.Student.CreateBookDeliveryAddress(model));
                var result = (await _unitOfWork.GetWayWorking.GenrateOrderBook((int)model.UserId, Convert.ToDecimal(model.Amount), (int)model.CourseId, (int)model.Quantity)).Split(",");
                var msg = result[0];
                var entityId = result[1];
                var oId = result[2];
                HttpContext.Session.SetString("User", entityId);
                HttpContext.Session.SetString("Key", oId);
                // ✅ IDs frontend ko bhi bhejo
                return Ok(new
                {
                    success = true,
                    message = msg,
                    userid = entityId,      // ← Ye frontend bhejega payment pe
                    paymentid = oId         // ← Ye bhi
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong.",
                    error = ex.Message
                });
            }
        }


    }
}

