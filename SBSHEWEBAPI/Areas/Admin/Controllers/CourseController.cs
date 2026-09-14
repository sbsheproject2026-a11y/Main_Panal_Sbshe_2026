using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CourseController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpGet("course-getall")]

        public async Task<IActionResult> Get(int referenceId = 1, int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {
                var data = await _unitOfWork.Course.GetAll(referenceId, pageNo, pageSize, search);

                return Ok(new
                {
                    Success = true,
                    PageNo = pageNo,
                    PageSize = pageSize,
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

        [HttpPost("course-create")]
        public async Task<IActionResult> Create(CoursePostDTOs model)
        {
            try
            {


                var result = await _unitOfWork.Course.Create(model);

                return Ok(new
                {
                    success = true,
                    message = result
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


        [HttpGet("course-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Course.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("course-update")]
        public async Task<IActionResult> Update(CoursePostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
            model.ParentId = model.ParentId == 0 ? null : model.ParentId;
            var result = await _unitOfWork.Course.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        [HttpDelete("course-delete/{id}")]
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

            var result = await _unitOfWork.Course.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }



        [HttpPost("CreateCourseAmount")]
        public async Task<IActionResult> CreateCourseAmount([FromBody] List<ProductAmountDtos> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return BadRequest("At least one Amount is required");
                }

                var result = await _unitOfWork.Course.CreateCourseAmount(model);

                if (result.Contains("Something went wrong"))
                {
                    return StatusCode(500, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }


        //----------------------------Course Drop Down All--------------------

    }
}

