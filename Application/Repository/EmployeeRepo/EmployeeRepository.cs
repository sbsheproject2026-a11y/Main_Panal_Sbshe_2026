using Application.DTOs;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;

namespace Application.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException("Database connection string not found.");
        }

        //--------------------------------------- Employee Start ------------------------------

        public async Task<List<EmployeeGetDTOs>> GetAll(
            int referenceId,
            int pageNo,
            int pageSize,
            string? search)
        {
            try
            {
                var data = new List<EmployeeGetDTOs>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
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
                            data.Add(new EmployeeGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ReferenceId = Convert.ToInt32(reader["ReferenceId"]),
                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                UserName = reader["UserName"]?.ToString(),
                                Password = reader["Password"]?.ToString(),
                                CityName = reader["CityName"]?.ToString(),
                                DistrictName = reader["DistrictName"]?.ToString(),
                                StateName = reader["StateName"]?.ToString()
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Employee data.", ex);
            }
        }


        public async Task<string> Create(EmployeePostDTOs model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("InsEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new SqlParameter("@ContactTypeId", model.ContactTypeId));
                    command.Parameters.Add(new SqlParameter("@AddressTypeId", model.AddressTypeId));
                    command.Parameters.Add(new SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new SqlParameter("@DesignationId", model.DesignationId));
                    command.Parameters.Add(new SqlParameter("@RoleId", model.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
                    command.Parameters.Add(new SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new SqlParameter("@ReferenceId", model.ReferenceId));
                    command.Parameters.Add(new SqlParameter("@EntityTypeId", model.EntityTypeId));
                    command.Parameters.Add(new SqlParameter("@WhatsAppContactTypeId", model.WhatsAppContactTypeId));
                    command.Parameters.Add(new SqlParameter("@FatherEntityTypeId", model.FatherEntityTypeId));
                    command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                    command.Parameters.Add(new SqlParameter("@PasswordHash", model.PasswordHash));
                    command.Parameters.Add(new SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new SqlParameter("@EmployeeTypeId", model.EmployeeTypeId));
                    command.Parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
                    command.Parameters.Add(new SqlParameter("@DateofJoining", model.DateofJoining));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString()
                        ?? "Employee created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<EmployeeGetDTOs> GetById(int id)
        {
            try
            {
                EmployeeGetDTOs data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("getEditEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new EmployeeGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                LocationId = Convert.ToInt32(reader["LocationId"]),
                                DistrictId = Convert.ToInt32(reader["DistrictId"]),
                                StateId = Convert.ToInt32(reader["StateId"]),
                                DesignationId = Convert.ToInt32(reader["DesignationId"]),
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                                GenderId = Convert.ToInt32(reader["GenderId"]),
                                EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),

                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                WhatsAppNo = reader["WhatsAppNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),

                                SelfImage = reader["SelfImage"]?.ToString(),
                                SelfImageShow =
                                   
                                    reader["SelfImage"]?.ToString(),

                                IdNumber = reader["IdNumber"]?.ToString(),

                                DateofJoining =
                                    Convert.ToDateTime(reader["DateofJoining"])
                            };

                            data.DateofJoining1 =
                                data.DateofJoining?.ToString("yyyy-MM-dd");
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Employee by id.",
                    ex);
            }
        }


        public async Task<string> Update(EmployeePostDTOs model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("UpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new SqlParameter("@DesignationId", model.DesignationId));
                    command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                    command.Parameters.Add(new SqlParameter("@GenderId", model.GenderId));
                    command.Parameters.Add(new SqlParameter("@IdNumber", model.IdNumber));
                    command.Parameters.Add(new SqlParameter("@SelfImage", model.SelfImage));
                    command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));
                    command.Parameters.Add(new SqlParameter("@EmployeeTypeId", model.EmployeeTypeId));
                    command.Parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
                    command.Parameters.Add(new SqlParameter("@DateofJoining", model.DateofJoining));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString()
                        ?? "Employee updated successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetDelete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));
                    command.Parameters.Add(new SqlParameter("@Code", 1));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString()
                        ?? "Employee deleted successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        //--------------------------------------- Employee End ------------------------------


        public async Task<string> AssignFrenchise(EmployeeFrenchiseAssignDTOs model)
        {
            try
            {
                foreach (var frenchiseId in model.FrenchiseIds)
                {
                    var data = new EmployeeFrenchiseAssign
                    {
                        FrenchiseId = frenchiseId,
                        EmployeeId = model.EmployeeId,
                        IsActive = 1
                    };

                    await _context.EmployeeFrenchiseAssign.AddAsync(data);
                }

                await _context.SaveChangesAsync();

                return "Employee Frenchise Assign created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<List<EmployeeGetDTOs>> GetAssignFrenchise(
            int referenceId,
            int pageNo,
            int pageSize,
            string? search)
        {
            try
            {
                var data = new List<EmployeeGetDTOs>();

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("GetAssignFrenchise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
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
                            data.Add(new EmployeeGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["EmployeeName"]?.ToString(),
                                FatherName = reader["FrenchiseName"]?.ToString()
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching assigned Frenchise data.",
                    ex);
            }
        }


        public async Task<string> DeleteAssignFrenchise(int id)
        {
            try
            {
                var data = await _context.EmployeeFrenchiseAssign
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return "Employee Frenchise Assign not found";
                }

                _context.EmployeeFrenchiseAssign.Remove(data);

                await _context.SaveChangesAsync();

                return "Employee Frenchise Assign deleted successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while deleting Employee Frenchise Assign",
                    ex);
            }
        }


        public async Task<string> PasswordUpdate(UpdatePasswordDTOs model)
        {
            try
            {
                var data = await _context.MasterLogin
                    .FirstOrDefaultAsync(x => x.EntityId == model.Id);

                if (data == null)
                {
                    return "Login Data not found";
                }

                data.Password = model.Password;
                data.HashPassword = model.HashPassword;

                await _context.SaveChangesAsync();

                return "Login updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while updating Password",
                    ex);
            }
        }
    }
}
