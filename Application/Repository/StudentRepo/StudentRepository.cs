using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;




namespace Application.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<List<StudentGetDTOs>> GetAll(int referenceId, int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetStudent";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    DistrictName = reader["DistrictName"]?.ToString(),
                                    StateName = reader["StateName"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }

        public async Task<List<StudentGetDTOs>> GetSendToConfirm(int referenceId, int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetStudentSendtoConfirm";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    DistrictName = reader["DistrictName"]?.ToString(),
                                    StateName = reader["StateName"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }
        public async Task<List<StudentGetDTOs>> GetConfirmToAdmin(int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetConfirmToAdmin";
                        command.CommandType = CommandType.StoredProcedure;

                        // command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    DistrictName = reader["DistrictName"]?.ToString(),
                                    StateName = reader["StateName"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }
        public async Task<List<StudentGetDTOs>> GetConfirmAddmissions(int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetConfirmAddmissions";
                        command.CommandType = CommandType.StoredProcedure;

                        // command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                    Code = reader["Code"]?.ToString(),
                                    EnrollmentNo = reader["EnrollmentNo"]?.ToString(),
                                    Rollno = reader["Rollno"]?.ToString(),
                                    RollNo1 = reader["RollNo1"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    DistrictName = reader["DistrictName"]?.ToString(),
                                    StateName = reader["StateName"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }


        public async Task<string> Create(StudentPostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsStudent";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@StudentNameHindi", model.StudentNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherNameHindi", model.FatherNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MotherName", model.MotherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MotherNameHindi", model.MotherNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@UserName", model.UserName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ReferenceId", model.ReferenceId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@PasswordHash", model.PasswordHash));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SignatureImage", model.SignatureImage));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AadhaarCardFrant", model.AadhaarCardFrant));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AadhaarCardBack", model.AadhaarCardBack));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AdmissionDate", model.AdmissionDate));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DOB", model.DOB));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CasteCategory", model.CasteCategory));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CourseId", model.CourseId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ExamSessionId", model.ExamSessionId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@StudyModeId", model.StudyModeId));




                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
       
        public async Task<string> CreateAcademicDetail(int entityId,AcademicDetailDTO model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsAcademicDetails";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LevelTypeId", model.LevelTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EntityId", entityId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SchoolCollege", model.SchoolCollege));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@RollNo", model.RollNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@BoardUniversity", model.BoardUniversity));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@PercentageCgpa", model.PercentageCgpa));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FileName", model.FileName));
                     
                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "AcademicDetail created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<StudentGetDTOs> GetById(int id)
        {
            try
            {
                StudentGetDTOs data = null;

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetEditStudent";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Id", id));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                data = new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    LocationId = Convert.ToInt32(reader["LocationId"]),
                                    DistrictId = Convert.ToInt32(reader["DistrictId"]),
                                    StateId = Convert.ToInt32(reader["StateId"]),
                                    GenderId = Convert.ToInt32(reader["GenderId"]),
                                    CasteCategoryId = Convert.ToInt32(reader["CasteCategoryId"]),

                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    CourseCategoryId = Convert.ToInt32(reader["CourseCategoryId"]),
                                    CourseTypeId = Convert.ToInt32(reader["CourseTypeId"]),
                                    ExamSessionId = Convert.ToInt32(reader["ExamSessionId"]),

                                    Name = reader["Name"]?.ToString(),
                                    StudentNameHindi = reader["StudentNameHindi"]?.ToString(),
                                    FatherNameHindi = reader["FatherNameHindi"]?.ToString(),
                                    DOB = Convert.ToDateTime(reader["DOB"]),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Pincode = reader["PinCode"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),

                                    Email = reader["Email"]?.ToString(),
                                    SelfImage = reader["SelfImage"]?.ToString(),
                                    SignatureImage = reader["SignatureImage"]?.ToString(),
                                    AadhaarCardFrant = reader["AadhaarCardFrant"]?.ToString(),
                                    AadhaarCardBack = reader["AadhaarCardBack"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    SignatureImageShow = "http://localhost:5188/Uploads/" + reader["SignatureImage"]?.ToString(),
                                    AadhaarCardFrantShow = "http://localhost:5188/Uploads/" + reader["AadhaarCardFrant"]?.ToString(),
                                    AadhaarCardBackShow = "http://localhost:5188/Uploads/" + reader["AadhaarCardBack"]?.ToString(),
                                    IdNumber = reader["IdNumber"]?.ToString(),




                                };
                                if (data != null)
                                {
                                    data.DOB1 = data.DOB?.ToString("yyyy-MM-dd");

                                }
                            }
                        }
                    }
                }


                return data;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Employee by id.", ex);
            }
        }


        public async Task<string> Update(StudentPostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateStudent";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@StudentNameHindi", model.StudentNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherNameHindi", model.FatherNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MotherName", model.MotherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MotherNameHindi", model.MotherNameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SignatureImage", model.SignatureImage));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AadhaarCardFrant", model.AadhaarCardFrant));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AadhaarCardBack", model.AadhaarCardBack));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsActive", model.IsActive));

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DOB", model.DOB));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CasteCategory", model.CasteCategory));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CourseId", model.CourseId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ExamSessionId", model.ExamSessionId));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Update successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }



        public async Task<string> Delete(int Id)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetDelete";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", 12));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> GetUpdateStudent(int Id)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetSendToAdmin";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", 12));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> ConfirmToAdmin(int Id)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetUpdateStudentStatus";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", 12));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }



        public async Task<List<StudentGetDTOs>> UnSetMarksStudents(int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetUnSetMarksStudents";
                        command.CommandType = CommandType.StoredProcedure;

                        // command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    PuId = Convert.ToInt32(reader["PuId"]),
                                    ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                    Code = reader["Code"]?.ToString(),
                                    EnrollmentNo = reader["EnrollmentNo"]?.ToString(),
                                    Rollno = reader["Rollno"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    DistrictName = reader["DistrictName"]?.ToString(),
                                    StateName = reader["StateName"]?.ToString(),
                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }

        public async Task<List<GetStudentMarksGetDTOs>> getStudentSubjectMarks(string rollno)
        {
            try
            {
                var data = new List<GetStudentMarksGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "getStudentSubjectMarks";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Rollno", rollno));


                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new GetStudentMarksGetDTOs
                                {
                                    CourseName = reader["CourseName"]?.ToString(),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    Rollno = reader["Rollno"]?.ToString(),
                                    TheoryMarks = Convert.ToInt32(reader["TheoryMarks"]),
                                    PracticalMarks = Convert.ToInt32(reader["PracticalMarks"]),
                                    AssignmentMarks = Convert.ToInt32(reader["AssignmentMarks"]),
                                    PuId = Convert.ToInt32(reader["PuId"]),
                                    SubjectId = Convert.ToInt32(reader["SubjectId"]),

                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }


        //---------------------------------------Frenchise End------------------------------


        public async Task<string> SaveStudentMarks(StudentMarksDto model)
        {
            try
            {
                if (model.Marks == null || !model.Marks.Any())
                {
                    return "No subjects found";
                }

                foreach (var item in model.Marks)
                {
                    // Old marks find karo
                    var oldMarks = await _context.StudentMarks
                        .Where(x =>
                            x.StudentId == item.Pid
                        )
                        .ToListAsync();

                    // Old marks delete karo
                    if (oldMarks.Any())
                    {
                        _context.StudentMarks.RemoveRange(oldMarks);
                    }

                    // New marks insert karo
                    var data = new StudentMarks
                    {
                        StudentId = item.Pid,
                        SubjectId = item.Sid,

                        TheoryMarks = item.TheoryMarks,
                        PracticalMarks = item.PracticalMarks,
                        AssignmentMarks = item.AssignmentMarks,

                        CreatedAt = DateTime.UtcNow,
                        IsActive = 1
                    };

                    await _context.StudentMarks.AddAsync(data);
                }

                await _context.SaveChangesAsync();

                return "Marks saved successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }



        public async Task<List<StudentGetDTOs>> GetUpgradeStudentList(int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetUpgradeStudentList";
                        command.CommandType = CommandType.StoredProcedure;

                        // command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                        command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                        command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new StudentGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    PuId = Convert.ToInt32(reader["PuId"]),
                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    EnrollmentNo = reader["EnrollmentNo"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),

                                    SelfImageShow = "http://localhost:5188/Uploads/" + reader["SelfImage"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }


        public async Task<string> AddClassStudent(StudentClassDto model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "AddClassStudentNew";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@puId", model.PUid[0]));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ClassId", model.ClassId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ExamSession", model.ExamSessionId));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        
        public async Task<List<AcademicDetailGetDTO>> GetAcademicDetailsById(int id)
        {
            try
            {
                var data = new List<AcademicDetailGetDTO>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetAcademicDetailsByEntityId";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@EntityId", id));
                         

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new AcademicDetailGetDTO
                                {
                                    Id = Convert.ToInt32(reader["Id"]),


                                    LevelTypeId = Convert.ToInt32(reader["LevelTypeId"]),

                                    EntityId = Convert.ToInt32(reader["EntityId"]),

                                    SchoolCollege = reader["SchoolCollege"]?.ToString(),

                                    RollNo = reader["RollNo"]?.ToString(),

                                    BoardUniversity = reader["BoardUniversity"]?.ToString(),

                                    PercentageCgpa = reader["PercentageCgpa"]?.ToString(),

                                    FileName = "http://localhost:5188/uploads/" + reader["FileName"]?.ToString()
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }



    }
}
