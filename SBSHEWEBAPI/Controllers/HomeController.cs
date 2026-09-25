using Application.DTOs;
using Application.Helper;
using ClosedXML.Excel;
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
                    relacition = data.Relacition ?? "",
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





        [HttpPost("bulk-upload")]
        public async Task<IActionResult> BulkUpload(IFormFile? file, [FromForm] AccRegistrationPostDTOs model)
        {
            try
            {
                // =====================================================
                // VALIDATION
                // =====================================================
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Please upload an Excel file."
                    });
                }

                // Validate extension
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Only .xlsx or .xls files are allowed"
                    });
                }

                var successList = new List<object>();
                var failedList = new List<object>();
                int rowNumber = 1;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);

                        // =============================================
                        // Data starts from ROW 2 (Row 1 = Header)
                        // =============================================
                        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

                        if (lastRow < 2)
                        {
                            return BadRequest(new
                            {
                                Success = false,
                                Message = "Excel file is empty."
                            });
                        }

                        // Loop from row 2 to last row
                        for (int r = 2; r <= lastRow; r++)
                        {
                            rowNumber = r;

                            try
                            {
                                var row = worksheet.Row(r);

                                // Skip completely empty rows
                                if (row.IsEmpty())
                                {
                                    continue;
                                }

                                // =============================================
                                // Map Excel columns to DTO
                                // Column 1: EnrollmentNo
                                // Column 2: CENTER NAME -> FatherName
                                // Column 3: NAME -> Name
                                // Column 4: MobileNo
                                // Column 5: ADRESS -> Address
                                // Column 6: Email
                                // =============================================
                                var rowModel = new AccRegistrationPostDTOs
                                {
                                    EnrollmentNo = row.Cell(1).GetString()?.Trim(),
                                    Name = row.Cell(2).GetString()?.Trim(), // Center Name
                                    FatherName = row.Cell(3).GetString()?.Trim(),
                                    MobileNo = row.Cell(4).GetString()?.Trim(),
                                    Address = row.Cell(5).GetString()?.Trim(),
                                    Email = row.Cell(6).GetString()?.Trim(),
                                };

                                // =============================================
                                // Validate required fields
                                // =============================================
                                var missingFields = new List<string>();

                                if (string.IsNullOrWhiteSpace(rowModel.EnrollmentNo))
                                    missingFields.Add("Enrollment No");

                                if (string.IsNullOrWhiteSpace(rowModel.Name))
                                    missingFields.Add("Center Name");

                                if (string.IsNullOrWhiteSpace(rowModel.FatherName))
                                    missingFields.Add("Name");

                                if (string.IsNullOrWhiteSpace(rowModel.MobileNo))
                                    missingFields.Add("Number");

                                if (string.IsNullOrWhiteSpace(rowModel.Address))
                                    missingFields.Add("Address");

                                if (string.IsNullOrWhiteSpace(rowModel.Email))
                                    missingFields.Add("Email");

                                if (missingFields.Count > 0)
                                {
                                    failedList.Add(new
                                    {
                                        Row = rowNumber,
                                        EnrollmentNo = rowModel.EnrollmentNo ?? "",
                                        Name = rowModel.Name ?? "",
                                        FatherName = rowModel.FatherName ?? "",
                                        Address = rowModel.Address ?? "",
                                        MobileNo = rowModel.MobileNo ?? "",
                                        Email = rowModel.Email ?? "",
                                        Reason = $"Missing required fields: {string.Join(", ", missingFields)}"
                                    });
                                    continue;
                                }

                                // =============================================
                                // Default values (same as acc-create)
                                // =============================================
                                rowModel.EntityTypeId = 9;
                                rowModel.RoleId = 6;
                                rowModel.ContactTypeId = 18;
                                rowModel.WhatsAppContactTypeId = 19;
                                rowModel.AddressTypeId = 16;
                                rowModel.FatherEntityTypeId = 14;
                                rowModel.ReferenceId = 1;

                                // =============================================
                                // Login credentials
                                // Username = Email
                                // Password = SBSHE@ + MobileNo
                                // =============================================
                                rowModel.UserName = rowModel.Email;
                                rowModel.Password = $"SBSHE@" + rowModel.MobileNo;
                                rowModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rowModel.Password);

                                // =============================================
                                // Create registration
                                // =============================================
                                var result = (await _unitOfWork.AccRegistration.Create(rowModel)).Split(",");
                                var msg = result[0];
                                var entityId = result.Length > 1 ? result[1] : "";

                                if (msg == "Successfully")
                                {
                                     

                                    successList.Add(new
                                    {
                                        Row = rowNumber,
                                        EnrollmentNo = rowModel.EnrollmentNo,
                                        CenterName = rowModel.FatherName,
                                        Name = rowModel.Name,
                                        MobileNo = rowModel.MobileNo,
                                        Address = rowModel.Address,
                                        Email = rowModel.Email,
                                        UserName = rowModel.UserName,
                                        Password = rowModel.Password,
                                        EntityId = entityId 
                                    });
                                }
                                else
                                {
                                    failedList.Add(new
                                    {
                                        Row = rowNumber,
                                        EnrollmentNo = rowModel.EnrollmentNo,
                                        Name = rowModel.Name,
                                        MobileNo = rowModel.MobileNo,
                                        Email = rowModel.Email,
                                        Reason = msg
                                    });
                                }
                            }
                            catch (Exception rowEx)
                            {
                                failedList.Add(new
                                {
                                    Row = rowNumber,
                                    Reason = rowEx.Message
                                });
                            }
                        }
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = $"Bulk upload completed. Success: {successList.Count}, Failed: {failedList.Count}",
                    totalRecords = successList.Count + failedList.Count,
                    successCount = successList.Count,
                    failedCount = failedList.Count,
                    successData = successList,
                    failedData = failedList
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


