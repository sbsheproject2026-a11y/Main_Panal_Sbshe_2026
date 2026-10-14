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


        [HttpGet("frenchise-list/{id}")]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _unitOfWork.WebsiteContent.GetAll(id);

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

        [HttpGet("website-content/{id}")]
        public async Task<IActionResult> GetMaster(int id)
        {
            var data = await _unitOfWork.WebsiteContent.GetWebsiteMaster(id);

            if (data == null || !data.Any())

                return NotFound("data not found");

            return Ok(data);
        }

        [HttpGet("website-course/{id:int}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var data = await _unitOfWork.WebsiteContent.GetCourseData(id);

            if (data == null || !data.Any())
                return NotFound("Data not found");

            return Ok(data);
        }


    }
}

