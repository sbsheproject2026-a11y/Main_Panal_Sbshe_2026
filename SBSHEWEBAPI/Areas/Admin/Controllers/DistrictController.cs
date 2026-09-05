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
    public class DistrictController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public DistrictController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("district-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.Location.GetAll(2);

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

        [HttpPost("district-create")]
        public async Task<IActionResult> Create(LocationPostDTOs model)
        {
            model.TypeId = 2;
            var result = await _unitOfWork.Location.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("district-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Location.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("district-update")]
        public async Task<IActionResult> Update(LocationPostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            model.TypeId = 2;
            var result = await _unitOfWork.Location.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("district-delete/{id}")]
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

            var result = await _unitOfWork.Location.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        //----------------------------Drop Down  State-----------------------------------//

      
    }
}
