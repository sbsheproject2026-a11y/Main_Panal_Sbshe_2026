using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public CourseController(IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }


        [HttpGet("course-getall")]
        public async Task<IActionResult> Get(
    [FromQuery] int? studyModeId,
    [FromQuery] int? departmentId,
    [FromQuery] int? categoryId,
    [FromQuery] int? parentId,
    [FromQuery] string? search)
        {
            try
            {
                // ✅ Repository ko filter parameters pass kiye
                var data = await _unitOfWork.Course.GetAll(studyModeId, departmentId, categoryId, parentId, search);

                return Ok(new
                {
                    Success = true,
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
        [HttpPost("course-create")]
        public async Task<IActionResult> Create([FromForm] CoursePostDTOs model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "data is required"
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
                if (model.ProductCategoryDocuments != null && model.ProductCategoryDocuments.Count > 0)
                {
                    foreach (var academic in model.ProductCategoryDocuments)
                    {
                        if (academic.Fileupload1 != null)
                        {

                            if (academic.Fileupload1 != null &&
                                academic.Fileupload1.Length > 0)
                            {
                                academic.Fileupload = await UploadFile(academic.Fileupload1);
                            }
                        }
                    }
                }


                model.ParentId = model.ParentId == 0 ? null : model.ParentId;

                var result = await _unitOfWork.Course.Create(model);

                return Ok(new
                {
                    success = true,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }


        [HttpGet("course-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Course.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("course-update")]
        public async Task<IActionResult> Update(CoursePostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
            model.ParentId = model.ParentId == 0 ? null : model.ParentId;
            var result = await _unitOfWork.Course.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        [HttpDelete("course-delete/{id}")]
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

            var result = await _unitOfWork.Course.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }




        [HttpGet("course-productamount-get/{id}")]
        public async Task<IActionResult> GetProductAmount(int id)
        {
            try
            {
                var data = await _unitOfWork.Course.GetProductAmount(id);

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


        [HttpPost("CreateCourseAmount")]
        public async Task<IActionResult> CreateCourseAmount([FromBody] List<ProductAmountDtos> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return BadRequest("At least one Amount is required");
                }


                var result = await _unitOfWork.Course.CreateCourseAmount(model);

                if (result.Contains("Something went wrong"))
                {
                    return StatusCode(500, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }


        [HttpGet("course-paymentamount-getbyid/{id}")]
        public async Task<IActionResult> GetProductAmountById(int id)
        {
            var data = await _unitOfWork.Course.GetProductAmountById(id);

            if (data == null)
            {
                return NotFound("Lecture not found");
            }

            return Ok(data);
        }

        [HttpPut("course-paymentamount-update/{id}")]
        public async Task<IActionResult> UpdateProductAmount(int id, [FromBody] ProductAmountDtos model)
        {
            model.Id = id;
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Course.UpdateProductAmount(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }



        [HttpDelete("course-paymentamount-delete/{id}")]
        public async Task<IActionResult> DeleteProductAmount(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Course.DeleteProductAmount(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        [HttpDelete("course-material-delete/{id}")]
        public async Task<IActionResult> DeleteCourseMaterial(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }

            var result = await _unitOfWork.Course.DeleteCourseMaterial(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }





        [HttpGet("course-material-get/{id}")]
        public async Task<IActionResult> GetCourseMaterial(int id)
        {
            try
            {
                var data = await _unitOfWork.Course.getCourseMaterial(id);

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
        [HttpGet("course-update-material-get/{id}")]
        public async Task<IActionResult> getEditCourseMaterial(int id)
        {
            try
            {
                var data = await _unitOfWork.Course.getEditCourseMaterial(id);

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


        [HttpPost("create-course-material")]

        public async Task<IActionResult> CreateCourseMaterial([FromForm] ProductMaterialDtos model)
        {
            try
            {
                if (model == null || model.GetType().GetProperties().All(p => p.GetValue(model) == null))
                {
                    return BadRequest("Course Material is required");
                }

                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "Uploads"
                );

                Directory.CreateDirectory(folderPath);

                // =========================
                // FILE UPLOAD
                // =========================

                if (model.Fileupload1 != null &&
                    model.Fileupload1.Length > 0)
                {
                    string fileName =
                        $"{Guid.NewGuid():N}"[..8] +
                        "_" +
                        Path.GetFileName(model.Fileupload1.FileName);

                    string filePath = Path.Combine(
                        folderPath,
                        fileName
                    );

                    await using (var stream = new FileStream(
                        filePath,
                        FileMode.Create
                    ))
                    {
                        await model.Fileupload1.CopyToAsync(stream);
                    }

                    model.Fileupload = fileName;
                }

                // =========================
                // DATABASE SAVE
                // =========================

                var result = await _unitOfWork.Course.CreateCourseMaterial(model);

                if (result.Contains("Something went wrong"))
                {
                    return StatusCode(500, result);
                }

                return Ok(new
                {
                    success = true,
                    message = "Course Material created successfully",
                    fileName = model.Fileupload
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Something went wrong: " + ex.Message
                );
            }
        }
        [HttpPut("Update-course-material")]

        public async Task<IActionResult> UpdateCourseMaterial([FromForm] ProductMaterialDtos model)
        {
            try
            {
                if (model == null || model.GetType().GetProperties().All(p => p.GetValue(model) == null))
                {
                    return BadRequest("Course Material is required");
                }

                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "Uploads"
                );

                Directory.CreateDirectory(folderPath);

                // =========================
                // FILE UPLOAD
                // =========================

                if (model.Fileupload1 != null && model.Fileupload1.Length > 0)
                {
                    // ✅ 1. Purani file ka naam ek variable me save karein (Delete karne ke liye)
                    string oldFile = model.Fileupload;

                    // 2. Nayi file ka unique naam banayein
                    string fileName =
                        $"{Guid.NewGuid():N}"[..8] +
                        "_" +
                        Path.GetFileName(model.Fileupload1.FileName);

                    string filePath = Path.Combine(
                        folderPath,
                        fileName
                    );

                    // 3. PEHLE Nayi file ko server par save karein
                    await using (var stream = new FileStream(
                        filePath,
                        FileMode.Create
                    ))
                    {
                        await model.Fileupload1.CopyToAsync(stream);
                    }

                    // 4. Model me nayi file ka naam set karein
                    model.Fileupload = fileName;

                    // ✅ 5. Nayi file save hone ke BAAD, purani file ko server se delete karein
                    if (!string.IsNullOrEmpty(oldFile))
                    {
                        string oldPath = Path.Combine(folderPath, oldFile);

                        if (System.IO.File.Exists(oldPath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldPath); // Purani file delete
                            }
                            catch
                            {
                                // Agar file kisi process me open ho aur delete na ho paye, toh error ignore karein
                            }
                        }
                    }
                }

                // =========================
                // DATABASE SAVE
                // =========================

                var result = await _unitOfWork.Course.CreateCourseMaterial(model);

                if (result.Contains("Something went wrong"))
                {
                    return StatusCode(500, result);
                }

                return Ok(new
                {
                    success = true,
                    message = "Course Material created successfully",
                    fileName = model.Fileupload
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Something went wrong: " + ex.Message
                );
            }
        }


        [HttpPost("set-franchise-commission-amount")]
        public async Task<IActionResult> SetFramchiseAmount([FromBody] List<CommissionDtos> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return BadRequest("At least one Amount is required");
                }

                // ✅ 1-1-1 karke repo ko bhejo
                var results = new List<string>();

                foreach (var item in model)
                {
                    // Validation
                    if (!item.Amount.HasValue || !item.FranchiseId.HasValue ||
                        !item.DepartmentId.HasValue || !item.ProductId.HasValue)
                    {
                        results.Add($"Skipped — Amount/FranchiseId/DepartmentId/ProductId required");
                        continue;
                    }

                    // ✅ Har item alag se service ko bhejo
                    var result = await _unitOfWork.Course.SetFranchiseAmount(item);

                    results.Add(result);
                }

                return Ok(new
                {
                    Success = true,
                    Total = model.Count,
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpPost("set-course-commission")]
        public async Task<IActionResult> SetCourseCommissionAmount([FromBody] List<CommissionDtos> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return BadRequest("At least one Amount is required");
                }

                // ✅ 1-1-1 karke repo ko bhejo
                var results = new List<string>();

                foreach (var item in model)
                {
                    // Validation
                    if (!item.Amount.HasValue ||
                        !item.DepartmentId.HasValue || !item.ProductId.HasValue)
                    {
                        results.Add($"Skipped — Amount/DepartmentId/ProductId required");
                        continue;
                    }

                    item.FranchiseId = null;
                    var result = await _unitOfWork.Course.SetCourseCommission(item);

                    results.Add(result);
                }

                return Ok(new
                {
                    Success = true,
                    Total = model.Count,
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }


        [HttpGet("course-getstudent-details-all/{id}")]
        public async Task<IActionResult> GetStudentCourseDetailById(int id)
        {
            var data = await _unitOfWork.Course.GetStudentCourseDetailById(id);

            if (data == null)
            {
                return NotFound("data not found");
            }

            return Ok(data);
        }



        //[HttpGet("course-course-material-getbyid/{id}")]
        //public async Task<IActionResult> GetCourseMaterialById(int id)
        //{
        //    var data = await _unitOfWork.Course.getEditCourseMaterial(id);

        //    if (data == null)
        //    {
        //        return NotFound("Lecture not found");
        //    }

        //    return Ok(data);
        //}






        //----------------------------Course Drop Down All--------------------

    }
}

