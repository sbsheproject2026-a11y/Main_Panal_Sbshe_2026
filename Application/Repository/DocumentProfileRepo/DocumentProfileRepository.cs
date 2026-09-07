using Application.DTOs;
using Application.DTOs.EmployeeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;




namespace Application.Repository
{
    public class DocumentProfileRepository : IDocumentProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<StudentGetDTOs> GetDiplomaPrintData(int id)
        {
            try
            {
                StudentGetDTOs data = null;

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetDiplomaPrintData";
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

                                    EnrollmentNo = reader["EnrollmentNo"]?.ToString(),
                                    Rollno = reader["Rollno"]?.ToString(),
                                    RollNo1 = reader["RollNo1"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    FirstName = reader["FirstName"]?.ToString(),
                                    StudentNameHindi = reader["StudentNameHindi"]?.ToString(),
                                    FatherNameHindi = reader["FatherNameHindi"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                    CourseNameHindi = reader["CourseNameHindi"]?.ToString(),
                                    DOB = Convert.ToDateTime(reader["DOB"]),
                                    DOB1 = reader["DOB1"]?.ToString(),
                                
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Pincode = reader["PinCode"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),

                                    Email = reader["Email"]?.ToString(),
                                    SelfImage = reader["SelfImage"]?.ToString(),
                                    IssueDate = reader["IssueDate"]?.ToString(),
                                    StartDate = reader["StartDate"]?.ToString(),
                                    EndDate = reader["EndDate"]?.ToString(),
                                    IssueYear = reader["IssueYear"]?.ToString(),
                                    AdmissionDate1 = reader["AdmissionDate"]?.ToString(),
                                    SrnoDiploma = reader["SrnoDiploma"]?.ToString(),
                                    MigrationSrno = reader["MigrationSrno"]?.ToString(),
                                    DepartmentName = reader["DepartmentName"]?.ToString(),
                                    CertificateName = reader["CertificateName"]?.ToString(),
                                    CentreName = reader["CentreName"]?.ToString(),

                                };

                            }
                        }
                    }
                }


                return data;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Student Print Data by id.", ex);
            }
        }
        public async Task<StudentGetDTOs> GetMarksheetPrintData(string id)
        {
            try
            {
                StudentGetDTOs data = null;

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetMarksheetPrintData";
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

                                    EnrollmentNo = reader["EnrollmentNo"]?.ToString(),
                                    Rollno = reader["Rollno"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    StudentNameHindi = reader["StudentNameHindi"]?.ToString(),
                                    FatherNameHindi = reader["FatherNameHindi"]?.ToString(),
                                    CourseName = reader["CourseName"]?.ToString(),
                                    CourseNameHindi = reader["CourseNameHindi"]?.ToString(),
                                    DOB = Convert.ToDateTime(reader["DOB"]),
                                    DOB1 = reader["DOB1"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    MotherName = reader["MotherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Pincode = reader["PinCode"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    SelfImage = reader["SelfImage"]?.ToString(),
                                    IssueDate = reader["IssueDate"]?.ToString(),
                                    IssueYear = reader["IssueYear"]?.ToString(),
                                    AdmissionDate1 = reader["AdmissionDate"]?.ToString(),
                                    SrnoDiploma = reader["SrnoDiploma"]?.ToString(),
                                    DepartmentName = reader["DepartmentName"]?.ToString(),
                                    Session = reader["Session"]?.ToString(),
                                    StudentYear = reader["StudentYear"]?.ToString(),
                                    CentreName = reader["CentreName"]?.ToString(),
                                    MaxMarks = Convert.ToInt32(reader["MaxMarks"]),
                                    TheoryMarks = Convert.ToInt32(reader["TheoryMarks"]),
                                    PracticalMarks = Convert.ToInt32(reader["PracticalMarks"]),
                                    AssignmentMarks = Convert.ToInt32(reader["AssignmentMarks"]),
                                    TotalMarks = Convert.ToInt32(reader["TotalMarks"]),
                                    Grade = reader["Grade"]?.ToString(),
                                    Result = reader["Result"]?.ToString(),

                                };

                            }
                        }
                    }
                }


                return data;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Student Print Data by id.", ex);
            }
        }



        public async Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectMarks(string id)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("Connection string is NULL or empty.");
                }

                var data = new List<GetStudentSubjectMarksGetDTOs>();

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("PrintStudentSubjectMarks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Rollno", id);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new GetStudentSubjectMarksGetDTOs
                                {
                                    Code = reader["Code"].ToString(),

                                    Name = reader["Name"].ToString(),

                                    Grade = reader["Grade"].ToString(),

                                    TheoryMarks =  reader["TheoryMarks"].ToString(),

                                    PracticalMarks =  reader["PracticalMarks"].ToString(),

                                    AssignmentMarks = reader["AssignmentMarks"].ToString(),

                                    MaxMarks = reader["MaxMarks"].ToString(),

                                    TotalMarks =  reader["TotalMarks"].ToString()
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Print Data by id.",
                    ex
                );
            }
        }
    
        public async Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectAdmitCard(string id)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("Connection string is NULL or empty.");
                }

                var data = new List<GetStudentSubjectMarksGetDTOs>();

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("PrintStudentSubjectAdmitCard", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Rollno", id);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new GetStudentSubjectMarksGetDTOs
                                {
                                    Code = reader["Code"].ToString(),

                                    Name = reader["Name"].ToString(),

                                    Grade = reader["ExaminationDate"].ToString(),
                                     
                                });
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Student Print Data by id.",
                    ex
                );
            }
        }
    
    
    }
}
