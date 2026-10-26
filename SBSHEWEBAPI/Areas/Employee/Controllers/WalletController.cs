using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public WalletController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet("emp-wallet-getall")]
        public async Task<IActionResult> Get([FromQuery] int? userId)
        {
            try
            {
                int finalUserId;

                if (userId.HasValue && userId.Value > 0)
                {
                    finalUserId = userId.Value;
                }
                else
                {
                    var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    var tokenUserId = claims.GetValueOrDefault("UserId");

                    if (string.IsNullOrEmpty(tokenUserId))
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            Message = "UserId not found"
                        });
                    }

                    finalUserId = Convert.ToInt32(tokenUserId);
                }
                var data = await _unitOfWork.Wallet.GetAll(finalUserId, 112);

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

        [HttpPost("emp-wallet-create")]

     
        public async Task<IActionResult> Create([FromForm] WalletPostDTOs model)
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


                if (!model.FranchiseId.HasValue || model.FranchiseId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Franchise is required."
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

                model.Status = 115;
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
       
        [HttpPost("emp-online-wallet-create")]
         public async Task<IActionResult> Createonline([FromForm] WalletPostDTOs model)
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


                if (!model.FranchiseId.HasValue || model.FranchiseId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Franchise is required."
                    });
                }
                if (!model.TotalAmount.HasValue || model.TotalAmount.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Amount is required."
                    });
                }
                

                 var result = (await _unitOfWork.GetWayWorking.GenrateOrder((int)model.FranchiseId,Convert.ToDecimal(model.TotalAmount),4,1)).Split(",");
                var msg = result[0];
                var entityId = result[1];
                var oId = result[2];
                HttpContext.Session.SetString("User", entityId);
                HttpContext.Session.SetString("Key", oId);
                // ✅ IDs frontend ko bhi bhejo
                return Ok(new
                {
                    success = true,
                    message = msg,
                    userid = entityId,      // ← Ye frontend bhejega payment pe
                    paymentid = oId         // ← Ye bhi
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

