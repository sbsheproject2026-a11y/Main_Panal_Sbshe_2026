using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Cryptography;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpPost("acc-create")]
        public async Task<IActionResult> Create(AccRegistrationPostDTOs model)
        {
            try
            {
                int num = RandomNumberGenerator.GetInt32(100000, 999999);

                // Registration default values
                model.EntityTypeId = 91;
                model.RoleId = 90;
                model.ContactTypeId = 18;
                model.WhatsAppContactTypeId = 19;
                model.AddressTypeId = 16;
                model.FatherEntityTypeId = 14;
                model.ReferenceId = 1;

                // Login credentials
                model.UserName = model.Email;
                model.Password = $"SBSHE@{num}";

                // Store only hashed password in database
                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Create registration
                var result = await _unitOfWork.AccRegistration.Create(model);

                // Default message
                string printmsg = string.Empty;

                // Only generate success message when registration is successful
                if (result == "Successfully")
                {
                    printmsg =
                        $"Dear {model.Name},\n" +
                        $"Congratulations! Your Admission Consultant registration has been submitted successfully.\n" +
                        $"Login Details:\n" +
                        $"Username: {model.UserName}\n" +
                        $"Password: {model.Password}\n" +
                        $"Please keep your login credentials safe. " +
                        $"Our team will review your registration details and contact you shortly regarding the next steps.\n" +
                        $"Thank you for registering with us.";
                }

                return Ok(new
                {
                    success = result == "Successfully",
                    data = model,
                    message = result,
                    printmsg = printmsg
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



     
        [HttpPost("new-student-create")]
        public async Task<IActionResult> Create([FromForm] StudentPostDTOs model)
        {
            try
            {
                
                

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


                model.ReferenceId = Convert.ToInt32(model.ReferenceId);
                model.UserName = $"{num}@SBSHE";
                model.Password = $"SBSHE@{num}";



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


    }
}

