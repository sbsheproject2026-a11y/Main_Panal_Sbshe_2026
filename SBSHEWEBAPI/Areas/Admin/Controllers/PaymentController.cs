using Application;
using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpGet("get-payment-detail")]
        public async Task<IActionResult> Get(string status)
        {
            try
            {
                var data = await _unitOfWork.GetWayWorking.offlinepayment(status);

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
       
        [HttpGet("get-books-payment-detail")]
        public async Task<IActionResult> offlinepaymentBook(string status)
        {
            try
            {
                var data = await _unitOfWork.GetWayWorking.offlinepaymentBook(status);

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
       
        
        [HttpGet("student-payment-detail")]
        public async Task<IActionResult> GetStudentFee(int Id)
        {
            try
            {
                var data = await _unitOfWork.GetWayWorking.GetStudentFee(Id);

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


      
        [HttpPost("student-payment-add")]
        public async Task<IActionResult> CreateOfflinePayment([FromForm] OfflinePaymentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request data is required."
                    });
                }

                var result = (await _unitOfWork.GetWayWorking.OfflinePayPayment(request)).Split(",");

                var msg = result[0];
                var entityId = result[1];
                var oId = result[2];

              

                return Ok(new
                {
                    success = true,
                    message = msg,
                    userid = entityId,
                    paymentid = oId
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

