using Application.DTOs.MasterTypeDTOs;
using Application.Helper;
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
        public async Task<IActionResult> CreateRTI([FromForm]RTIApplicationsDTOs model)
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
