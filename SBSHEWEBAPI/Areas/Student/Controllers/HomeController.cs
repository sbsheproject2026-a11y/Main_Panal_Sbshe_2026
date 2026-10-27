using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Student.Controllers
{
    [Area("Student")]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;

        public HomeController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
            _appEnvironment = appEnvironment;
        }


        [HttpGet("student-profile/{id}")]
        public async Task<IActionResult> StudentDetails(int id)
        {
            var data = await _unitOfWork.Student.GetStudentProfileData(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPost("applyforbookbystudent")]
        public async Task<IActionResult> ApplyForBooks([FromForm] ApplyStudentPostDTOs model)
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


                if (!model.UserId.HasValue || model.UserId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "UserId is required."
                    });
                }
                if (!model.Amount.HasValue || model.Amount.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Amount is required."
                    });
                }

                model.Quantity = 1;
                var resultaddress = (await _unitOfWork.Student.CreateBookDeliveryAddress(model));
                var result = (await _unitOfWork.GetWayWorking.GenrateOrderBook((int)model.UserId, Convert.ToDecimal(model.Amount), (int)model.CourseId, (int)model.Quantity)).Split(",");
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
        [HttpPost("pay-student-pending-amount")]
        public async Task<IActionResult> PayPendingAmount([FromForm] ApplyStudentPostDTOs model)
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


                if (!model.UserId.HasValue || model.UserId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "UserId is required."
                    });
                }
                if (!model.Amount.HasValue || model.Amount.Value <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Amount is required."
                    });
                }

                model.Quantity = 1;


                var result = (await _unitOfWork.GetWayWorking.GenrateOrder((int)model.UserId, Convert.ToDecimal(model.Amount), (int)model.CourseId, (int)model.Quantity)).Split(",");
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

