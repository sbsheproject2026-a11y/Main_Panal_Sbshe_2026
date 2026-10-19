using Application.DTOs;
using Application.Helper;
using Application.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ILogger<StudentController> _logger;
        private static readonly SemaphoreSlim _fileLock = new(50, 50);

        public StudentController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, ILogger<StudentController> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("student-getall")]

        public async Task<IActionResult> Get(int? referenceId, int? pageNo = null, int? pageSize = null, string? search = null, int studyModeId = 0)
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
                pageNo ??= 1;
                pageSize ??= 100;

                var (data, totalCount) = await _unitOfWork.Student.GetAll(finalUserId, pageNo, pageSize, search, studyModeId);

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
        [Authorize]



        [Authorize]
        [HttpPost("student-create")]
        public async Task<IActionResult> Create([FromForm] StudentPostDTOs model)
        {
            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");

            try
            {
                // =========================
                // 1. Auth — User context
                // =========================
                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                var userId = claims.GetValueOrDefault("UserId");

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { success = false, message = "Session expired. Please login again." });

                // =========================
                // 2. Folder Create (once)
                // =========================
                Directory.CreateDirectory(folderPath);

                // =========================
                // 3. File Upload Helper
                // =========================
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
                        if (academic.File != null && academic.File.Length > 0)
                            academic.FileName = await UploadFile(academic.File);
                    }
                }

                // =========================
                // 4. Defaults
                // =========================
                model.ReferenceId = model.ReferenceId == 0 ? Convert.ToInt32(userId) : model.ReferenceId;
                model.EmployeeId = Convert.ToInt32(userId);
                model.UserName = model.Email;
                model.Password = model.DOB?.ToString("ddMMyyyy") ?? "";

                model.StudyModeId = model.StudyModeId == 0 ? 89 : model.StudyModeId;
                model.GenderId = model.GenderId == 0 ? 43 : model.GenderId;
                model.CasteCategory = model.CasteCategory == 0 ? 60 : model.CasteCategory;

                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, workFactor: 12);

                // =========================
                // 5. DB Insert
                // =========================
                //var result = (await _unitOfWork.Student.Create(model)).Split(",");
                //var msg = result[0];
                //var entityId = result[1];
                var raw = await _unitOfWork.Student.Create(model);
                var parts = raw.Split(',');

                var msg = parts.Length > 0 ? parts[0] : "Failed";
                var entityId = parts.Length > 1 ? parts[1] : "0";

                if (msg != "Successfully")
                {
                    return BadRequest(new { success = false, message = msg });
                }

                // ✅ Fix: DB fail → user ko seedha error do
                if (msg != "Successfully")
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = msg
                    });
                }

                // =========================
                // 6. Academic Details Insert
                // =========================
                if (model.AcademicDetails != null && model.AcademicDetails.Count > 0)
                {
                    foreach (var academic in model.AcademicDetails)
                    {
                        await _unitOfWork.Student.CreateAcademicDetail(
                            Convert.ToInt32(entityId), academic);
                    }
                }

                // ============================================================
                // 7. 📧 EMAIL — fire & forget, fully isolated
                //    Mail fail ho ya setup fail ho, response pe koi asar nahi
                // ============================================================
                try
                {
                    // ⚠️ Task.Run se pehle LOCAL variables me copy karo
                    string emailTo = model.UserName;
                    string emailName = model.Name ?? "Student";
                    string emailUserName = model.UserName ?? "";
                    string emailPassword = string.IsNullOrWhiteSpace(model.Password)
                                            ? "(please contact support)"
                                            : model.Password;

                    string subject = "Registration Successful – Login Details Inside";

                    string emailBody = $@"
<div style='font-family:Arial,Helvetica,sans-serif;font-size:15px;color:#222;
            line-height:1.7;max-width:600px;margin:0 auto;padding:24px;'>

  <p>Dear <b>{emailName}</b>,</p>

  <p>Your registration has been <b>successfully submitted</b>. ✅</p>

  <p>Below are your login details:</p>

  <ul style='margin:10px 0 20px 20px;padding:0;'>
    <li style='margin-bottom:8px;'>
      <b>Username:</b> {emailUserName}
      <span style='color:#666;font-size:13px;'>(your email address)</span>
    </li>
    <li style='margin-bottom:8px;'>
      <b>Password:</b> {emailPassword}
      <span style='color:#666;font-size:13px;'>(your date of birth in DDMMYYYY format)</span>
    </li>
  </ul>

  <p>You can now log in to your dashboard using the credentials above.</p>

  <p>Please keep your login details safe and do not share them with anyone.</p>

  <p>If you face any issues, feel free to contact us at
     <a href='mailto:contact@shaheedbhagatsinghhealthandeducation.com'
        style='color:#1a73e8;text-decoration:none;'>
       contact@shaheedbhagatsinghhealthandeducation.com
     </a>.
  </p>

  <p style='margin-top:24px;'>Regards,<br/>
     <b>Team Shaheed Bhagat Singh Health &amp; Education</b>
  </p>

  <hr style='border:none;border-top:1px solid #e5e5e5;margin:24px 0 12px 0;' />

  <p style='font-size:12px;color:#888;margin:0;'>
    This is an automated email. Please do not reply to this message.
  </p>

</div>";

                    // Background me bhejo — response block nahi hoga
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            Email_sms obj = new Email_sms();
                            await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                        }
                        catch (Exception mailEx)
                        {
                            _logger.LogError(mailEx, "Student email failed to {Email}", emailTo);
                        }
                    });
                }
                catch (Exception setupEx)
                {
                    // Email setup fail ho jaye (string build, class init) to bhi response safe
                    _logger.LogError(setupEx, "Email dispatch setup failed for {Email}", model.Email);
                }

                // =========================
                // 8. Response
                // =========================
                return Ok(new
                {
                    success = true,
                    message = msg,
                    userid = entityId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Franchise → Student create failed for {Email}", model?.Email);

                return BadRequest(new
                {
                    success = false,
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }
       
        
        [HttpPost("send-to-student-create")]

        public async Task<IActionResult> CreateSend()
        {
             

            try
            {
                 
                
                try
                {
                    // ⚠️ Task.Run se pehle LOCAL variables me copy karo
                    string emailTo = "ajaykumar72913@gmail.com";
                    string emailName =   "Ajay Kumar";
                    string emailUserName = "ajaykumar72913@gmail.com";
                    string emailPassword = "(please contact support)";
                                             

                    string subject = "Registration Successful – Login Details Inside";

                    string emailBody = $@"
<div style='font-family:Arial,Helvetica,sans-serif;font-size:15px;color:#222;
            line-height:1.7;max-width:600px;margin:0 auto;padding:24px;'>

  <p>Dear <b>{emailName}</b>,</p>

  <p>Your registration has been <b>successfully submitted</b>. ✅</p>

  <p>Below are your login details:</p>

  <ul style='margin:10px 0 20px 20px;padding:0;'>
    <li style='margin-bottom:8px;'>
      <b>Username:</b> {emailUserName}
      <span style='color:#666;font-size:13px;'>(your email address)</span>
    </li>
    <li style='margin-bottom:8px;'>
      <b>Password:</b> {emailPassword}
      <span style='color:#666;font-size:13px;'>(your date of birth in DDMMYYYY format)</span>
    </li>
  </ul>

  <p>You can now log in to your dashboard using the credentials above.</p>

  <p>Please keep your login details safe and do not share them with anyone.</p>

  <p>If you face any issues, feel free to contact us at
     <a href='mailto:contact@shaheedbhagatsinghhealthandeducation.com'
        style='color:#1a73e8;text-decoration:none;'>
       contact@shaheedbhagatsinghhealthandeducation.com
     </a>.
  </p>

  <p style='margin-top:24px;'>Regards,<br/>
     <b>Team Shaheed Bhagat Singh Health &amp; Education</b>
  </p>

  <hr style='border:none;border-top:1px solid #e5e5e5;margin:24px 0 12px 0;' />

  <p style='font-size:12px;color:#888;margin:0;'>
    This is an automated email. Please do not reply to this message.
  </p>

</div>";

                    // Background me bhejo — response block nahi hoga
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            Email_sms obj = new Email_sms();
                            await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                        }
                        catch (Exception mailEx)
                        {
                            _logger.LogError(mailEx, "Student email failed to {Email}");
                        }
                    });
                }
                catch (Exception setupEx)
                {
                    // Email setup fail ho jaye (string build, class init) to bhi response safe
                    _logger.LogError(setupEx, "Email dispatch setup failed for {Email}");
                }

                // =========================
                // 8. Response
                // =========================
                return Ok(new
                {
                    success = true 
                    
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Franchise → Student create failed for {Email}");

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

                var academicDetailsData = await _unitOfWork.Student.GetAcademicDetailsById(id);



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

