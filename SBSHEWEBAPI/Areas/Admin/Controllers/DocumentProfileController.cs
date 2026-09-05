using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using QRCoder;
using System.Data;
 

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentProfileController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public DocumentProfileController(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _appEnvironment = appEnvironment;
        }

        private byte[]? Getimage(string filepath)
        {
            try
            {
                if (!System.IO.File.Exists(filepath))
                    return null;

                return System.IO.File.ReadAllBytes(filepath);
            }
            catch
            {
                return null;
            }
        }





        [HttpGet("document-print-marksheet-print/{id}")]
        public async Task<IActionResult> MarksheetPrint(string id)
        {
            try
            {
                // =========================================================
                // 1. GET DIPLOMA DATA
                // =========================================================

                var data = await _unitOfWork.DocumentProfile.GetMarksheetPrintData(id);
                var subject = await _unitOfWork.DocumentProfile.PrintStudentSubjectMarks(id);

                if (data == null)
                {
                    return NotFound(new
                    {
                        message = $"Diploma data not found for ID: {id}"
                    });
                }


                // =========================================================
                // 2. STUDENT IMAGE -> BASE64
                // =========================================================

                string selfImageBase64 = "";

                try
                {
                    if (!string.IsNullOrWhiteSpace(data.SelfImage))
                    {
                        // -------------------------------------------------
                        // Only filename rakhenge
                        // -------------------------------------------------

                        string fileName =
                            Path.GetFileName(data.SelfImage);

                        // -------------------------------------------------
                        // Physical file path
                        // wwwroot/Uploads/filename.jpg
                        // -------------------------------------------------

                        string imagePath = Path.Combine(
                            _appEnvironment.WebRootPath,
                            "Uploads",
                            fileName
                        );

                        Console.WriteLine(
                            "Student Image Path: " +
                            imagePath
                        );


                        // -------------------------------------------------
                        // File exist check
                        // -------------------------------------------------

                        if (System.IO.File.Exists(imagePath))
                        {
                            byte[] imageBytes =
                                await System.IO.File.ReadAllBytesAsync(
                                    imagePath
                                );


                            // -------------------------------------------------
                            // Detect extension
                            // -------------------------------------------------

                            string extension =
                                Path.GetExtension(imagePath)
                                    .ToLowerInvariant();


                            string mimeType = extension switch
                            {
                                ".jpg" => "image/jpeg",
                                ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",

                                _ => "application/octet-stream"
                            };


                            // -------------------------------------------------
                            // BYTE[] -> BASE64
                            // -------------------------------------------------

                            selfImageBase64 =
                                $"data:{mimeType};base64," +
                                Convert.ToBase64String(
                                    imageBytes
                                );


                            Console.WriteLine(
                                "Student Image Base64 generated."
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                "Student Image File NOT FOUND: " +
                                imagePath
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "data.SelfImage is empty."
                        );
                    }
                }
                catch (Exception imageEx)
                {
                    Console.WriteLine(
                        "Student Image Error: " +
                        imageEx.Message
                    );

                    selfImageBase64 = "";
                }


                // =========================================================
                // 3. GENERATE QR CODE -> BASE64
                // =========================================================

                string qrImage = "";

                try
                {
                    // -------------------------------------------------
                    // QR DATA
                    // -------------------------------------------------

                    string qrText =
                        "Name-" + (data.Name ?? "") +
                        ",FatherName-" + (data.FatherName ?? "") +
                        ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                        ",Course-" + (data.CourseName ?? "");


                    // -------------------------------------------------
                    // QR GENERATOR
                    // -------------------------------------------------

                    using (QRCodeGenerator qrGenerator =
                           new QRCodeGenerator())
                    {
                        using (QRCodeData qrCodeData =
                               qrGenerator.CreateQrCode(
                                   qrText,
                                   QRCodeGenerator.ECCLevel.Q))
                        {
                            // -------------------------------------------------
                            // Generate PNG bytes
                            // -------------------------------------------------

                            var qrCode =
                                new PngByteQRCode(
                                    qrCodeData
                                );

                            byte[] qrBytes =
                                qrCode.GetGraphic(20);


                            // -------------------------------------------------
                            // PNG bytes -> Base64
                            // -------------------------------------------------

                            qrImage =
                                "data:image/png;base64," +
                                Convert.ToBase64String(
                                    qrBytes
                                );
                        }
                    }

                    Console.WriteLine(
                        "QR Base64 generated successfully."
                    );
                }
                catch (Exception qrEx)
                {
                    Console.WriteLine(
                        "QR Generation Error: " +
                        qrEx.Message
                    );

                    qrImage = "";
                }


                // =========================================================
                // 4. API RESPONSE
                // =========================================================

                return Ok(new
                {
                    name = data.Name ?? "",
                    fatherName = data.FatherName ?? "",
                    motherName = data.MotherName ?? "",
                    enrollmentNo = data.EnrollmentNo ?? "",

                    rollno = data.Rollno ?? "",
                    dob = data.DOB1 ?? "",
                    session = data.Session ?? "",
                    studentYear = data.StudentYear ?? "",
                    centreName = data.CentreName ?? "",

                    srnoDiploma = data.SrnoDiploma ?? "",

                    issueDate = data.IssueDate ?? "",

                    issueYear = data.IssueYear ?? "",

                    courseName = data.CourseName ?? "",

                    departmentName = data.DepartmentName ?? "",
                    maxMarks = data.MaxMarks ?? null,
                    theoryMarks = data.TheoryMarks ?? null,
                    practicalMarks = data.PracticalMarks ?? null,
                    assignmentMarks = data.AssignmentMarks ?? null,
                    totalMarks = data.TotalMarks ?? null,
                    grade = data.Grade ?? "",
                    Result = data.Result ?? "",

                    // =====================================================
                    // Subject Print
                    // =====================================================

                    subjects = subject,




                    // =====================================================
                    // IMPORTANT
                    // =====================================================
                    // Yahan data.SelfImage nahi lagana hai.
                    // selfImageBase64 lagana hai.
                    // =====================================================

                    selfImage =
                        selfImageBase64,


                    // =====================================================
                    // QR BASE64
                    // =====================================================

                    qrimage =
                        qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Diploma data fetch failed.",

                    error =
                        ex.Message,

                    innerError =
                        ex.InnerException?.Message
                });
            }
        }
        [HttpGet("document-print-admitcard-print/{id}")]
        public async Task<IActionResult> AdmitCardPrint(string id)
        {
            try
            {
                // =========================================================
                // 1. GET DIPLOMA DATA
                // =========================================================

                var data = await _unitOfWork.DocumentProfile.GetMarksheetPrintData(id);
                var subject = await _unitOfWork.DocumentProfile.PrintStudentSubjectAdmitCard(id);

                if (data == null)
                {
                    return NotFound(new
                    {
                        message = $"Diploma data not found for ID: {id}"
                    });
                }


                // =========================================================
                // 2. STUDENT IMAGE -> BASE64
                // =========================================================

                string selfImageBase64 = "";

                try
                {
                    if (!string.IsNullOrWhiteSpace(data.SelfImage))
                    {
                        // -------------------------------------------------
                        // Only filename rakhenge
                        // -------------------------------------------------

                        string fileName =
                            Path.GetFileName(data.SelfImage);

                        // -------------------------------------------------
                        // Physical file path
                        // wwwroot/Uploads/filename.jpg
                        // -------------------------------------------------

                        string imagePath = Path.Combine(
                            _appEnvironment.WebRootPath,
                            "Uploads",
                            fileName
                        );

                        Console.WriteLine(
                            "Student Image Path: " +
                            imagePath
                        );


                        // -------------------------------------------------
                        // File exist check
                        // -------------------------------------------------

                        if (System.IO.File.Exists(imagePath))
                        {
                            byte[] imageBytes =
                                await System.IO.File.ReadAllBytesAsync(
                                    imagePath
                                );


                            // -------------------------------------------------
                            // Detect extension
                            // -------------------------------------------------

                            string extension =
                                Path.GetExtension(imagePath)
                                    .ToLowerInvariant();


                            string mimeType = extension switch
                            {
                                ".jpg" => "image/jpeg",
                                ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",

                                _ => "application/octet-stream"
                            };


                            // -------------------------------------------------
                            // BYTE[] -> BASE64
                            // -------------------------------------------------

                            selfImageBase64 =
                                $"data:{mimeType};base64," +
                                Convert.ToBase64String(
                                    imageBytes
                                );


                            Console.WriteLine(
                                "Student Image Base64 generated."
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                "Student Image File NOT FOUND: " +
                                imagePath
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "data.SelfImage is empty."
                        );
                    }
                }
                catch (Exception imageEx)
                {
                    Console.WriteLine(
                        "Student Image Error: " +
                        imageEx.Message
                    );

                    selfImageBase64 = "";
                }
                 

                // =========================================================
                // 3. API RESPONSE
                // =========================================================

                return Ok(new
                {
                    name = data.Name ?? "",
                    fatherName = data.FatherName ?? "",
                    
                    enrollmentNo = data.EnrollmentNo ?? "",

                    rollno = data.Rollno ?? "",
                    
                    session = data.Session ?? "",
                   
                    centreName = data.CentreName ?? "",

                    courseName = data.CourseName ?? "",
                     
                    // =====================================================
                    // Subject Print
                    // =====================================================

                    subjects = subject,




                    // =====================================================
                    // IMPORTANT
                    // =====================================================
                    // Yahan data.SelfImage nahi lagana hai.
                    // selfImageBase64 lagana hai.
                    // =====================================================

                    selfImage = selfImageBase64 


                     
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Diploma data fetch failed.",

                    error =
                        ex.Message,

                    innerError =
                        ex.InnerException?.Message
                });
            }
        }



        [HttpGet("diploma-print/{id}")]
        public async Task<IActionResult> DiplomaPrint(int id)
        {
            try
            {
                var data = await _unitOfWork.DocumentProfile
                    .GetDiplomaPrintData(id);

                if (data == null)
                {
                    return NotFound(new
                    {
                        message = $"Diploma data not found for ID: {id}"
                    });
                }

                string selfImageBase64 = "";

                try
                {
                    if (!string.IsNullOrWhiteSpace(data.SelfImage))
                    {
                        string fileName =
                            Path.GetFileName(data.SelfImage);

                        string imagePath = Path.Combine(
                            _appEnvironment.WebRootPath,
                            "Uploads",
                            fileName
                        );

                        Console.WriteLine(
                            "Student Image Path: " +
                            imagePath
                        );

                        if (System.IO.File.Exists(imagePath))
                        {
                            byte[] imageBytes =
                                await System.IO.File.ReadAllBytesAsync(
                                    imagePath
                                );

                            string extension =
                                Path.GetExtension(imagePath)
                                    .ToLowerInvariant();

                            string mimeType = extension switch
                            {
                                ".jpg" => "image/jpeg",
                                ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",
                                _ => "application/octet-stream"
                            };

                            selfImageBase64 =
                                $"data:{mimeType};base64," +
                                Convert.ToBase64String(
                                    imageBytes
                                );

                            Console.WriteLine(
                                "Student Image Base64 generated."
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                "Student Image File NOT FOUND: " +
                                imagePath
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "data.SelfImage is empty."
                        );
                    }
                }
                catch (Exception imageEx)
                {
                    Console.WriteLine(
                        "Student Image Error: " +
                        imageEx.Message
                    );

                    selfImageBase64 = "";
                }

                string qrImage = "";

                try
                {
                    string qrText =
                        "Name-" + (data.Name ?? "") +
                        ",FatherName-" + (data.FatherName ?? "") +
                        ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                        ",Course-" + (data.CourseName ?? "");

                    using (QRCodeGenerator qrGenerator =
                           new QRCodeGenerator())
                    using (QRCodeData qrCodeData =
                           qrGenerator.CreateQrCode(
                               qrText,
                               QRCodeGenerator.ECCLevel.Q))
                    {
                        var qrCode =
                            new PngByteQRCode(
                                qrCodeData
                            );

                        byte[] qrBytes =
                            qrCode.GetGraphic(
                                20,
                                System.Drawing.Color.Black,
                                System.Drawing.Color.Transparent,
                                false
                            );

                        qrImage =
                            "data:image/png;base64," +
                            Convert.ToBase64String(
                                qrBytes
                            );

                        Console.WriteLine(
                            "QR Base64 generated successfully."
                        );
                    }
                }
                catch (Exception qrEx)
                {
                    Console.WriteLine(
                        "QR Generation Error: " +
                        qrEx.Message
                    );

                    qrImage = "";
                }

                return Ok(new
                {
                    name = data.Name ?? "",
                    firstName = data.FirstName ?? "",

                    studentNameHindi =
                        data.StudentNameHindi ?? "",

                    fatherName =
                        data.FatherName ?? "",

                    fatherNameHindi =
                        data.FatherNameHindi ?? "",

                    motherName =
                        data.MotherName ?? "",

                    courseNameHindi =
                        data.CourseNameHindi ?? "",

                    enrollmentNo =
                        data.EnrollmentNo ?? "",

                    rollno =
                        data.Rollno ?? "",

                    srnoDiploma =
                        data.SrnoDiploma ?? "",

                    issueDate =
                        data.IssueDate ?? "",

                    startDate =
                        data.StartDate ?? "",

                    endDate =
                        data.EndDate ?? "",

                    dOB1 =
                        data.DOB1 ?? "",

                    address =
                        data.Address ?? "",

                    issueYear =
                        data.IssueYear ?? "",

                    courseName =
                        data.CourseName ?? "",

                    departmentName =
                        data.DepartmentName ?? "",

                    certificateName =
                        data.CertificateName ?? "",

                    migrationSrno =
                        data.MigrationSrno ?? "",

                    centreName =
                        data.CentreName ?? "",

                    selfImage =
                        selfImageBase64,

                    qrimage =
                        qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Diploma data fetch failed.",

                        error =
                            ex.Message,

                        innerError =
                            ex.InnerException?.Message
                    }
                );
            }
        }




        [HttpGet("diploma-print-new/{id:int}")]
        public async Task<IActionResult> DiplomaPrintNew(int id)
        {
            try
            {
                // ============================================================
                // 1. Get diploma data
                // ============================================================
                var dt = await DiplomaPrintData(id);

                if (dt == null)
                {
                    return NotFound(new
                    {
                        message = "Diploma data could not be loaded.",
                        id = id
                    });
                }

                if (dt.Rows.Count == 0)
                {
                    return NotFound(new
                    {
                        message = $"Diploma data not found for ID: {id}",
                        id = id
                    });
                }

                // ============================================================
                // 2. RDLC file path
                // ============================================================
                var webRoot = _appEnvironment.WebRootPath;

                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot"
                    );
                }

                var reportPath = Path.Combine(
                    webRoot,
                    "Reports",
                    "Diploma_Print.rdlc"
                );

                // ============================================================
                // 3. Check RDLC exists
                // ============================================================
                if (!System.IO.File.Exists(reportPath))
                {
                    return StatusCode(500, new
                    {
                        message = "Diploma_Print.rdlc not found.",
                        path = reportPath
                    });
                }

                // ============================================================
                // 4. Create LocalReport
                // ============================================================
                using var report = new Microsoft.Reporting.NETCore.LocalReport();

                await using (var reportStream = new FileStream(
                    reportPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read))
                {
                    report.LoadReportDefinition(reportStream);
                }

                // ============================================================
                // 5. Add dataset
                // ============================================================
                report.DataSources.Clear();

                report.DataSources.Add(
                    new ReportDataSource(
                        "DataSet1",
                        dt
                    )
                );

                // ============================================================
                // 6. Report parameters
                // ============================================================
                try
                {
                    var parameters = new List<ReportParameter>
            {
                new ReportParameter(
                    "check",
                    "Hello"
                )
            };

                    report.SetParameters(parameters);
                }
                catch (Exception parameterEx)
                {
                    Console.WriteLine(
                        $"Diploma report parameter warning: {parameterEx}"
                    );
                }

                // ============================================================
                // 7. Render PDF
                // ============================================================
                byte[] pdf;

                try
                {
                    pdf = report.Render("PDF");
                }
                catch (Exception renderEx)
                {
                    Console.WriteLine(
                        $"Diploma PDF render failed for ID {id}: {renderEx}"
                    );

                    return StatusCode(500, new
                    {
                        message = "Diploma PDF rendering failed.",
                        id = id,
                        error = renderEx.Message,
                        innerError = renderEx.InnerException?.Message,
                        fullError = renderEx.ToString()
                    });
                }

                // ============================================================
                // 8. Validate PDF
                // ============================================================
                if (pdf == null || pdf.Length == 0)
                {
                    return StatusCode(500, new
                    {
                        message = "RDLC generated an empty PDF.",
                        id = id
                    });
                }

                // ============================================================
                // 9. Return PDF
                // ============================================================
                return File(
                    pdf,
                    "application/pdf",
                    $"Diploma_{id}.pdf"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Diploma printing failed for ID {id}: {ex}"
                );

                return StatusCode(500, new
                {
                    message = "Diploma printing failed.",
                    id = id,
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    fullError = ex.ToString()
                });
            }
        }


        // ====================================================================
        // DIPLOMA PRINT DATA
        // ====================================================================

        private async Task<DataTable> DiplomaPrintData(int id)
        {
            var dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("StudentNameHindi");
            dt.Columns.Add("FatherName");
            dt.Columns.Add("FatherNameHindi");
            dt.Columns.Add("MotherName");
            dt.Columns.Add("CourseNameHindi");
            dt.Columns.Add("EnrollmentNo");
            dt.Columns.Add("Rollno");
            dt.Columns.Add("SrnoDiploma");
            dt.Columns.Add("IssueDate");
            dt.Columns.Add("IssueYear");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("QRImage");
            dt.Columns.Add("SelfImage");

            var data = await _unitOfWork.DocumentProfile
                .GetDiplomaPrintData(id);

            if (data != null)
            {
                DataRow row = dt.NewRow();

                // ============================================================
                // Uploads folder - Linux compatible path
                // ============================================================
                string folderPath = Path.Combine(
                    _appEnvironment.WebRootPath,
                    "Uploads"
                );

                // ============================================================
                // Image path
                // ============================================================
                string fileName = Convert.ToString(data.SelfImage);

                string filePath = Path.Combine(
                    folderPath,
                    fileName ?? string.Empty
                );

                byte[] photoUrl = null;

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        photoUrl = Getimage(filePath);
                    }
                }

                // ============================================================
                // QR Image
                // ============================================================
                string imagePath = "";

                row["QRImage"] = imagePath;

                // ============================================================
                // Student Data
                // ============================================================
                row["Name"] = data.Name;
                row["StudentNameHindi"] = data.StudentNameHindi;
                row["FatherName"] = data.FatherName;
                row["FatherNameHindi"] = data.FatherNameHindi;
                row["MotherName"] = data.MotherName;
                row["CourseNameHindi"] = data.CourseNameHindi;
                row["EnrollmentNo"] = data.EnrollmentNo;
                row["Rollno"] = data.Rollno;
                row["SrnoDiploma"] = data.SrnoDiploma;
                row["IssueDate"] = data.IssueDate;
                row["IssueYear"] = data.IssueYear;
                row["CourseName"] = data.CourseName;
                row["DepartmentName"] = data.DepartmentName;

                // ============================================================
                // Student Image
                // ============================================================
                if (photoUrl == null || photoUrl.Length == 0)
                {
                    row["SelfImage"] = "";
                }
                else
                {
                    row["SelfImage"] =
                        Convert.ToBase64String(photoUrl);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

    }
}
