using Application;
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
    public class MasterTypeController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public MasterTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("mastertype-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.MasterType.GetAll();

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

        [HttpPost("mastertype-create")]
        public async Task<IActionResult> Create(MasterTypePostDTOs model)
        {

            if (model == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "data is required"
                });
            }
            var result = await _unitOfWork.MasterType.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("mastertype-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.MasterType.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("mastertype-update")]
        public async Task<IActionResult> Update(MasterTypePostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.MasterType.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("mastertype-delete/{id}")]
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

            var result = await _unitOfWork.MasterType.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

         


    }
}
