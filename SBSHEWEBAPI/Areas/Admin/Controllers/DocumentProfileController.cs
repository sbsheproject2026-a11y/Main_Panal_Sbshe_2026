using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using QRCoder;
using System.Data;
using System.Diagnostics;

namespace SBSHEWEBAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentProfileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        // ✅ Static caches — stamp aur signature ek baar load, forever reuse
        private static readonly object _cacheLock = new();
        private static string _stampBase64Cache;
        private static string _signatureBase64Cache;

        // ✅ Student image max dimensions (PDF print ke liye kaafi)
        private const int STUDENT_IMG_MAX_W = 500;
        private const int STUDENT_IMG_MAX_H = 600;

        public DocumentProfileController(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _environment = environment;
        }

        // ================================================================
        // ✅ HELPER — File bytes (kept for RDLC path)
        // ================================================================
        private byte[]? Getimage(string filepath)
        {
            try
            {
                if (!System.IO.File.Exists(filepath)) return null;
                return System.IO.File.ReadAllBytes(filepath);
            }
            catch { return null; }
        }

        // ================================================================
        // ✅ HELPER — Mime type detection
        // ================================================================
        private static string GetMimeType(string path)
        {
            return Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        // ================================================================
        // ✅ HELPER — Stamp cached (load only once)
        // ================================================================
        private string GetCachedStampBase64()
        {
            if (_stampBase64Cache != null) return _stampBase64Cache;

            lock (_cacheLock)
            {
                if (_stampBase64Cache != null) return _stampBase64Cache;

                try
                {
                    var path = Path.Combine(_environment.WebRootPath, "Uploads", "stemp_1.png");
                    if (!System.IO.File.Exists(path))
                    {
                        Console.WriteLine($"⚠️ Stamp not found: {path}");
                        return "";
                    }

                    var bytes = System.IO.File.ReadAllBytes(path);
                    _stampBase64Cache = "data:image/png;base64," + Convert.ToBase64String(bytes);
                    Console.WriteLine($"✅ Stamp cached ({bytes.Length / 1024}KB)");
                    return _stampBase64Cache;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Stamp error: {ex.Message}");
                    return "";
                }
            }
        }

        // ================================================================
        // ✅ HELPER — Signature cached (load only once)
        // ================================================================
        private string GetCachedSignatureBase64()
        {
            if (_signatureBase64Cache != null) return _signatureBase64Cache;

            lock (_cacheLock)
            {
                if (_signatureBase64Cache != null) return _signatureBase64Cache;

                try
                {
                    var path = Path.Combine(_environment.WebRootPath, "Uploads", "singh.png");
                    if (!System.IO.File.Exists(path))
                    {
                        Console.WriteLine($"⚠️ Signature not found: {path}");
                        return "";
                    }

                    var bytes = System.IO.File.ReadAllBytes(path);
                    _signatureBase64Cache = "data:image/png;base64," + Convert.ToBase64String(bytes);
                    Console.WriteLine($"✅ Signature cached ({bytes.Length / 1024}KB)");
                    return _signatureBase64Cache;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Signature error: {ex.Message}");
                    return "";
                }
            }
        }

        // ================================================================
        // ✅ HELPER — Student image load + auto-resize if big
        // ================================================================
        private async Task<string> LoadStudentImageBase64Async(string selfImage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selfImage)) return "";

                string fileName = Path.GetFileName(selfImage);
                string imagePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

                if (!System.IO.File.Exists(imagePath))
                {
                    Console.WriteLine($"⚠️ Student image not found: {imagePath}");
                    return "";
                }

                byte[] bytes = await System.IO.File.ReadAllBytesAsync(imagePath);

                // ✅ Try resize if image is > 200KB
                byte[] resized = TryResizeImage(bytes, STUDENT_IMG_MAX_W, STUDENT_IMG_MAX_H);
                if (resized != null)
                {
                    Console.WriteLine($"✅ Student image resized: {bytes.Length / 1024}KB → {resized.Length / 1024}KB");
                    return $"data:image/jpeg;base64,{Convert.ToBase64String(resized)}";
                }

                string mimeType = GetMimeType(imagePath);
                return $"data:{mimeType};base64,{Convert.ToBase64String(bytes)}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Student image error: {ex.Message}");
                return "";
            }
        }

        // ================================================================
        // ✅ HELPER — Safe resize (fails gracefully → original bytes)
        // ================================================================
        private byte[] TryResizeImage(byte[] imageBytes, int maxW, int maxH)
        {
            try
            {
                if (imageBytes.Length < 200 * 1024) return null;

                using var ms = new MemoryStream(imageBytes);
                using var img = System.Drawing.Image.FromStream(ms);

                if (img.Width <= maxW && img.Height <= maxH) return null;

                double ratio = Math.Min((double)maxW / img.Width, (double)maxH / img.Height);
                int newW = (int)(img.Width * ratio);
                int newH = (int)(img.Height * ratio);

                using var resized = new System.Drawing.Bitmap(newW, newH);
                using (var g = System.Drawing.Graphics.FromImage(resized))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.DrawImage(img, 0, 0, newW, newH);
                }

                using var outMs = new MemoryStream();
                resized.Save(outMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                return outMs.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Resize failed (using original): {ex.Message}");
                return null;
            }
        }

        // ================================================================
        // ✅ HELPER — QR code (cross-platform, RGBA byte[] overload)
        // ================================================================
        private string GenerateQrBase64(string qrText)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);

                byte[] qrBytes = qrCode.GetGraphic(
                    20,
                    new byte[] { 0, 0, 0, 255 },     // black
                    new byte[] { 255, 255, 255, 0 }, // transparent
                    false
                );

                return "data:image/png;base64," + Convert.ToBase64String(qrBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ QR error: {ex.Message}");
                return "";
            }
        }

        // ====================================================================
        // 1. MARKSHEET PRINT
        // ====================================================================
        [HttpGet("document-print-marksheet-print/{id}")]
        public async Task<IActionResult> MarksheetPrint(string id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var data = await _unitOfWork.DocumentProfile.GetMarksheetPrintData(id);
                var subject = await _unitOfWork.DocumentProfile.PrintStudentSubjectMarks(id);
                Console.WriteLine($"⏱️ Marksheet DB: {sw.ElapsedMilliseconds}ms");

                if (data == null)
                    return NotFound(new { message = $"Diploma data not found for ID: {id}" });

                // ✅ Parallel: student image + QR
                var qrText =
                    "Name-" + (data.Name ?? "") +
                    ",FatherName-" + (data.FatherName ?? "") +
                    ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                    ",Course-" + (data.CourseName ?? "");

                var imageTask = LoadStudentImageBase64Async(data.SelfImage);
                var qrTask = Task.Run(() => GenerateQrBase64(qrText));

                await Task.WhenAll(imageTask, qrTask);

                var selfImageBase64 = imageTask.Result;
                var qrImage = qrTask.Result;

                Console.WriteLine($"⏱️ Marksheet total: {sw.ElapsedMilliseconds}ms");

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
                    subjects = subject,
                    selfImage = selfImageBase64,
                    qrimage = qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Marksheet data fetch failed.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // ====================================================================
        // 2. ADMIT CARD PRINT
        // ====================================================================
        [HttpGet("document-print-admitcard-print/{id}")]
        public async Task<IActionResult> AdmitCardPrint(string id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var data = await _unitOfWork.DocumentProfile.GetMarksheetPrintData(id);
                var subject = await _unitOfWork.DocumentProfile.PrintStudentSubjectAdmitCard(id);
                Console.WriteLine($"⏱️ AdmitCard DB: {sw.ElapsedMilliseconds}ms");

                if (data == null)
                    return NotFound(new { message = $"Diploma data not found for ID: {id}" });

                var selfImageBase64 = await LoadStudentImageBase64Async(data.SelfImage);

                Console.WriteLine($"⏱️ AdmitCard total: {sw.ElapsedMilliseconds}ms");

                return Ok(new
                {
                    name = data.Name ?? "",
                    fatherName = data.FatherName ?? "",
                    enrollmentNo = data.EnrollmentNo ?? "",
                    rollno = data.Rollno ?? "",
                    session = data.Session ?? "",
                    centreName = data.CentreName ?? "",
                    courseName = data.CourseName ?? "",
                    subjects = subject,
                    selfImage = selfImageBase64
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Admit Card data fetch failed.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // ====================================================================
        // 3. DIPLOMA PRINT
        // ====================================================================
        [HttpGet("diploma-print/{id}")]
        public async Task<IActionResult> DiplomaPrint(int id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var data = await _unitOfWork.DocumentProfile.GetDiplomaPrintData(id);
                Console.WriteLine($"⏱️ Diploma DB: {sw.ElapsedMilliseconds}ms");

                if (data == null)
                    return NotFound(new { message = $"Diploma data not found for ID: {id}" });

                var qrText =
                    "Name-" + (data.Name ?? "") +
                    ",FatherName-" + (data.FatherName ?? "") +
                    ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                    ",Course-" + (data.CourseName ?? "");

                // ✅ Parallel: image + QR (stamp/signature cached = instant)
                var imageTask = LoadStudentImageBase64Async(data.SelfImage);
                var qrTask = Task.Run(() => GenerateQrBase64(qrText));

                await Task.WhenAll(imageTask, qrTask);

                var selfImageBase64 = imageTask.Result;
                var stempImageBase64 = GetCachedStampBase64();
                var sighImageBase64 = GetCachedSignatureBase64();
                var qrImage = qrTask.Result;

                Console.WriteLine($"⏱️ Diploma total: {sw.ElapsedMilliseconds}ms");

                return Ok(new
                {
                    name = data.Name ?? "",
                    firstName = data.FirstName ?? "",
                    studentNameHindi = data.StudentNameHindi ?? "",
                    fatherName = data.FatherName ?? "",
                    fatherNameHindi = data.FatherNameHindi ?? "",
                    motherName = data.MotherName ?? "",
                    courseNameHindi = data.CourseNameHindi ?? "",
                    enrollmentNo = data.EnrollmentNo ?? "",
                    rollnoa = data.Rollno ?? "",
                    rollno1 = data.RollNo1 ?? "",
                    rollno = !string.IsNullOrWhiteSpace(data.RollNo1)
                        ? $"{data.Rollno ?? ""} , {data.RollNo1}"
                        : data.Rollno ?? "",
                    srnoDiploma = data.SrnoDiploma ?? "",
                    issueDate = data.IssueDate ?? "",
                    startDate = data.StartDate ?? "",
                    endDate = data.EndDate ?? "",
                    dOB1 = data.DOB1 ?? "",
                    address = data.Address ?? "",
                    issueYear = data.IssueYear ?? "",
                    courseName = data.CourseName ?? "",
                    departmentName = data.DepartmentName ?? "",
                    certificateName = data.CertificateName ?? "",
                    migrationSrno = data.MigrationSrno ?? "",
                    centreName = data.CentreName ?? "",
                    branchAddress = data.BranchAddress ?? "",
                    selfImage = selfImageBase64,
                    stempImage = stempImageBase64,
                    singhImage = sighImageBase64,
                    qrimage = qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Diploma data fetch failed.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // ====================================================================
        // 4. NOC PRINT
        // ====================================================================
        [HttpGet("noc-print/{id}")]
        public async Task<IActionResult> NOCPrint(int id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var data = await _unitOfWork.DocumentProfile.GetNOCPrintData(id);
                Console.WriteLine($"⏱️ NOC DB: {sw.ElapsedMilliseconds}ms");

                if (data == null)
                    return NotFound(new { message = $"NOC data not found for ID: {id}" });

                var qrText =
                    "Name-" + (data.Name ?? "") +
                    ",FatherName-" + (data.FatherName ?? "") +
                    ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                    ",Course-" + (data.CourseName ?? "");

                var imageTask = LoadStudentImageBase64Async(data.SelfImage);
                var qrTask = Task.Run(() => GenerateQrBase64(qrText));

                await Task.WhenAll(imageTask, qrTask);

                var selfImageBase64 = imageTask.Result;
                var stempImageBase64 = GetCachedStampBase64();
                var sighImageBase64 = GetCachedSignatureBase64();
                var qrImage = qrTask.Result;

                Console.WriteLine($"⏱️ NOC total: {sw.ElapsedMilliseconds}ms");

                return Ok(new
                {
                    name = data.Name ?? "",
                    firstName = data.FirstName ?? "",
                    studentNameHindi = data.StudentNameHindi ?? "",
                    fatherName = data.FatherName ?? "",
                    fatherNameHindi = data.FatherNameHindi ?? "",
                    motherName = data.MotherName ?? "",
                    courseNameHindi = data.CourseNameHindi ?? "",
                    enrollmentNo = data.EnrollmentNo ?? "",
                    rollnoa = data.Rollno ?? "",
                    rollno1 = data.RollNo1 ?? "",
                    rollno = !string.IsNullOrWhiteSpace(data.RollNo1)
                        ? $"{data.Rollno ?? ""} , {data.RollNo1}"
                        : data.Rollno ?? "",
                    srnoDiploma = data.SrnoDiploma ?? "",
                    migrationSrno = data.MigrationSrno ?? "",
                    nocSRNO = data.NOCSRNO ?? "",
                    bonafideCode = data.BonafideCode ?? "",
                    trainingReqCode = data.TrainingReqCode ?? "",
                    date = data.Date ?? "",
                    issueDate = data.IssueDate ?? "",
                    startDate = data.StartDate ?? "",
                    endDate = data.EndDate ?? "",
                    dOB1 = data.DOB1 ?? "",
                    address = data.Address ?? "",
                    issueYear = data.IssueYear ?? "",
                    courseName = data.CourseName ?? "",
                    departmentName = data.DepartmentName ?? "",
                    certificateName = data.CertificateName ?? "",
                    centreName = data.CentreName ?? "",
                    branchAddress = data.BranchAddress ?? "",
                    selfImage = selfImageBase64,
                    stempImage = stempImageBase64,
                    singhImage = sighImageBase64,
                    qrimage = qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "NOC data fetch failed.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // ====================================================================
        // 5. DIPLOMA PRINT NEW (RDLC) — unchanged, keeps working
        // ====================================================================
        [HttpGet("diploma-print-new/{id:int}")]
        public async Task<IActionResult> DiplomaPrintNew(int id)
        {
            try
            {
                var dt = await DiplomaPrintData(id);

                if (dt == null)
                    return NotFound(new { message = "Diploma data could not be loaded.", id = id });

                if (dt.Rows.Count == 0)
                    return NotFound(new { message = $"Diploma data not found for ID: {id}", id = id });

                var webRoot = _environment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                    webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var reportPath = Path.Combine(webRoot, "Reports", "Diploma_Print.rdlc");

                if (!System.IO.File.Exists(reportPath))
                    return StatusCode(500, new { message = "Diploma_Print.rdlc not found.", path = reportPath });

                using var report = new Microsoft.Reporting.NETCore.LocalReport();

                await using (var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    report.LoadReportDefinition(reportStream);
                }

                report.DataSources.Clear();
                report.DataSources.Add(new ReportDataSource("DataSet1", dt));

                try
                {
                    report.SetParameters(new List<ReportParameter>
                    {
                        new ReportParameter("check", "Hello")
                    });
                }
                catch (Exception parameterEx)
                {
                    Console.WriteLine($"Diploma report parameter warning: {parameterEx}");
                }

                byte[] pdf;
                try
                {
                    pdf = report.Render("PDF");
                }
                catch (Exception renderEx)
                {
                    Console.WriteLine($"Diploma PDF render failed for ID {id}: {renderEx}");
                    return StatusCode(500, new
                    {
                        message = "Diploma PDF rendering failed.",
                        id = id,
                        error = renderEx.Message,
                        innerError = renderEx.InnerException?.Message,
                        fullError = renderEx.ToString()
                    });
                }

                if (pdf == null || pdf.Length == 0)
                    return StatusCode(500, new { message = "RDLC generated an empty PDF.", id = id });

                return File(pdf, "application/pdf", $"Diploma_{id}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Diploma printing failed for ID {id}: {ex}");
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
        // RDLC HELPER — unchanged
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

            var data = await _unitOfWork.DocumentProfile.GetDiplomaPrintData(id);

            if (data != null)
            {
                DataRow row = dt.NewRow();

                string folderPath = Path.Combine(_environment.WebRootPath, "Uploads");
                string fileName = Convert.ToString(data.SelfImage);
                string filePath = Path.Combine(folderPath, fileName ?? string.Empty);

                byte[] photoUrl = null;
                if (!string.IsNullOrWhiteSpace(fileName) && System.IO.File.Exists(filePath))
                {
                    photoUrl = Getimage(filePath);
                }

                row["QRImage"] = "";
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

                row["SelfImage"] = (photoUrl == null || photoUrl.Length == 0)
                    ? ""
                    : Convert.ToBase64String(photoUrl);

                dt.Rows.Add(row);
            }

            return dt;
        }

        // ====================================================================
        // 6. AUTHORITY LETTER PRINT
        // ====================================================================
        [HttpGet("admin-authority-letter-print/{id}")]
        public async Task<IActionResult> AuthorityLetterPrint(int id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var data = await _unitOfWork.DocumentProfile.GetAuthorityLetterPrintData(id);
                Console.WriteLine($"⏱️ AuthorityLetter DB: {sw.ElapsedMilliseconds}ms");

                if (data == null)
                    return NotFound(new { message = $"Authority Letter data not found for ID: {id}" });

                var qrText =
                    "Name-" + (data.Name ?? "") +
                    ",FatherName-" + (data.FatherName ?? "") +
                    ",EnrollmentNo-" + (data.EnrollmentNo ?? "") +
                    ",Course-" + (data.CourseName ?? "");

                var imageTask = LoadStudentImageBase64Async(data.SelfImage);
                var qrTask = Task.Run(() => GenerateQrBase64(qrText));

                await Task.WhenAll(imageTask, qrTask);

                var selfImageBase64 = imageTask.Result;
                var stempImageBase64 = GetCachedStampBase64();
                var sighImageBase64 = GetCachedSignatureBase64();
                var qrImage = qrTask.Result;

                Console.WriteLine($"⏱️ AuthorityLetter total: {sw.ElapsedMilliseconds}ms");

                return Ok(new
                {
                    name = data.Name ?? "",
                    centreName = data.CentreName ?? "",
                    mobileNo = data.MobileNo ?? "",
                    email = data.Email ?? "",
                    districtName = data.DistrictName ?? "",
                    entityType = data.EntityType ?? "",
                    fatherName1 = data.FatherName1 ?? "",
                    firstName = data.FirstName ?? "",
                    fatherName = data.FatherName ?? "",
                    relacition = data.Relacition ?? "",
                    enrollmentNo = data.Code ?? "",
                    srnoDiploma = data.SrnoDiploma ?? "",
                    endDate = data.EndDate ?? "",
                    startDate = data.StartDate ?? "",
                    address = data.Address ?? "",
                    registrationLine = data.RegistrationLine ?? "",
                    certificateTitle = data.CertificateTitle ?? "",
                    certifyLine = data.CertifyLine ?? "",
                    centreAddress = data.CentreAddress ?? "",
                    appointedLine = data.AppointedLine ?? "",
                    signatoryLine = data.SignatoryLine ?? "",
                    officeAddress = data.OfficeAddress ?? "",
                    corporateOffice = data.CorporateOffice ?? "",
                    emailLine = data.EmailLine ?? "",
                    websiteLine = data.WebsiteLine ?? "",
                    selfImage = selfImageBase64,
                    stempImage = stempImageBase64,
                    singhImage = sighImageBase64,
                    qrimage = qrImage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Authority Letter data fetch failed.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }
    }
}