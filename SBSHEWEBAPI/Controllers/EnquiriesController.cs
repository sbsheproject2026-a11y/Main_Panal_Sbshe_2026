using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
using Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnquiriesController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public EnquiriesController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }


        [HttpGet("Enquiries-getall")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _unitOfWork.Enquiries.GetAll(id);

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



        [HttpPost("student-Enquiries")]
        public async Task<IActionResult> CreateStudent(EnquiriesPostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Enquiry data is required."
                    });
                }

                model.TypeId = 1;
                var result = await _unitOfWork.Enquiries.Create(model);

                // ============================================================
                // ✅ EMAIL BHEJO (Exactly same as your Student Creation code)
                // ============================================================

                string emailTo = model.Email;
                string emailName = model.Name ?? "Student";

                // ✅ Subject line sahi kar diya hai
                string subject = "Enquiry Submitted Successfully – Shaheed Bhagat Singh Health & Education";

                string emailBody = $@"
<div style='font-family:Arial,Helvetica,sans-serif;font-size:15px;color:#222;
            line-height:1.7;max-width:600px;margin:0 auto;padding:24px;'>

  <p>Dear <b>{emailName}</b>,</p>

  <p>Your Enquiry has been <b>successfully submitted</b>. ✅</p>

  <p>Our team has received your details and will get back to you shortly.</p>
  
  <div style='background-color:#f9f9f9;padding:15px;border-radius:8px;margin:20px 0;border-left:4px solid #ff6600;'>
      <h4 style='margin-top:0;color:#111;'>Your Enquiry Summary:</h4>
      <ul style='margin:10px 0 0 20px;padding:0;'>
        <li style='margin-bottom:8px;'><b>Name:</b> {model.Name}</li>
        <li style='margin-bottom:8px;'><b>Email:</b> {model.Email}</li>
        <li style='margin-bottom:8px;'><b>Course:</b> {model.Course}</li>
      </ul>
  </div>

  <p>If you face any issues, feel free to contact us at
     <a href='mailto:contact@shaheedbhagatsinghhealthandeducation.com'
        style='color:#1a73e8;text-decoration:none;'>
       contact@shaheedbhagatsinghhealthandeducation.com
     </a>.
  </p>

  <p style='margin-top:24px;'>Regards,<br/>
     <b>Team Shaheed Bhagat Singh Health &amp; Education</b>
  </p>

  <hr style='border:none;border-top:1px solid #e5e5e5;margin:24px 0 12px 0;' />

  <p style='font-size:12px;color:#888;margin:0;'>
    This is an automated email. Please do not reply to this message.
  </p>

</div>";

                _ = Task.Run(async () =>
                {
                    try
                    {
                        Email_sms obj = new Email_sms();
                        await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                    }
                    catch (Exception emailEx)
                    {
                        Console.WriteLine($"Enquiry email failed to {emailTo}: {emailEx.Message}");
                    }
                });

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while creating enquiry.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }



        [HttpPost("franchise-Enquiries")]
        public async Task<IActionResult> CreateFranchise(EnquiriesPostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Enquiry data is required."
                    });
                }

                model.TypeId = 2;
                var result = await _unitOfWork.Enquiries.Create(model);

                // ============================================================
                // ✅ EMAIL BHEJO (Franchise Enquiry)
                // ============================================================

                string emailTo = model.Email;
                string emailName = model.Name ?? "Partner";

                // ✅ Franchise ke liye Subject line
                string subject = "Franchise Enquiry Submitted Successfully – Shaheed Bhagat Singh Health & Education";

                string emailBody = $@"
<div style='font-family:Arial,Helvetica,sans-serif;font-size:15px;color:#222;
            line-height:1.7;max-width:600px;margin:0 auto;padding:24px;'>

  <p>Dear <b>{emailName}</b>,</p>

  <p>Your Franchise Enquiry has been <b>successfully submitted</b>. ✅</p>

  <p>Thank you for your interest in partnering with us. Our team has received your details and will get back to you shortly to discuss the next steps.</p>
  
  <div style='background-color:#f9f9f9;padding:15px;border-radius:8px;margin:20px 0;border-left:4px solid #ff6600;'>
      <h4 style='margin-top:0;color:#111;'>Your Enquiry Summary:</h4>
      <ul style='margin:10px 0 0 20px;padding:0;'>
        <li style='margin-bottom:8px;'><b>Institute Name:</b> {model.Course}</li>
        <li style='margin-bottom:8px;'><b>Name:</b> {model.Name}</li>
        <li style='margin-bottom:8px;'><b>Email:</b> {model.Email}</li>
      </ul>
  </div>

  <p>If you face any issues, feel free to contact us at
     <a href='mailto:contact@shaheedbhagatsinghhealthandeducation.com'
        style='color:#1a73e8;text-decoration:none;'>
       contact@shaheedbhagatsinghhealthandeducation.com
     </a>.
  </p>

  <p style='margin-top:24px;'>Regards,<br/>
     <b>Team Shaheed Bhagat Singh Health &amp; Education</b>
  </p>

  <hr style='border:none;border-top:1px solid #e5e5e5;margin:24px 0 12px 0;' />

  <p style='font-size:12px;color:#888;margin:0;'>
    This is an automated email. Please do not reply to this message.
  </p>

</div>";

                _ = Task.Run(async () =>
                {
                    try
                    {
                        Email_sms obj = new Email_sms();
                        await obj.SendEmailAsync(emailTo, subject, emailBody, isHtml: true);
                    }
                    catch (Exception emailEx)
                    {
                        Console.WriteLine($"Franchise Enquiry email failed to {emailTo}: {emailEx.Message}");
                    }
                });

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while creating enquiry.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }

        [HttpDelete("Enquiries-delete/{id}")]
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

            var result = await _unitOfWork.Enquiries.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }



        [HttpGet("rtidata-getall")]
        public async Task<IActionResult> GetRTIAll()
        {
            try
            {
                var data = await _unitOfWork.Enquiries.GetRTIAll();

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

        [HttpPost("rti-apply")]
        public async Task<IActionResult> CreateRTI([FromForm] RTIApplicationsDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "RTI data is required."
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

                if (model.Path != null && model.Path.Length > 0)
                    model.FilePath = await UploadFile(model.Path);

                if (model.Path1 != null && model.Path1.Length > 0)
                    model.FilePath1 = await UploadFile(model.Path1);


                var result = await _unitOfWork.Enquiries.RTICreate(model);

                return Ok(new
                {
                    success = true,
                    message = result,
                    trackingId = model.TrackingId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while creating RTI application.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }


        [HttpPost("update-rti-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateRTIStatusDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid request. RTI Id required."
                    });
                }

                if (model.StatusId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Status is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.Remarks))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Remark is required."
                    });
                }

                var result = await _unitOfWork.Enquiries.UpdateRTIStatus(model);

                return Ok(new
                {
                    success = true,
                    message = "RTI status updated successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Error while updating RTI status.",
                        error = ex.Message,
                        innerError = ex.InnerException?.Message
                    }
                );
            }
        }

        [HttpDelete("rti-delete/{id}")]
        public async Task<IActionResult> DeleteRTI(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Enquiries.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }


        [HttpGet("rtidata-get")]
        public async Task<IActionResult> GetRTIAll(string id)
        {
            try
            {
                var data = await _unitOfWork.Enquiries.GetRTIByTraickId(id);

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

    }
}
