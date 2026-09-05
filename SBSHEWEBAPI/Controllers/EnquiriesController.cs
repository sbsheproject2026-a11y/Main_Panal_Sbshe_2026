using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnquiriesController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public EnquiriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("Enquiries-getall")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _unitOfWork.Enquiries.GetAll(id);

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


        [HttpPost("student-Enquiries")]
        public async Task<IActionResult> CreateStudent(EnquiriesPostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Enquiry data is required."
                    });
                }
                model.TypeId = 1;
                var result = await _unitOfWork.Enquiries.Create(model);

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while creating enquiry.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }
        [HttpPost("franchise-Enquiries")]
        public async Task<IActionResult> CreateFranchise(EnquiriesPostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Enquiry data is required."
                    });
                }
                model.TypeId = 2;
                var result = await _unitOfWork.Enquiries.Create(model);

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while creating enquiry.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }


        [HttpDelete("Enquiries-delete/{id}")]
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

            var result = await _unitOfWork.Enquiries.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


    }
}
