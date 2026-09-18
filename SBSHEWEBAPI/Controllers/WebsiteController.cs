using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public WebsiteController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        [HttpGet("frenchise-list")]

        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.WebsiteContent.GetAll();

                var result = data.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    contactpersonname = x.FatherName,
                    address = x.Address,
                    cityName = x.CityName,
                    districtName = x.DistrictName,
                    stateName = x.StateName,
                    mobileNo = x.MobileNo,
                    email = x.Email,
                    pincode = x.Pincode,
                     code = x.Code
                }).ToList();

                return Ok(new
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }




    }
}

