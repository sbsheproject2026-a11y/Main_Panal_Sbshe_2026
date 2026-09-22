using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteContentController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public WebsiteContentController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }
        [HttpGet("website-content-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.WebsiteContent.GetAllData();

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



        [HttpPost("website-content-create")]

        public async Task<IActionResult> Create([FromForm] WebsiteContentPostDTOs model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Request data is required."
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
            if (model.ProductCategoryDocuments != null && model.ProductCategoryDocuments.Count > 0)
            {
                foreach (var academic in model.ProductCategoryDocuments)
                {
                    if (academic.File != null &&
                        academic.File.Length > 0)
                    {
                        academic.Path = await UploadFile(academic.File);
                    }
                }
            }
            var result = await _unitOfWork.WebsiteContent.Create(model);

            if (string.IsNullOrEmpty(result) ||
                result.StartsWith("Something went wrong"))
            {
                return BadRequest(new
                {
                    success = false,
                    message = result ?? "Something went wrong."
                });
            }

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("website-content-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.WebsiteContent.GetbyId(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("website-content-update")]
        public async Task<IActionResult> Update([FromForm] WebsiteContentPostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.WebsiteContent.Update(model);

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
