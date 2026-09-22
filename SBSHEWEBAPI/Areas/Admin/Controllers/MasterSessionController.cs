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
    public class MasterSessionController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public MasterSessionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("mastersession-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.MasterSession.GetAll();

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

        [HttpPost("mastersession-create")]
        public async Task<IActionResult> Create(MasterSessionPostDTOs model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "data is required"
                });
            }
            var result = await _unitOfWork.MasterSession.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("mastersession-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.MasterSession.GetById(id);

            if (data == null)
            {
                return NotFound("MasterSession not found");
            }

            return Ok(data);
        }

        [HttpPut("mastersession-update")]
        public async Task<IActionResult> Update(MasterSessionPostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.MasterSession.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("mastersession-delete/{id}")]
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

            var result = await _unitOfWork.MasterSession.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

 

    }
}
