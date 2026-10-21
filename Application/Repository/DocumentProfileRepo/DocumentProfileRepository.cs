using Application.DTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Repository
{
    public class DocumentProfileRepository : IDocumentProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public DocumentProfileRepository(ApplicationDbContext context)
        {
            _context = context;

            _connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException(
                    "Database connection string not found.");
        }

        // ---------------------------------------------
        // Authority Letter Print Data
        // ---------------------------------------------

        public async Task<AuthorityLetterPrintGetDTOs?> GetAuthorityLetterPrintData(int id)
        {
            try
            {
                AuthorityLetterPrintGetDTOs? data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetAuthorityLetterPrintData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add( new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new AuthorityLetterPrintGetDTOs
                            {
                                Id = GetInt(reader, "Id"),
                                EntityTypeId = GetInt(reader, "EntityTypeId"),
                                SelfImage =  GetString(reader, "SelfImage"),
                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                FatherName1 = reader["FatherName1"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                CityName = reader["CityName"]?.ToString(),
                                DistrictName = reader["DistrictName"]?.ToString(),
                                StateName = reader["StateName"]?.ToString(),
                                StartDate = reader["StartDate"]?.ToString(),
                                EndDate = reader["EndDate"]?.ToString(),
                                CentreName = reader["CenterName"]?.ToString(),
                                InstituteName = reader["InstituteName"]?.ToString(),
                                Relacition = reader["Relacition"]?.ToString(),
                                EntityType = reader["EntityType"]?.ToString(),
                                RegistrationLine = reader["RegistrationLine"]?.ToString(),
                                CertificateTitle = reader["CertificateTitle"]?.ToString(),
                                CertifyLine = reader["CertifyLine"]?.ToString(),
                                CentreAddress = reader["CentreAddress"]?.ToString(),
                                AppointedLine = reader["AppointedLine"]?.ToString(),
                                SignatoryLine = reader["SignatoryLine"]?.ToString(),
                                OfficeAddress = reader["OfficeAddress"]?.ToString(),
                                CorporateOffice = reader["CorporateOffice"]?.ToString(),
                                EmailLine = reader["EmailLine"]?.ToString(),
                                WebsiteLine = reader["WebsiteLine"]?.ToString(),
                            };
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Diploma Print Data.",
                    ex);
            }
        }
        

        // ---------------------------------------------
        // Diploma Print Data
        // ---------------------------------------------

        public async Task<StudentGetDTOs?> GetDiplomaPrintData(int id)
        {
            try
            {
                StudentGetDTOs? data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetDiplomaPrintData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new StudentGetDTOs
                            {
                                Id = GetInt(reader, "Id"),

                                EnrollmentNo = GetString(reader, "EnrollmentNo"),
                                Rollno = GetString(reader, "Rollno"),
                                RollNo1 = GetString(reader, "RollNo1"),

                                Name = GetString(reader, "Name"),
                                FirstName = GetString(reader, "FirstName"),

                                StudentNameHindi =   GetString(reader, "StudentNameHindi"),

                                FatherNameHindi =  GetString(reader, "FatherNameHindi"),

                                CourseName =   GetString(reader, "CourseName"),

                                CourseNameHindi =  GetString(reader, "CourseNameHindi"),

                                DOB = GetDateTime(reader, "DOB"),
                                DOB1 = GetString(reader, "DOB1"),

                                FatherName =   GetString(reader, "FatherName"),

                                MotherName =  GetString(reader, "MotherName"),

                                Address =   GetString(reader, "Address"),

                                Pincode =   GetString(reader, "PinCode"),

                                MobileNo =   GetString(reader, "MobileNo"),

                                Email =  GetString(reader, "Email"),

                                SelfImage =  GetString(reader, "SelfImage"),

                                IssueDate =  GetString(reader, "IssueDate"),

                                StartDate =   GetString(reader, "StartDate"),

                                EndDate =  GetString(reader, "EndDate"),

                                IssueYear =  GetString(reader, "IssueYear"),

                                AdmissionDate1 =  GetString(reader, "AdmissionDate"),

                                SrnoDiploma = GetString(reader, "SrnoDiploma"),

                                MigrationSrno =  GetString(reader, "MigrationSrno"),

                                DepartmentName =  GetString(reader, "DepartmentName"),

                                CertificateName =  GetString(reader, "CertificateName"),

                                CentreName = GetString(reader, "CentreName"),
                                BranchAddress = GetString(reader, "BranchAddress")
                            };
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Diploma Print Data.",
                    ex);
            }
        }
        public async Task<StudentGetDTOs?> GetNOCPrintData(int id)
        {
            try
            {
                StudentGetDTOs? data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetNOCPrintData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new StudentGetDTOs
                            {
                                Id = GetInt(reader, "Id"),

                                EnrollmentNo = GetString(reader, "EnrollmentNo"),
                                

                                Name = GetString(reader, "Name"),
                                FirstName = GetString(reader, "FirstName"),

                                StudentNameHindi =   GetString(reader, "StudentNameHindi"),

                                FatherNameHindi =  GetString(reader, "FatherNameHindi"),

                                CourseName =   GetString(reader, "CourseName"),

                                CourseNameHindi =  GetString(reader, "CourseNameHindi"),

                                DOB = GetDateTime(reader, "DOB"),
                                DOB1 = GetString(reader, "DOB1"),

                                FatherName =   GetString(reader, "FatherName"),

                                MotherName =  GetString(reader, "MotherName"),

                                Address =   GetString(reader, "Address"),

                                Pincode =   GetString(reader, "PinCode"),

                                MobileNo =   GetString(reader, "MobileNo"),

                                Email =  GetString(reader, "Email"),

                                SelfImage =  GetString(reader, "SelfImage"),

                                IssueDate =  GetString(reader, "IssueDate"),

                                StartDate =   GetString(reader, "StartDate"),

                                EndDate =  GetString(reader, "EndDate"),

                                IssueYear =  GetString(reader, "IssueYear"),

                                AdmissionDate1 =  GetString(reader, "AdmissionDate"),

                                SrnoDiploma = GetString(reader, "SrnoDiploma"),

                                MigrationSrno =  GetString(reader, "MigrationSrno"),
                                NOCSRNO =  GetString(reader, "NOCSRNO"),

                                DepartmentName =  GetString(reader, "DepartmentName"),

                                CertificateName =  GetString(reader, "CertificateName"),

                                CentreName = GetString(reader, "CentreName"),
                                BranchAddress = GetString(reader, "BranchAddress")
                            };
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Diploma Print Data.",
                    ex);
            }
        }


        // ---------------------------------------------
        // Marksheet Print Data
        // ---------------------------------------------

        public async Task<StudentGetDTOs?> GetMarksheetPrintData(string id)
        {
            try
            {
                StudentGetDTOs? data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetMarksheetPrintData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new StudentGetDTOs
                            {
                                Id = GetInt(reader, "Id"),

                                EnrollmentNo =
                                    GetString(reader, "EnrollmentNo"),

                                Rollno =
                                    GetString(reader, "Rollno"),

                                Name =
                                    GetString(reader, "Name"),

                                StudentNameHindi =
                                    GetString(reader, "StudentNameHindi"),

                                FatherNameHindi =
                                    GetString(reader, "FatherNameHindi"),

                                CourseName =
                                    GetString(reader, "CourseName"),

                                CourseNameHindi =
                                    GetString(reader, "CourseNameHindi"),

                                DOB =
                                    GetDateTime(reader, "DOB"),

                                DOB1 =
                                    GetString(reader, "DOB1"),

                                FatherName =
                                    GetString(reader, "FatherName"),

                                MotherName =
                                    GetString(reader, "MotherName"),

                                Address =
                                    GetString(reader, "Address"),

                                Pincode =
                                    GetString(reader, "PinCode"),

                                MobileNo =
                                    GetString(reader, "MobileNo"),

                                Email =
                                    GetString(reader, "Email"),

                                SelfImage =
                                    GetString(reader, "SelfImage"),

                                IssueDate =
                                    GetString(reader, "IssueDate"),

                                IssueYear =
                                    GetString(reader, "IssueYear"),

                                AdmissionDate1 =
                                    GetString(reader, "AdmissionDate"),

                                SrnoDiploma =
                                    GetString(reader, "SrnoDiploma"),

                                DepartmentName =
                                    GetString(reader, "DepartmentName"),

                                Session =
                                    GetString(reader, "Session"),

                                StudentYear =
                                    GetString(reader, "StudentYear"),

                                CentreName =
                                    GetString(reader, "CentreName"),

                                MaxMarks =
                                    GetInt(reader, "MaxMarks"),

                                TheoryMarks =
                                    GetInt(reader, "TheoryMarks"),

                                PracticalMarks =
                                    GetInt(reader, "PracticalMarks"),

                                AssignmentMarks =
                                    GetInt(reader, "AssignmentMarks"),

                                TotalMarks =
                                    GetInt(reader, "TotalMarks"),

                                Grade =
                                    GetString(reader, "Grade"),

                                Result =
                                    GetString(reader, "Result")
                            };
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Marksheet Print Data.",
                    ex);
            }
        }


        // ---------------------------------------------
        // Student Subject Marks
        // ---------------------------------------------

        public async Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectMarks(
            string id)
        {
            try
            {
                var data = new List<GetStudentSubjectMarksGetDTOs>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(
                    "PrintStudentSubjectMarks",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@Rollno", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new GetStudentSubjectMarksGetDTOs
                            {
                                Code =
                                    GetString(reader, "Code"),

                                Name =
                                    GetString(reader, "Name"),

                                Grade =
                                    GetString(reader, "Grade"),

                                TheoryMarks =
                                    GetString(reader, "TheoryMarks"),

                                PracticalMarks =
                                    GetString(reader, "PracticalMarks"),

                                AssignmentMarks =
                                    GetString(reader, "AssignmentMarks"),

                                MaxMarks =
                                    GetString(reader, "MaxMarks"),

                                TotalMarks =
                                    GetString(reader, "TotalMarks")
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Subject Marks.",
                    ex);
            }
        }


        // ---------------------------------------------
        // Student Subject Admit Card
        // ---------------------------------------------

        public async Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectAdmitCard(
            string id)
        {
            try
            {
                var data = new List<GetStudentSubjectMarksGetDTOs>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(
                    "PrintStudentSubjectAdmitCard",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        new SqlParameter("@Rollno", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new GetStudentSubjectMarksGetDTOs
                            {
                                Code =
                                    GetString(reader, "Code"),

                                Name =
                                    GetString(reader, "Name"),

                                Grade =
                                    GetString(reader, "ExaminationDate")
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Admit Card Data.",
                    ex);
            }
        }


        // ---------------------------------------------
        // Helper Methods
        // ---------------------------------------------

        private static string? GetString(
            SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] == DBNull.Value
                ? null
                : reader[columnName]?.ToString();
        }


        private static int GetInt(
            SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader[columnName]);
        }


        private static DateTime? GetDateTime(
            SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] == DBNull.Value
                ? null
                : Convert.ToDateTime(reader[columnName]);
        }
    }
}
