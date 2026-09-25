using Application.DTOs;
using Application.Helper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SBSHEWEBAPI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentApprovalController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public StudentApprovalController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

       
        
        [Authorize]
        [HttpGet("student-senttoconfirm")]

        public async Task<IActionResult> GetSendToConfirm(int pageNo = 1, int pageSize = 100, string? search = null)
        {
            try
            {
                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                var userId = claims.GetValueOrDefault("UserId");
             
                var data = await _unitOfWork.Student.GetSendToConfirm(Convert.ToInt32(userId), pageNo, pageSize, search);

                return Ok(new
                {
                    Success = true,
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TotalRecords = data.Count,
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


        [Authorize]

        [HttpPut("SendToRequest")]
        public async Task<IActionResult> SendToRequest([FromBody] List<SendToConfirmDTO> students)
        {
            var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
            var userId = claims.GetValueOrDefault("UserId");

            var buser = 1;
            string result = "";

            if (students == null || students.Count == 0)
            {
                return BadRequest("Please provide at least one student.");
            }

            // Random 1-digit code
            var chars2 = "1234567890";
            var stringChars6 = new char[5];
            var random6 = new Random();

            for (int i = 0; i < stringChars6.Length; i++)
            {
                stringChars6[i] = chars2[random6.Next(chars2.Length)];
            }
            var unic6 = new string(stringChars6);

            var results = new List<object>();

            foreach (var item in students)
            {
                int studentId = item.Id;
                int courseId = item.CourseId;
                int amount = item.Amount;

                // ✅ Wallet balance check
                string ss = await _unitOfWork.Student.viewbalview(  Convert.ToInt32(userId), amount);

                if (ss == "ok")
                {
                    // ✅ Update student
                    string spMessage = await _unitOfWork.Student.insSendForConfirm( studentId, Convert.ToInt32(userId), Convert.ToInt32(buser),  amount,
                        unic6
                    );

                    result = spMessage;
                }
                else
                {
                    result = "Wallet Balance is Low.!";
                }

                results.Add(new
                {
                    id = studentId,
                    courseId = courseId,
                    amount = amount,
                    message = result
                });
            }

            return Ok(new
            {
                success = true,
                message = "Students processed successfully.",
                data = results
            });
        }

        //public IActionResult SendToRequest(string[] id)
        //{

        //    var user = Convert.ToInt32((HttpContext.Session.GetString("User")));
        //    var buser = 1;
        //    string str = "";
        //    string Result = "";
        //    int sumTotal1 = 0;
        //    string CourseId = "";
        //    int sumTotal = 0;

        //    var chars2 = "1234567890";
        //    var stringChars6 = new char[1];
        //    var random6 = new Random();

        //    for (int i = 0; i < stringChars6.Length; i++)
        //    {
        //        stringChars6[i] = chars2[random6.Next(chars2.Length)];
        //    }
        //    var unic6 = new String(stringChars6);



        //    foreach (var i in id)
        //    {
        //        string[] a = i.Split(",");
        //        if (a[0] != null && a[0] != "0" && a[1] != null && a[1] != "0" && a[5] != null && a[5] != "0")
        //        {


        //            sumTotal = Convert.ToInt32(a[5]);
        //            Result += "insert TransFundProductDetail(LedgerId,ProductId,Quntity,Amount,TransType,Remark,demoId) values(" + Convert.ToInt32(a[0]) + "," + Convert.ToInt32(a[1]) + ",1," + sumTotal + ",'DR','Certificate Fee'," + unic6 + "); ";

        //            Result += "update ProductUpgrade set CertificateStatus=2 where Id=" + a[4] + "; ";
        //            Result += "update Entity set Status=10643 where Id=" + a[0] + "; ";

        //            string ss = _procAdo.viewbalview(Convert.ToInt32(user), sumTotal);
        //            if (ss == "ok")
        //            {
        //                str = _procAdo.insSendForConfirm(Convert.ToInt32(user), Convert.ToInt32(buser), sumTotal, Result, unic6);
        //                ViewBag.dataList = str;
        //            }
        //            else
        //            {
        //                HttpContext.Session.SetString("SendToMsg", "Wallet Balance is Low.!");
        //                //return View("Wallet Balance is Low!");
        //            }
        //        }
        //    }



        //    return View(str);
        //}



    }
}

