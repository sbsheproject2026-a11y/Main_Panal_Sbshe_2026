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
    public class LectureController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public LectureController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("lecture-getall/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _unitOfWork.Lecture.GetAll(id);

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

        [HttpPost("lecture-create-subject")]
        public async Task<IActionResult> Create(LecturePostDTOs model)
        {
            var result = await _unitOfWork.Lecture.CreateSubject(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("lecture-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Lecture.GetById(id);

            if (data == null)
            {
                return NotFound("Lecture not found");
            }

            return Ok(data);
        }

        [HttpPut("lecture-update-subject/{id}")]
        public async Task<IActionResult> Update( int id,[FromBody] LecturePostDTOs model)
        {
            model.Id = id;
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
           
            var result = await _unitOfWork.Lecture.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("lecture-delete/{id}")]
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

            var result = await _unitOfWork.Lecture.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

         


    }
}
