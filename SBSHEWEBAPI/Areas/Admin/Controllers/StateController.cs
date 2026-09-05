using Application;
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
    public class StateController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public StateController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpGet("state-getall")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _unitOfWork.Location.GetAll(1);

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

        [HttpPost("state-create")]
        public async Task<IActionResult> Create(LocationPostDTOs model)
        {
            model.TypeId = 1;
            var result = await _unitOfWork.Location.Create(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }


        [HttpGet("state-getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Location.GetById(id);

            if (data == null)
            {
                return NotFound("MasterType not found");
            }

            return Ok(data);
        }

        [HttpPut("state-update")]
        public async Task<IActionResult> Update(LocationPostDTOs model)
        {
            if (model.Id <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Id is required"
                });
            }
            model.TypeId = 1;
            var result = await _unitOfWork.Location.Update(model);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }
        [HttpDelete("state-delete/{id}")]
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

            var result = await _unitOfWork.Location.Delete(Id);

            return Ok(new
            {
                Success = true,
                Message = result
            });
        }

        public class ExcelUploadRequest
        {
            public IFormFile File { get; set; }
        }
        [HttpPost("bulk-upload")]

        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Please upload an Excel file.");

                // EPPlus License
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                DataTable dt = new DataTable();

                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("ParentId", typeof(int));
                dt.Columns.Add("TypeId", typeof(int));
                dt.Columns.Add("IsActive", typeof(int));

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        if (worksheet.Dimension == null)
                            return BadRequest("Excel file is empty.");

                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            DataRow dr = dt.NewRow();

                            // Code (Nullable)
                            string code = worksheet.Cells[row, 1].Text.Trim();
                            dr["Code"] = string.IsNullOrWhiteSpace(code)
                                ? DBNull.Value
                                : code;

                            // Name
                            string name = worksheet.Cells[row, 2].Text.Trim();

                            if (string.IsNullOrWhiteSpace(name))
                                continue; // Skip row if Name is empty

                            dr["Name"] = name;

                            // ParentId (Nullable)
                            string parentId = worksheet.Cells[row, 3].Text.Trim();

                            if (int.TryParse(parentId, out int pid))
                                dr["ParentId"] = pid;
                            else
                                dr["ParentId"] = DBNull.Value;

                            // TypeId
                            string typeId = worksheet.Cells[row, 4].Text.Trim();

                            if (int.TryParse(typeId, out int tid))
                                dr["TypeId"] = tid;
                            else
                                dr["TypeId"] = 0;

                            // IsActive
                            string isActive = worksheet.Cells[row, 5].Text.Trim();

                            if (int.TryParse(isActive, out int active))
                                dr["IsActive"] = active;
                            else
                                dr["IsActive"] = 1;

                            dt.Rows.Add(dr);
                        }
                    }
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await con.OpenAsync();

                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                    {
                        bulk.DestinationTableName = "dbo.Location";
                        bulk.BatchSize = 5000;
                        bulk.BulkCopyTimeout = 0;

                        bulk.ColumnMappings.Add("Code", "Code");
                        bulk.ColumnMappings.Add("Name", "Name");
                        bulk.ColumnMappings.Add("ParentId", "ParentId");
                        bulk.ColumnMappings.Add("TypeId", "TypeId");
                        bulk.ColumnMappings.Add("IsActive", "IsActive");

                        await bulk.WriteToServerAsync(dt);
                    }
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Bulk Upload Successful",
                    TotalRows = dt.Rows.Count
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

