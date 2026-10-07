using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;
using System.Drawing;
using System.Linq;




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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

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
                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                                InstituteName = reader["CentreName"]?.ToString(),
                            });
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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetStudentSendtoConfirm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new StudentGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Amount = Convert.ToInt32(reader["Amount"]),
                                ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                CourseId = Convert.ToInt32(reader["CourseId"]),
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
                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                                InstituteName = reader["CentreName"]?.ToString(),
                            });
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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetConfirmToAdmin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

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
                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                                InstituteName = reader["CentreName"]?.ToString(),
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching students for admin confirmation.", ex);
            }
        }

        public async  Task<(List<StudentGetDTOs> StudentList, int TotalCount)> GetConfirmAddmissions(int? referenceId,int? pageNo, int? pageSize, string? search, int? studyModeId )
        {
            try
            {
                var data = new List<StudentGetDTOs>();
                int totalCount = 0;
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetConfirmAddmissions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@studyModeId", studyModeId));
                    command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

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
                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                                InstituteName = reader["CentreName"]?.ToString(),
                            });
                            if (reader["TotalRows"] != DBNull.Value)
                            {
                                totalCount = Convert.ToInt32(reader["TotalRows"]);
                            }
                        }
                    }
                }

                return (data, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching confirmed admissions.", ex);
            }
        }
        

        public async Task<string> Create(StudentPostDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("InsStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new SqlParameter("@StudentNameHindi", model.StudentNameHindi));
                    command.Parameters.Add(new SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new SqlParameter("@FatherNameHindi", model.FatherNameHindi));
                    command.Parameters.Add(new SqlParameter("@MotherName", model.MotherName));
                    command.Parameters.Add(new SqlParameter("@MotherNameHindi", model.MotherNameHindi));
                    command.Parameters.Add(new SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
                    command.Parameters.Add(new SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new SqlParameter("@ReferenceId", model.ReferenceId));
                    command.Parameters.Add(new SqlParameter("@PasswordHash", model.PasswordHash));
                    command.Parameters.Add(new SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new SqlParameter("@SignatureImage", model.SignatureImage));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardFrant", model.AadhaarCardFrant));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardBack", model.AadhaarCardBack));
                    command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new SqlParameter("@AdmissionDate", model.AdmissionDate));
                    command.Parameters.Add(new SqlParameter("@DOB", model.DOB));
                    command.Parameters.Add(new SqlParameter("@CasteCategory", model.CasteCategory));
                    command.Parameters.Add(new SqlParameter("@CourseId", model.CourseId));
                    command.Parameters.Add(new SqlParameter("@ExamSessionId", model.ExamSessionId));
                    command.Parameters.Add(new SqlParameter("@StudyModeId", model.StudyModeId));
                    command.Parameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Student created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> CreateAcademicDetail(int entityId, AcademicDetailDTO model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("InsAcademicDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LevelTypeId", model.LevelTypeId));
                    command.Parameters.Add(new SqlParameter("@EntityId", entityId));
                    command.Parameters.Add(new SqlParameter("@SchoolCollege", model.SchoolCollege));
                    command.Parameters.Add(new SqlParameter("@RollNo", model.RollNo));
                    command.Parameters.Add(new SqlParameter("@BoardUniversity", model.BoardUniversity));
                    command.Parameters.Add(new SqlParameter("@PercentageCgpa", model.PercentageCgpa));
                    command.Parameters.Add(new SqlParameter("@FileName", model.FileName));

                    await connection.OpenAsync();

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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetEditStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

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
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                FatherNameHindi = reader["FatherNameHindi"]?.ToString(),
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

                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                SignatureImageShow =  reader["SignatureImage"]?.ToString(),
                                AadhaarCardFrantShow =   reader["AadhaarCardFrant"]?.ToString(),
                                AadhaarCardBackShow =   reader["AadhaarCardBack"]?.ToString(),

                                IdNumber = reader["IdNumber"]?.ToString()
                            };

                            if (data != null)
                            {
                                data.DOB1 = data.DOB?.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Student by id.", ex);
            }
        }
        public async Task<StudentGetDTOs> GetStudentDetailsPrint(int id)
        {
            try
            {
                StudentGetDTOs data = null;

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetStudentDetail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

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
                                ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                StateId = Convert.ToInt32(reader["StateId"]),
                                GenderId = Convert.ToInt32(reader["GenderId"]),
                                CasteCategoryId = Convert.ToInt32(reader["CasteCategoryId"]),

                                CourseId = Convert.ToInt32(reader["CourseId"]),
                                CourseCategoryId = Convert.ToInt32(reader["CourseCategoryId"]),
                                CourseTypeId = Convert.ToInt32(reader["CourseTypeId"]),
                                ExamSessionId = Convert.ToInt32(reader["ExamSessionId"]),
                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                StudentNameHindi = reader["StudentNameHindi"]?.ToString(),
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                FatherNameHindi = reader["FatherNameHindi"]?.ToString(),
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

                                SelfImageShow =   reader["SelfImage"]?.ToString(),
                                SignatureImageShow =  reader["SignatureImage"]?.ToString(),
                                AadhaarCardFrantShow =   reader["AadhaarCardFrant"]?.ToString(),
                                AadhaarCardBackShow =   reader["AadhaarCardBack"]?.ToString(),
                                CourseCode =   reader["CourseCode"]?.ToString(),
                                Duration =   reader["Duration"]?.ToString(),

                                IdNumber = reader["IdNumber"]?.ToString()
                            };

                            if (data != null)
                            {
                                data.DOB1 = data.DOB?.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Student by id.", ex);
            }
        }


        public async Task<string> Update(StudentPostDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new SqlParameter("@StudentNameHindi", model.StudentNameHindi));
                    command.Parameters.Add(new SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new SqlParameter("@FatherNameHindi", model.FatherNameHindi));
                    command.Parameters.Add(new SqlParameter("@MotherName", model.MotherName));
                    command.Parameters.Add(new SqlParameter("@MotherNameHindi", model.MotherNameHindi));
                    command.Parameters.Add(new SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new SqlParameter("@SignatureImage", model.SignatureImage));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardFrant", model.AadhaarCardFrant));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardBack", model.AadhaarCardBack));
                    command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new SqlParameter("@DOB", model.DOB));
                    command.Parameters.Add(new SqlParameter("@CasteCategory", model.CasteCategory));
                    command.Parameters.Add(new SqlParameter("@CourseId", model.CourseId));
                    command.Parameters.Add(new SqlParameter("@ExamSessionId", model.ExamSessionId));

                    await connection.OpenAsync();

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
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetDelete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", Id));
                    command.Parameters.Add(new SqlParameter("@Code", 12));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<string> insSendForConfirm(int Id,int FranchiseId, int ParentFranchiseId, int TotalAmount, string unic6)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
               // using (var command = new SqlCommand("GetSendToAdmin", connection))
                using (var command = new SqlCommand("insSendForConfirm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@StudentId", Id));
                    command.Parameters.Add(new SqlParameter("@FranchiseId", FranchiseId));
                    command.Parameters.Add(new SqlParameter("@ParentFranchiseId", ParentFranchiseId));
                    command.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                    command.Parameters.Add(new SqlParameter("@demoId", unic6));
                      
                    await connection.OpenAsync();

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
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetUpdateStudentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", Id));
                    command.Parameters.Add(new SqlParameter("@Code", 12));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Employee Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }



        public async Task<List<StudentGetDTOs>> UnSetMarksStudents(int? referenceId, int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetUnSetMarksStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

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
                                SelfImageShow =  reader["SelfImage"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                            });
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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("getStudentSubjectMarks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Rollno", rollno));

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

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Student Subject Marks.", ex);
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

                var studentIds = model.Marks
                    .Select(x => x.Pid)
                    .Distinct()
                    .ToList();

                // Existing marks delete karo
                var oldMarks = await _context.StudentMarks
                    .Where(x => studentIds.Contains((int)x.StudentId))
                    .ToListAsync();

                if (oldMarks.Any())
                {
                    _context.StudentMarks.RemoveRange(oldMarks);
                }

                // New marks insert karo
                foreach (var item in model.Marks)
                {
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



        public async Task<List<StudentGetDTOs>> GetUpgradeStudentList(
     int pageNo,
     int pageSize,
     string? search)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetUpgradeStudentList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter(
                        "@Search",
                        search ?? (object)DBNull.Value));

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

                                SelfImageShow =
                                    
                                    reader["SelfImage"]?.ToString(),

                                CourseName = reader["CourseName"]?.ToString(),
                                StartDate = reader["ExamSession"]?.ToString(),
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Upgrade Student data.", ex);
            }
        }


        public async Task<string> AddClassStudent(StudentClassDto model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("AddClassStudentNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add( new SqlParameter("@puId", model.PUid[0]));

                   // command.Parameters.Add(  new SqlParameter("@ClassId", model.ClassId));

                    command.Parameters.Add(  new SqlParameter("@ExamSession", model.ExamSessionId));

                    await connection.OpenAsync();

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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetAcademicDetailsByEntityId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@EntityId", id));

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
                                FileName =   reader["FileName"]?.ToString()
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Academic Details.",
                    ex);
            }
        }



        

        public async Task<string> viewbalview(int FranchiseId, int aa)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("walletbalanceNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", aa);
                    command.Parameters.AddWithValue("@franchiseId", FranchiseId);

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

    }
}
