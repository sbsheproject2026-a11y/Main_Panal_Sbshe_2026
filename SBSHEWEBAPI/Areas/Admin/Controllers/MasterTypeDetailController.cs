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
    public class MasterTypeDetailController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public MasterTypeDetailController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }
        [HttpGet("mastertypedetail-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.MasterTypeDetail.GetAll();

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

        [HttpPost("mastertypedetail-create")]
        public async Task<IActionResult> Create([FromForm] MasterTypeDetailPostDTOs model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "data is required"
                });
            }
            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");
            Directory.CreateDirectory(folderPath);

            async Task<string> UploadFile(IFormFile file)
            {
                string fileName = $"{Guid.NewGuid():N}"[..8] + "_" +
                                  Path.GetFileName(file.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;
            }

            if (model.File != null && model.File.Length > 0)
                model.FilePath = await UploadFile(model.File);


            var result = await _unitOfWork.MasterTypeDetail.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("mastertypedetail-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.MasterTypeDetail.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("mastertypedetail-update")]
        public async Task<IActionResult> Update([FromForm] MasterTypeDetailPostDTOs model)
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

            async Task<string> UploadFile(IFormFile file)
            {
                string fileName = $"{Guid.NewGuid():N}"[..8] + "_" +
                                  Path.GetFileName(file.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;
            }

            if (model.File != null && model.File.Length > 0)
                model.FilePath = await UploadFile(model.File);
            var result = await _unitOfWork.MasterTypeDetail.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("mastertypedetail-delete/{id}")]
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

            var result = await _unitOfWork.MasterTypeDetail.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


        //----------------------------Drop Down Master Type-----------------------------------//

      

    }
}
