using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
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
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;

        public HomeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _appEnvironment = appEnvironment;
        }

        [HttpPost("acc-create")]
        public async Task<IActionResult> Create([FromForm] AccRegistrationPostDTOs model)
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
                if (model.Name == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
                    });
                }
                if (model.InstituteName == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
                    });
                }

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
                model.Password = $"SBSHE@" + model.MobileNo;

                // Store only hashed password in database
                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Create registration
                // var result = await _unitOfWork.AccRegistration.Create(model);
                var result = (await _unitOfWork.AccRegistration.Create(model)).Split(",");
                var msg = result[0];
                var entityId = result[1];
                // Default message
                string printmsg1 = string.Empty;

                // Only generate success message when registration is successful
                if (msg == "Successfully")
                {
                    printmsg1 =
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
                    success = msg == "Successfully",
                    data = model,
                    message = msg,
                    printmsg = printmsg1
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

                if (model == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
                    });
                }
                if (model.Name == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
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


                model.ReferenceId = 3;
                model.UserName = $"{num}@SBSHE";
                model.Password = $"SBSHE@{num}";
                model.StudyModeId = 88;


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
                // Default message
                string printmsg1 = string.Empty;

                if (msg == "Successfully")
                {
                    printmsg1 =
                      $"Dear {model.Name},\n" +
                      $"Congratulations! Your student registration has been submitted successfully.\n" +
                      $"Our team will review your registration details and contact you shortly regarding the next steps.\n" +
                      $"Thank you for registering with us. We look forward to supporting you in your academic journey.";

                }

                return Ok(new
                {
                    success = true,
                    message = msg,
                    data = model,
                    printmsg = printmsg1
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


        [HttpPost("studycenter-create")]
        public async Task<IActionResult> Create([FromForm] FrenchisePostDTOs model)
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
                if (model.Name == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
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


                int num = RandomNumberGenerator.GetInt32(100000, 999999);

                model.EntityTypeId = 9;
                model.RoleId = 6;
                model.ContactTypeId = 18;
                model.WhatsAppContactTypeId = 19;
                model.AddressTypeId = 16;
                model.FatherEntityTypeId = 14;
                model.UserName = model.Email;
                model.ReferenceId = 1;
                model.Password = $"SBSHE@" + model.MobileNo;

                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

                // var result = await _unitOfWork.Frenchise.Create(model);
                var result = (await _unitOfWork.Frenchise.Create(model)).Split(",");
                var msg = result[0];
                var entityId = result[1];

                if (msg == "Successfully")
                {

                    if (model.DepartmentIds != null && model.DepartmentIds.Count > 0)
                    {
                        foreach (var academic in model.DepartmentIds)
                        {
                            await _unitOfWork.Frenchise.CreateInstituteDepartmentDetails(Convert.ToInt32(entityId), academic);
                        }
                    }
                }
                // Default message
                string printmsg1 = string.Empty;

                // Only generate success message when registration is successful
                if (msg == "Successfully")
                {
                    printmsg1 =
                    $"Dear {model.FatherName},\n" +
                    $"Congratulations! Your Study Centre registration has been submitted successfully.\n" +
                    $"Registration Details:\n" +
                    $"Centre Name: {model.Name}\n" +
                    $"Login Details:\n" +
                    $"Username: {model.UserName}\n" +
                    $"Password: {model.Password}\n" +
                    $"Your registration details and submitted documents will now be reviewed by our team. " +
                    $"Once the verification process is completed, our team will contact you regarding the approval and further onboarding process.\n" +
                    $"Please keep your login credentials safe and do not share them with anyone.\n" +
                    $"Thank you for choosing to register as an authorized Study Centre with us.";
                }
                return Ok(new
                {
                    success = msg == "Successfully",
                    data = model,
                    message = msg,
                    printmsg = printmsg1
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



        [HttpGet("authority-letter-print/{id}")]
        public async Task<IActionResult> AuthorityLetterPrint(int id)
        {
            try
            {
                var data = await _unitOfWork.DocumentProfile.GetAuthorityLetterPrintData(id);

                if (data == null)
                {
                    return NotFound(new
                    {
                        message = $"Authority Letter data not found for ID: {id}"
                    });
                }

                string selfImageBase64 = "";

                try
                {
                    if (!string.IsNullOrWhiteSpace(data.SelfImage))
                    {
                        string fileName =
                            Path.GetFileName(data.SelfImage);

                        string imagePath = Path.Combine(
                            _appEnvironment.WebRootPath,
                            "Uploads",
                            fileName
                        );

                        Console.WriteLine(
                            "Student Image Path: " +
                            imagePath
                        );

                        if (System.IO.File.Exists(imagePath))
                        {
                            byte[] imageBytes =
                                await System.IO.File.ReadAllBytesAsync(
                                    imagePath
                                );

                            string extension =
                                Path.GetExtension(imagePath)
                                    .ToLowerInvariant();

                            string mimeType = extension switch
                            {
                                ".jpg" => "image/jpeg",
                                ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",
                                _ => "application/octet-stream"
                            };

                            selfImageBase64 =
                                $"data:{mimeType};base64," +
                                Convert.ToBase64String(
                                    imageBytes
                                );

                            Console.WriteLine(
                                "Student Image Base64 generated."
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                "Student Image File NOT FOUND: " +
                                imagePath
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "data.SelfImage is empty."
                        );
                    }
                }
                catch (Exception imageEx)
                {
                    Console.WriteLine(
                        "Student Image Error: " +
                        imageEx.Message
                    );

                    selfImageBase64 = "";
                }

                // =========================================================
                // 3. Stemp IMAGE -> BASE64
                // =========================================================

                string stempImageBase64 = "";

                try
                {
                    string stampFileName = "stemp_1.png";

                    string stampImagePath = Path.Combine(
                        _appEnvironment.WebRootPath,
                        "Uploads",
                        stampFileName
                    );

                    Console.WriteLine(
                        "Stamp Image Path: " +
                        stampImagePath
                    );

                    if (System.IO.File.Exists(stampImagePath))
                    {
                        byte[] stampImageBytes =
                            await System.IO.File.ReadAllBytesAsync(
                                stampImagePath
                            );

                        string extension =
                            Path.GetExtension(stampImagePath)
                                .ToLowerInvariant();

                        string mimeType = extension switch
                        {
                            ".jpg" => "image/jpeg",
                            ".jpeg" => "image/jpeg",
                            ".png" => "image/png",
                            ".gif" => "image/gif",
                            ".webp" => "image/webp",
                            _ => "application/octet-stream"
                        };

                        stempImageBase64 =
                            $"data:{mimeType};base64," +
                            Convert.ToBase64String(
                                stampImageBytes
                            );

                        Console.WriteLine(
                            "Stamp Image Base64 generated."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            "Stamp Image File NOT FOUND: " +
                            stampImagePath
                        );
                    }
                }
                catch (Exception stampEx)
                {
                    Console.WriteLine(
                        "Stamp Image Error: " +
                        stampEx.Message
                    );

                    stempImageBase64 = "";
                }
                // =========================================================
                // 3. Sigh. IMAGE -> BASE64
                // =========================================================

                string sighImageBase64 = "";

                try
                {
                    string sighFileName = "singh.png";

                    string sighImagePath = Path.Combine(
                        _appEnvironment.WebRootPath,
                        "Uploads",
                        sighFileName
                    );

                    Console.WriteLine(
                        "Stamp Image Path: " +
                        sighImagePath
                    );

                    if (System.IO.File.Exists(sighImagePath))
                    {
                        byte[] sighImageBytes =
                            await System.IO.File.ReadAllBytesAsync(
                                sighImagePath
                            );

                        string extension =
                            Path.GetExtension(sighImagePath)
                                .ToLowerInvariant();

                        string mimeType = extension switch
                        {
                            ".jpg" => "image/jpeg",
                            ".jpeg" => "image/jpeg",
                            ".png" => "image/png",
                            ".gif" => "image/gif",
                            ".webp" => "image/webp",
                            _ => "application/octet-stream"
                        };

                        sighImageBase64 =
                            $"data:{mimeType};base64," +
                            Convert.ToBase64String(
                                sighImageBytes
                            );

                        Console.WriteLine(
                            "Stamp Image Base64 generated."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            "Stamp Image File NOT FOUND: " +
                            sighImagePath
                        );
                    }
                }
                catch (Exception stampEx)
                {
                    Console.WriteLine(
                        "Stamp Image Error: " +
                        stampEx.Message
                    );

                    sighImageBase64 = "";
                }




                string qrImage = "";

                try
                {
                    string qrText = "Name-" + (data.Name ?? "") + ",FatherName-" + (data.FatherName ?? "") + ",EnrollmentNo-" + (data.EnrollmentNo ?? "") + ",Course-" + (data.CourseName ?? "");

                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    using (QRCodeData qrCodeData =
                           qrGenerator.CreateQrCode(
                               qrText,
                               QRCodeGenerator.ECCLevel.Q))
                    {
                        var qrCode = new PngByteQRCode(qrCodeData);

                        byte[] qrBytes =
                            qrCode.GetGraphic(
                                20,
                                System.Drawing.Color.Black,
                                System.Drawing.Color.Transparent,
                                false
                            );

                        qrImage =
                            "data:image/png;base64," +
                            Convert.ToBase64String(
                                qrBytes
                            );

                        Console.WriteLine(
                            "QR Base64 generated successfully."
                        );
                    }
                }
                catch (Exception qrEx)
                {
                    Console.WriteLine(
                        "QR Generation Error: " +
                        qrEx.Message
                    );

                    qrImage = "";
                }

                return Ok(new
                {
                    name = data.Name ?? "",
                    centreName = data.CentreName ?? "",
                    firstName = data.FirstName ?? "",
                    fatherName = data.FatherName ?? "",
                    enrollmentNo = data.Code ?? "",
                    srnoDiploma = data.SrnoDiploma ?? "",
                    endDate = data.EndDate ?? "",
                    startDate = data.StartDate ?? "",
                    address = data.Address ?? "",
                    selfImage = selfImageBase64,
                    stempImage = stempImageBase64,
                    singhImage = sighImageBase64,

                    qrimage = qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Authority Letter data fetch failed.",

                        error =
                            ex.Message,

                        innerError =
                            ex.InnerException?.Message
                    }
                );
            }
        }


    }
}

