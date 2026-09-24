using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminWalletController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AdminWalletController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;

        }

        [HttpGet("wallet-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.Wallet.GetAll(0,112);

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
        [HttpGet("penfing-wallet-getall")]
        public async Task<IActionResult> GetAllPending()
        {
            try
            {
                var data = await _unitOfWork.Wallet.GetAllPending(1,112);

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

        [HttpPost("wallet-create")]
        public async Task<IActionResult> Create([FromForm]WalletPostDTOs model)
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

                if (model.SlipUpload1 != null && model.SlipUpload1.Length > 0)
                    model.SlipUpload = await UploadFile(model.SlipUpload1);

                model.Status = 116;
                var result = await _unitOfWork.Wallet.Create(model);

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

     
        [HttpPost("PaymentConfirm")]
        public async Task<IActionResult> PaymentConfirm([FromForm] List<int> Ids)
        {
            try
            {
                if (Ids == null || Ids.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "At least one Id is required."
                    });
                }

                var results = new List<object>();

                foreach (var id in Ids)
                {
                    var result = await _unitOfWork.Wallet.WalletPaymentConfirm(id, 116);
                    results.Add(new { id, result });
                }

                return Ok(new
                {
                    success = true,
                    message = $"{Ids.Count} request(s) confirmed successfully.",
                    data = results
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

