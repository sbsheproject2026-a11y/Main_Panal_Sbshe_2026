using Application.DTOs;
using Application.Helper;
using Application.Repository;
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
        private readonly ILogger<HomeController> _logger;
        private static readonly SemaphoreSlim _fileLock = new(50, 50);

        public HomeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _appEnvironment = appEnvironment;
            _logger = logger;
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

                // ===== Success Case =====
                if (msg == "Successfully")
                {
                    printmsg1 =
                        $"Dear {model.Name},\n\n" +
                        $"Congratulations! Your Admission Consultant registration has been submitted successfully.\n\n" +
                        $"Login Details:\n" +
                        $"Username: {model.UserName}\n" +
                        $"Password: {model.Password}\n\n" +
                        $"Please keep your login credentials safe and do not share them with anyone.\n" +
                        $"You can now log in to your Admission Consultant dashboard to access your referred students, commissions, and reports.\n" +
                        $"Our team will review your registration details and contact you shortly regarding the next steps.\n\n" +
                        $"Thank you for registering with us.\n" +
                        $"Regards,\n" +
                        $"Team Shaheed Bhagat Singh Health & Education";

                    // ===== Email Bhejo =====
                    // Local variables copy karo — Task.Run safe ke liye
                    string emailTo = model.Email;
                    string emailName = model.Name;
                    string emailUserName = model.UserName;
                    string emailPassword = model.Password;
                    string emailMobile = model.MobileNo;

                    string subject = "Welcome – Admission Consultant Registration Successful 🤝";

                    string emailBody = $@"
<div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;
            border:1px solid #e5e7eb;border-radius:12px;overflow:hidden'>
  <div style='background:#2563eb;color:#fff;padding:20px;text-align:center'>
    <h2 style='margin:0'>Welcome to Shaheed Bhagat Singh Health &amp; Education 🎓</h2>
    <p style='margin:6px 0 0 0;font-size:14px'>Admission Consultant Registration</p>
  </div>
  <div style='padding:24px;color:#111827'>
    <p>Dear <b>{emailName}</b>,</p>
    <p>Congratulations! Your <b>Admission Consultant</b> registration has been submitted successfully. 🎉</p>
    <p><b>Login Details:</b></p>
    <table style='border-collapse:collapse;margin:10px 0'>
      <tr>
        <td style='padding:8px 14px;background:#f3f4f6;border:1px solid #e5e7eb'><b>Username</b></td>
        <td style='padding:8px 14px;border:1px solid #e5e7eb'>{emailUserName}</td>
      </tr>
      <tr>
        <td style='padding:8px 14px;background:#f3f4f6;border:1px solid #e5e7eb'><b>Password</b></td>
        <td style='padding:8px 14px;border:1px solid #e5e7eb'>{emailPassword}</td>
      </tr>
    </table>
    <p>You can now log in to your <b>Admission Consultant</b> dashboard to access your referred students, commissions, and reports.</p>
    <p style='background:#fef3c7;padding:12px;border-radius:8px;border-left:4px solid #f59e0b'>
      ⚠️ <b>Important:</b> Please keep your login credentials safe and do not share them with anyone.
    </p>
    <p>Our team will review your registration details and contact you shortly regarding the next steps.</p>
    <p style='text-align:center;margin:28px 0'>
      <a href='https://shaheedbhagatsinghhealthandeducation.com/'
         style='background:#2563eb;color:#fff;padding:12px 24px;
                text-decoration:none;border-radius:8px;display:inline-block'>
        Login Now
      </a>
    </p>
    <p>If you face any issues, please contact us at contact@shaheedbhagatsinghhealthandeducation.com.</p>
    <p>Thank you for registering with us.</p>
    <p>Regards,<br/><b>Team Shaheed Bhagat Singh Health &amp; Education</b></p>
  </div>
  <div style='background:#f3f4f6;padding:14px;text-align:center;
              font-size:12px;color:#6b7280'>
    © 2026 Shaheed Bhagat Singh Health &amp; Education. All rights reserved.
  </div>
</div>";

                    // Background me bhejo — response slow na ho
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            Email_sms obj = new Email_sms();
                            await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                        }
                        catch (Exception emailEx)
                        {
                            _logger.LogError(emailEx, "Email failed to {Email}", emailTo);
                        }
                    });
                }

                // ===== Response =====
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
                _logger.LogError(ex, "Registration error");
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
            // ✅ Static folder — har request pe create nahi hoga
            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");

            // ✅ Static semaphore — disk IO throttle (max 50 parallel writes)
            // Class level pe rakho (neeche bataya hai)
            // private static readonly SemaphoreSlim _fileLock = new(50, 50);

            var uploadedFiles = new List<string>();
            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // =========================
                // 1. Validation
                // =========================
                if (model == null || string.IsNullOrWhiteSpace(model.Name))
                {
                    return BadRequest(new { success = false, message = "Data is required" });
                }

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    return BadRequest(new { success = false, message = "Email is required" });
                }

                // =========================
                // 2. Folder Create (once)
                // =========================
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // =========================
                // 3. File Upload Helper (Local)
                // =========================
                async Task<string> UploadFile(IFormFile file)
                {
                    // ✅ Full Guid — no collision
                    string fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
                    string filePath = Path.Combine(folderPath, fileName);

                    // ✅ Semaphore — disk IO throttle
                    await _fileLock.WaitAsync();
                    try
                    {
                        using var stream = new FileStream(
                            filePath,
                            FileMode.CreateNew,
                            FileAccess.Write,
                            FileShare.None,
                            bufferSize: 81920,
                            useAsync: true);

                        await file.CopyToAsync(stream);
                    }
                    finally
                    {
                        _fileLock.Release();
                    }

                    lock (uploadedFiles) { uploadedFiles.Add(fileName); }
                    return fileName;
                }

                // =========================
                // 4. ✅ PARALLEL File Uploads
                // =========================
                var uploadTasks = new List<Task>();

                if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
                    uploadTasks.Add(Task.Run(async () =>
                        model.SelfImage = await UploadFile(model.SelfImage1)));

                if (model.SignatureImage1 != null && model.SignatureImage1.Length > 0)
                    uploadTasks.Add(Task.Run(async () =>
                        model.SignatureImage = await UploadFile(model.SignatureImage1)));

                if (model.AadhaarCardFrant1 != null && model.AadhaarCardFrant1.Length > 0)
                    uploadTasks.Add(Task.Run(async () =>
                        model.AadhaarCardFrant = await UploadFile(model.AadhaarCardFrant1)));

                if (model.AadhaarCardBack1 != null && model.AadhaarCardBack1.Length > 0)
                    uploadTasks.Add(Task.Run(async () =>
                        model.AadhaarCardBack = await UploadFile(model.AadhaarCardBack1)));

                // ✅ Academic files — parallel
                if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
                {
                    foreach (var academic in model.AcademicDetails)
                    {
                        if (academic.File != null && academic.File.Length > 0)
                        {
                            var acad = academic;
                            uploadTasks.Add(Task.Run(async () =>
                                acad.FileName = await UploadFile(acad.File)));
                        }
                    }
                }

                // ✅ Sab files ek saath upload
                if (uploadTasks.Count > 0)
                {
                    await Task.WhenAll(uploadTasks);
                }

                // =========================
                // 5. Defaults
                // =========================
                int num = RandomNumberGenerator.GetInt32(10000, 99999);

                model.ReferenceId = model.ReferenceId == 0 ? 3 : model.ReferenceId;
                model.EmployeeId = null;
                model.UserName = model.Email;
                model.GenderId = model.GenderId == 0 ? 43 : model.GenderId;
                model.CasteCategory = model.CasteCategory == 0 ? 60 : model.CasteCategory;
                model.Password = model.DOB?.ToString("ddMMyyyy") ?? "";
                model.StudyModeId = model.StudyModeId == 0 ? 88 : model.StudyModeId;

                // ✅ BCrypt workFactor: 10 (default 11 se tez, same security level)
                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, workFactor: 10);

                // =========================
                // 6. DB Insert
                // =========================
                var result = (await _unitOfWork.Student.CreateNew(model)).Split(",");
                var msg = result[0];
                var entityId = result[1];
                var oId = result[2];

                if (msg != "Successfully")
                {
                    // ✅ Failed → orphan files delete karo
                    await CleanupFilesAsync(folderPath, uploadedFiles);

                    return BadRequest(new
                    {
                        success = false,
                        message = msg
                    });
                }

                // =========================
                // 7. Academic Details Insert
                // =========================
                if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
                {
                    foreach (var academic in model.AcademicDetails)
                    {
                        await _unitOfWork.Student.CreateAcademicDetail(
                            Convert.ToInt32(entityId), academic);
                    }
                }

                // =========================
                // 8. Session Set
                // =========================
                HttpContext.Session.SetString("User", entityId);
                HttpContext.Session.SetString("Key", oId);

                sw.Stop();

                // ✅ Logging (agar _logger inject hai to)
                // _logger.LogInformation("Student created: {Email} in {Ms}ms",
                //     model.Email, sw.ElapsedMilliseconds);

                return Ok(new
                {
                    success = true,
                    message = msg,
                    data = model,
                    userid = entityId,
                    paymentid = oId
                });
            }
            catch (Exception ex)
            {
                sw.Stop();

                // ✅ Orphan files cleanup
                await CleanupFilesAsync(folderPath, uploadedFiles);

                // ✅ Console log — full error visible for debugging
                Console.WriteLine($"[StudentCreate ERROR] Email: {model?.Email} | {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                // ❌ Client ko sirf safe message — stack trace nahi
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong. Please try again."
                });
            }
        }

        // =============================================
        // ✅ Helper — File Cleanup (Controller ke andar)
        // =============================================
        private static async Task CleanupFilesAsync(string folderPath, List<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0) return;

            await Task.Run(() =>
            {
                foreach (var name in fileNames)
                {
                    try
                    {
                        var path = Path.Combine(folderPath, name);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    catch { /* ignore cleanup errors */ }
                }
            });
        }


        //[HttpPost("new-student-create")]
        //public async Task<IActionResult> Create([FromForm] StudentPostDTOs model)
        //{
        //    try
        //    {

        //        if (model == null)
        //        {
        //            return BadRequest(new
        //            {
        //                Success = false,
        //                Message = "data is required"
        //            });
        //        }
        //        if (model.Name == null)
        //        {
        //            return BadRequest(new
        //            {
        //                Success = false,
        //                Message = "data is required"
        //            });
        //        }


        //        string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");
        //        Directory.CreateDirectory(folderPath);

        //        async Task<string> UploadFile(IFormFile file)
        //        {
        //            string fileName = $"{Guid.NewGuid():N}"[..8] + "_" +
        //                              Path.GetFileName(file.FileName);

        //            string filePath = Path.Combine(folderPath, fileName);

        //            using var stream = new FileStream(filePath, FileMode.Create);
        //            await file.CopyToAsync(stream);

        //            return fileName;
        //        }

        //        if (model.SelfImage1 != null && model.SelfImage1.Length > 0)
        //            model.SelfImage = await UploadFile(model.SelfImage1);

        //        if (model.SignatureImage1 != null && model.SignatureImage1.Length > 0)
        //            model.SignatureImage = await UploadFile(model.SignatureImage1);

        //        if (model.AadhaarCardFrant1 != null && model.AadhaarCardFrant1.Length > 0)
        //            model.AadhaarCardFrant = await UploadFile(model.AadhaarCardFrant1);

        //        if (model.AadhaarCardBack1 != null && model.AadhaarCardBack1.Length > 0)
        //            model.AadhaarCardBack = await UploadFile(model.AadhaarCardBack1);


        //        if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
        //        {
        //            foreach (var academic in model.AcademicDetails)
        //            {
        //                if (academic.File != null &&
        //                    academic.File.Length > 0)
        //                {
        //                    academic.FileName = await UploadFile(academic.File);
        //                }
        //            }
        //        }

        //        int num = RandomNumberGenerator.GetInt32(10000, 99999);

        //        model.ReferenceId = model.ReferenceId == 0 ? 3 : model.ReferenceId;
        //        model.EmployeeId = null;
        //        model.UserName = model.Email;
        //        model.GenderId = model.GenderId == 0 ? 43 : model.GenderId;
        //        model.CasteCategory = model.CasteCategory == 0 ? 60 : model.CasteCategory;
        //        model.Password = model.DOB?.ToString("ddMMyyyy") ?? "";
        //        model.StudyModeId = model.StudyModeId == 0 ? 88 : model.StudyModeId;


        //        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashed Password

        //        var result = (await _unitOfWork.Student.CreateNew(model)).Split(",");
        //        var msg = result[0];
        //        var entityId = result[1];
        //        var oId = result[2];

        //        if (msg == "Successfully")
        //        {

        //            if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
        //            {
        //                foreach (var academic in model.AcademicDetails)
        //                {
        //                    await _unitOfWork.Student.CreateAcademicDetail(Convert.ToInt32(entityId), academic);
        //                }
        //            }
        //            // ✅✅✅ SESSION SET KARO (zaroori hai!)
        //            HttpContext.Session.SetString("User", entityId);
        //            HttpContext.Session.SetString("Key", oId);
        //        }
        //        // Default message


        //        return Ok(new
        //        {
        //            success = true,
        //            message = msg,
        //            data = model,
        //            userid = entityId,
        //            paymentid = oId

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "Something went wrong",
        //            error = ex.Message
        //        });
        //    }
        //}

        // ✅ Request Model


        [HttpPost("amount-detail")]
        public async Task<IActionResult> PaymentAmountDetails([FromBody] AmountDetailRequest request)
        {
            try
            {
                // ✅ Validation
                if (request == null || request.UserId <= 0 || request.OId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "userId and oId are required"
                    });
                }

                // ✅ Order details fetch karo
                var list = _unitOfWork.GetWayWorking
                            .OrderListAll("Print", request.OId, request.UserId);

                if (list == null || list.Count == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Order not found"
                    });
                }

                string order_id = list[0].OrderId;
                decimal totalAmountWithGst = list[0].Amount;   // 👈 DB se (already with GST)
                string productId = Convert.ToString(list[0].ProductId);

                // ✅ GST Percent (aapke hisaab se)
                decimal cgstPercent = 9;
                decimal sgstPercent = 9;
                decimal totalGstPercent = cgstPercent + sgstPercent;  // 18%

                // ✅ GST ko alag karo (Reverse Calculation)
                // Total = Base + GST
                // Total = Base × (1 + GST%/100)
                // Base = Total / (1 + GST%/100) = Total × 100 / (100 + GST%)

                decimal divisor = 100 + totalGstPercent;   // 118

                decimal baseAmount = Math.Round(
                    totalAmountWithGst * 100 / divisor, 2
                );

                decimal totalGst = Math.Round(
                    totalAmountWithGst - baseAmount, 2
                );

                // ✅ CGST aur SGST alag karo
                decimal cgst = Math.Round(
                    totalAmountWithGst * cgstPercent / divisor, 2
                );

                decimal sgst = Math.Round(
                    totalAmountWithGst * sgstPercent / divisor, 2
                );

                // ✅ Final total (DB se jo aaya wahi hai)
                decimal total = totalAmountWithGst;

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        orderId = order_id,
                        productId = productId,

                        // Base Amount (bina GST)
                        courseFee = baseAmount,

                        // CGST
                        cgstPercent = cgstPercent,
                        cgst = cgst,

                        // SGST
                        sgstPercent = sgstPercent,
                        sgst = sgst,

                        // Total GST
                        totalGstPercent = totalGstPercent,
                        totalGst = totalGst,

                        // Final Total (with GST)
                        total = total
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Please try again!",
                    error = ex.Message
                });
            }
        }

        [HttpPost("study-center-create")]
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
                string printmsg1 = string.Empty;
                if (msg == "Successfully")
                {

                    if (model.DepartmentIds != null && model.DepartmentIds.Count > 0)
                    {
                        foreach (var academic in model.DepartmentIds)
                        {
                            await _unitOfWork.Frenchise.CreateInstituteDepartmentDetails(Convert.ToInt32(entityId), academic);
                        }
                    }
                    // ============================================================
                    // ✅ PLAIN TEXT MESSAGE (frontend ke liye)
                    // ============================================================
                    printmsg1 =
                        $"Dear {model.FatherName},\n\n" +
                        $"Congratulations! Your Study Centre registration has been submitted successfully.\n\n" +
                        $"Registration Details:\n" +
                        $"Centre Name: {model.Name}\n\n" +
                        $"Login Details:\n" +
                        $"Username: {model.UserName}\n" +
                        $"Password: {model.Password}\n\n" +
                        $"Your registration details and submitted documents will now be reviewed by our team.\n" +
                        $"Once the verification process is completed, our team will contact you regarding the approval and further onboarding process.\n\n" +
                        $"Please keep your login credentials safe and do not share them with anyone.\n\n" +
                        $"Thank you for choosing to register as an authorized Study Centre with us.\n" +
                        $"Regards,\n" +
                        $"Team Shaheed Bhagat Singh Health & Education";

                    // ============================================================
                    // ✅ EMAIL BHEJO (background me)
                    // ============================================================
                    string emailTo = model.Email;
                    string emailFatherName = model.FatherName ?? "Sir/Madam";
                    string emailCentreName = model.Name ?? "Study Centre";
                    string emailUserName = model.UserName ?? "";
                    string emailPassword = model.Password ?? "";

                    string subject = "Welcome – Study Centre Registration Successful 🏫";

                    string emailBody = $@"
<div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;
            border:1px solid #e5e7eb;border-radius:12px;overflow:hidden'>
  <div style='background:#2563eb;color:#fff;padding:20px;text-align:center'>
    <h2 style='margin:0'>Welcome to Shaheed Bhagat Singh Health &amp; Education 🎓</h2>
    <p style='margin:6px 0 0 0;font-size:14px'>Study Centre Registration</p>
  </div>

  <div style='padding:24px;color:#111827'>
    <p>Dear <b>{emailFatherName}</b>,</p>

    <p>Congratulations! Your <b>Study Centre</b> registration has been submitted successfully. 🎉</p>

    <p><b>Registration Details:</b></p>
    <table style='border-collapse:collapse;margin:10px 0'>
      <tr>
        <td style='padding:8px 14px;background:#f3f4f6;border:1px solid #e5e7eb'><b>Centre Name</b></td>
        <td style='padding:8px 14px;border:1px solid #e5e7eb'>{emailCentreName}</td>
      </tr>
    </table>

    <p><b>Login Details:</b></p>
    <table style='border-collapse:collapse;margin:10px 0'>
      <tr>
        <td style='padding:8px 14px;background:#f3f4f6;border:1px solid #e5e7eb'><b>Username</b></td>
        <td style='padding:8px 14px;border:1px solid #e5e7eb'>{emailUserName}</td>
      </tr>
      <tr>
        <td style='padding:8px 14px;background:#f3f4f6;border:1px solid #e5e7eb'><b>Password</b></td>
        <td style='padding:8px 14px;border:1px solid #e5e7eb'>{emailPassword}</td>
      </tr>
    </table>

    <p>You can now log in to your <b>Study Centre</b> dashboard to manage your institute, students, and other activities.</p>

    <p style='background:#fef3c7;padding:12px;border-radius:8px;border-left:4px solid #f59e0b'>
      ⚠️ <b>Important:</b> Please keep your login credentials safe and do not share them with anyone.
    </p>

    <p>Your registration details and submitted documents will now be reviewed by our team. Once the verification process is completed, our team will contact you regarding the approval and further onboarding process.</p>

    <p style='text-align:center;margin:28px 0'>
      <a href='https://shaheedbhagatsinghhealthandeducation.com/'
         style='background:#2563eb;color:#fff;padding:12px 24px;
                text-decoration:none;border-radius:8px;display:inline-block'>
        Login Now
      </a>
    </p>

    <p>If you face any issues, please contact us at contact@shaheedbhagatsinghhealthandeducation.com.</p>

    <p>Thank you for choosing to register as an authorized Study Centre with us.</p>

    <p>Regards,<br/>
       <b>Team Shaheed Bhagat Singh Health &amp; Education</b></p>
  </div>

  <div style='background:#f3f4f6;padding:14px;text-align:center;
              font-size:12px;color:#6b7280'>
    © 2026 Shaheed Bhagat Singh Health &amp; Education. All rights reserved.
  </div>
</div>";

                    // Background me bhejo — response slow na ho
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            Email_sms obj = new Email_sms();
                            await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                        }
                        catch (Exception emailEx)
                        {
                            _logger.LogError(emailEx, "Study Centre email failed to {Email}", emailTo);
                        }
                    });
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
                _logger.LogError(ex, "Study Centre registration error");
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

                    //data
                    registrationLine = data.RegistrationLine ?? "",
                    certificateTitle = data.CertificateTitle ?? "",
                    certifyLine = data.CertifyLine ?? "",
                    centreAddress = data.CentreAddress ?? "",
                    appointedLine = data.AppointedLine ?? "",
                    signatoryLine = data.SignatoryLine ?? "",
                    officeAddress = data.OfficeAddress ?? "",
                    corporateOffice = data.CorporateOffice ?? "",
                    emailLine = data.EmailLine ?? "",
                    websiteLine = data.WebsiteLine ?? "",

                    //image
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



        [HttpGet("student-details-print/{id}")]

        public async Task<IActionResult> StudentDetailsPrint(int id)
        {
            try
            {
                var data = await _unitOfWork.Student.GetStudentDetailsPrint(id);

                if (data == null)
                {
                    return NotFound(new { message = "Student not found" });
                }

                var academicDetailsData = await _unitOfWork.Student.GetAcademicDetailsById(id);
                var franchisedata = await _unitOfWork.Frenchise.GetDetaiId((int)data.ReferenceId);

                // 👇 Academic list me har row me levelName add karo
                var academicWithLevel = new List<object>();
                if (academicDetailsData != null && academicDetailsData.Any())
                {
                    foreach (var ac in academicDetailsData)
                    {
                        var levelMaster = await _unitOfWork.DropDown.GetMasterDetail(Convert.ToInt32(ac.LevelTypeId));

                        academicWithLevel.Add(new
                        {
                            id = ac.Id,
                            levelTypeId = ac.LevelTypeId,
                            levelName = levelMaster?.Name ?? "",     // 👈 Level ka naam
                            entityId = ac.EntityId,
                            schoolCollege = ac.SchoolCollege,
                            rollNo = ac.RollNo,
                            boardUniversity = ac.BoardUniversity,
                            percentageCgpa = ac.PercentageCgpa,

                            fileName = ac.FileName
                        });
                    }
                }

                return Ok(new
                {
                    studentdata = new
                    {
                        id = data.Id,
                        code = data.Code,
                        name = data.Name,
                        studentNameHindi = data.StudentNameHindi,
                        fatherName = data.FatherName,
                        fatherNameHindi = data.FatherNameHindi,
                        motherName = data.MotherName,
                        dob = data.DOB,
                        genderId = data.GenderId,
                        mobileNo = data.MobileNo,
                        whatsAppNo = data.WhatsAppNo,
                        email = data.Email,
                        idNumber = data.IdNumber,
                        address = data.Address,
                        pincode = data.Pincode,
                        stateId = data.StateId,
                        districtId = data.DistrictId,
                        locationId = data.LocationId,
                        courseTypeId = data.CourseTypeId,
                        courseCategoryId = data.CourseCategoryId,
                        courseId = data.CourseId,
                        courseCode = data.CourseCode,
                        duration = data.Duration,
                        examSessionId = data.ExamSessionId,
                        casteCategoryId = data.CasteCategoryId,
                        selfImageShow = data.SelfImage,
                        signatureImageShow = data.SignatureImage,
                        aadhaarCardFrantShow = data.AadhaarCardFrant,
                        aadhaarCardBackShow = data.AadhaarCardBack
                    },
                    academicDetails = academicWithLevel,   // 👈 yahan bhej do
                    franchise = franchisedata == null ? null : new
                    {
                        id = franchisedata.Id,
                        code = franchisedata.Code,
                        name = franchisedata.Name,
                        fatherName = franchisedata.FatherName,
                        address = franchisedata.Address,
                        pincode = franchisedata.Pincode,
                        mobileNo = franchisedata.MobileNo,
                        whatsAppNo = franchisedata.WhatsAppNo,
                        email = franchisedata.Email,
                        cityName = franchisedata.CityName,
                        districtName = franchisedata.DistrictName,
                        stateName = franchisedata.StateName,
                        affiliation = franchisedata.Affiliation
                    }
                });
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


        [HttpPost("new-student")]
        public async Task<IActionResult> CreateRegiste([FromForm] StudentPostDTOs model)
        {
            try
            {


                var msg = "Successfully";
                model.Name = "Ajay Kumar";
                model.Email = "kashniaajay73@gmail.com";

                if (msg == "Successfully")
                {






                }

                return Ok(new
                {
                    success = true,
                    message = msg,
                    data = model

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


