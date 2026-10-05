using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestStudentController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public RequestStudentController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }


        [HttpGet("request-student-confirm-to-admin")]

        public async Task<IActionResult> GetConfirmToAdmin(int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {


                var data = await _unitOfWork.Student.GetConfirmToAdmin(pageNo, pageSize, search);

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

        [HttpGet("request-student-confirm-addmissions")]

        public async Task<IActionResult> GetConfirmAddmissions(int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {


                var data = await _unitOfWork.Student.GetConfirmAddmissions(null,pageNo, pageSize, search);

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




        [HttpPut("request-student-confirm")]
        public async Task<IActionResult> ConfirmToAdmin([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("Please provide at least one student ID.");
            }

            var results = new List<object>();

            foreach (var id in ids)
            {
                var result = await _unitOfWork.Student.ConfirmToAdmin(id);

                results.Add(new
                {
                    id = id,
                    message = result
                });
            }

            return Ok(new
            {
                success = true,
                message = "Students processed successfully.",
                data = results
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


        [HttpGet("request-student-unsetmarks")]

        public async Task<IActionResult> GetUnSetMarksStudents(int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {


                var data = await _unitOfWork.Student.UnSetMarksStudents(null,pageNo, pageSize, search);

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


        [HttpGet("student-getsubject/{id}")]
        public async Task<IActionResult> getStudentSubjectMarks(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound("Subject not found");
                }
                var data = await _unitOfWork.Student.getStudentSubjectMarks(id);

                if (data == null)
                {
                    return NotFound("student Subject not found");
                }

                return Ok(new
                {
                    Success = true,
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

        [HttpPost("student-set-marks")]
        public async Task<IActionResult> StudentSetMarks([FromBody] StudentMarksDto model)
        {
            try
            {
                // Model null check
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request data is required."
                    });
                }

                // Marks null / empty check
                if (model.Marks == null || !model.Marks.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "No marks found."
                    });
                }

                // Abhi data valid hai, tabhi service call hogi
                var result = await _unitOfWork.Student.SaveStudentMarks(model);

                return Ok(new
                {
                    success = true,
                    message = result
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

        
        [HttpGet("request-upgrade-student-list")]
        public async Task<IActionResult> GetUpgradeStudentList(int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {


                var data = await _unitOfWork.Student.GetUpgradeStudentList(pageNo, pageSize, search);

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

        [HttpPost("request-upgrade-addclass")]
        public async Task<IActionResult> AddClassStudent([FromBody] StudentClassDto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request data is required."
                    });
                }

                 

                if (model.ExamSessionId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Exam Session is required."
                    });
                }

                if (model.PUid == null || !model.PUid.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Please select at least one student."
                    });
                }

                var results = new List<string>();

                foreach (var puid in model.PUid)
                {
                    var studentModel = new StudentClassDto
                    {
                        //ClassId = model.ClassId,
                        ExamSessionId = model.ExamSessionId,
                        PUid = new List<int> { puid }
                    };

                    var result = await _unitOfWork.Student.AddClassStudent(studentModel);

                    results.Add(result);
                }

                return Ok(new
                {
                    success = true,
                    message = "Students processed successfully.",
                    totalStudents = model.PUid.Count,
                    results = results
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

