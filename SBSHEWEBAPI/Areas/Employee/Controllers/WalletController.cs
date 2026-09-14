using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public WalletController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet("emp-wallet-getall")]
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

        [HttpPost("emp-wallet-create")]
        public async Task<IActionResult> Create(LocationPostDTOs model)
        {

            var result = await _unitOfWork.Location.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }



    }
}

