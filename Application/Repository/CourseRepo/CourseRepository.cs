using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;




namespace Application.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<List<CourseGetDTOs>> GetAll(int referenceId, int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<CourseGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetCourse";
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
                                data.Add(new CourseGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IsActive = Convert.ToInt32(reader["IsActive"]),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    NameHindi = reader["CourseNameHindi"]?.ToString(),
                                    DurationName = reader["Duration"]?.ToString(),
                                    CategoryName = reader["Category"]?.ToString(),
                                    DepartmentName = reader["Department"]?.ToString(),
                                   
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


        public async Task<string> Create(CoursePostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "insCourse";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ShortName", model.ShortName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@NameHindi", model.NameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Duration", model.Duration));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DurationTypeId", model.DurationTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ParentId", model.ParentId ?? null));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CategoryId", model.CategoryId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DepartmentId", model.DepartmentId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@TotalClass", model.TotalClass));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ClassNo", model.ClassNo));


                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Course created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<CourseGetDTOs> GetById(int id)
        {
            try
            {
                CourseGetDTOs data = null;

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "getEditCourse";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Id", id));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                data = new CourseGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IsActive = Convert.ToInt32(reader["isActive"]),
                                    Duration = Convert.ToInt32(reader["Duration"]),
                                    DurationTypeId = Convert.ToInt32(reader["DurationTypeId"]),
                                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                                    TotalClass = Convert.ToInt32(reader["TotalClass"]),
                                    ClassNo = Convert.ToInt32(reader["ClassNo"]),
                                    ParentId = Convert.ToInt32(reader["ParentId"]),
                                    Code = reader["Code"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    NameHindi = reader["CourseNameHindi"]?.ToString(),
                                     
                                };
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise by id.", ex);
            }
        }


        public async Task<string> Update(CoursePostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateCourse";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ShortName", model.ShortName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@NameHindi", model.NameHindi));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Duration", model.Duration));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DurationTypeId", model.DurationTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ParentId", model.ParentId ?? null));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CategoryId", model.CategoryId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DepartmentId", model.DepartmentId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@TotalClass", model.TotalClass));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ClassNo", model.ClassNo));


                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise Update successfully";
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
                    command.CommandText = "GetCourseDelete";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", 0));

                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise Delete successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        //---------------------------------------Frenchise End------------------------------

 

    }
}
