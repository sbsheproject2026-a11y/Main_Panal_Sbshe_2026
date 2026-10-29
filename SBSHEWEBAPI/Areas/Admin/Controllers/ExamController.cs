using Application;
using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public ExamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("exam-detail-getall/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _unitOfWork.Exam.GetAll(id);

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

        [HttpPost("exam-create")]
        public async Task<IActionResult> Create(ExamPostDTOs model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "data is required"
                });
            }
            var result = await _unitOfWork.Exam.CreatePaper(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("exam-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Exam.GetById(id);

            if (data == null)
            {
                return NotFound("Lecture not found");
            }

            return Ok(data);
        }

      
        [HttpPost("exam-update-question")]
        public async Task<IActionResult> Update([FromBody] ExamPostDTOs model)
        {
            // ✅ Check karo ki body me ID aayi hai ya nahi
            if (model == null || model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required in the request body."
                });
            }

            try
            {
                // ✅ Yahan _unitOfWork.Exam.Update(model) call hoga
                var result = await _unitOfWork.Exam.Update(model);

                return Ok(new
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                // Agar Service me koi error aaye (jaise "Question name is required") to 500 error bhejega
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpDelete("exam-delete/{id}")]
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

            var result = await _unitOfWork.Exam.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


        [HttpGet("student-exam-detail/{id}")]
        public async Task<IActionResult> StudentExamDetails(int id)
        {
            try
            {
                var data = await _unitOfWork.Exam.StudentExamDetails(id);

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
        public async Task<IActionResult> GetExamQuestionDetails(int id)
        {
            try
            {
                var data = await _unitOfWork.Exam.GetExamQuestionDetails(id);

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




    }
}
