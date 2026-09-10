using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Create(LoginDTOs model)
        {
            var result = await _unitOfWork.Login.Login(model);

            // JWT Token mila = Login successful
            if (!string.IsNullOrWhiteSpace(result) && result.StartsWith("eyJ"))
            {
                return Ok(new
                {
                    success = true,
                    token = result
                });
            }

            // Invalid login / unauthorized
            return Unauthorized(new
            {
                success = false,
                message = result
            });
        }


    }
}
