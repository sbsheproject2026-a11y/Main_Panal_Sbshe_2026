using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Student.Controllers
{
    [Area("Student")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamStudentController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;

        public ExamStudentController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _appEnvironment = appEnvironment;
        }

       
        [HttpGet("student-exam-detail/{id}")]
        public async Task<IActionResult> StudentExamDetails(int id, int userId)
        {
            try
            {
                var data = await _unitOfWork.Exam.StudentExamDetails(id, userId);

                return Ok(new
                {
                    Success = true,
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


        [HttpGet("student-exam-question-detail/{id}")]
        public async Task<IActionResult> StartExam(int id, [FromQuery] int userId)
        {
            try
            {
                var entry = await _unitOfWork.Exam.ExamStart(id, userId);
                var data = await _unitOfWork.Exam.GetExamQuestionDetails(id);

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Entrydata = entry
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



       

        [HttpPost("student-exam-end")]
        public async Task<IActionResult> EndExam([FromForm] SubmitExamDto model)
        {
            try
            {
                // 1. IDs Check
                if (model.PaperId == null || model.UserId == null || model.AttemptId == null)
                {
                    return BadRequest(new { Success = false, Message = "PaperId, UserId, and AttemptId are required." });
                }

                // 2. File Check
                if (model.Path1 == null || model.Path1.Length == 0)
                {
                    return BadRequest(new { Success = false, Message = "Answer file is required." });
                }

                // ✅ 3. YAHAN PAR FILE KA SIZE CHECK HO RAHA HAI (Direct Controller me)
                // 2MB = 2 * 1024 * 1024 bytes
                long maxFileSize = 2 * 1024 * 1024;
                if (model.Path1.Length > maxFileSize)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "File size exceeds the 2MB limit. Please compress it and upload again."
                    });
                }

                // 4. Folder Path Check
                string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 5. File Save (Bina compress kiye, jitni size ki hai utni hi save hogi)
                string fileName = $"{Guid.NewGuid():N}".Substring(0, 8) + "_" + Path.GetFileName(model.Path1.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Path1.CopyToAsync(stream);
                }

                model.Path = fileName;

                // 6. Database Entry
                var entry = await _unitOfWork.Exam.ExamEnd(model);

                return Ok(new
                {
                    Success = true,
                    Message = "Exam submitted successfully.",
                    Data = entry
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Something went wrong: " + ex.Message
                });
            }
        }

    }
}



