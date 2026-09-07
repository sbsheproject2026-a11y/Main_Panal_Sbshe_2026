using Application.DTOs;
 
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;




namespace Application.Repository 
{
    public class AccRepository : IAccRepository
    {
        private readonly ApplicationDbContext _context;

        public AccRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<List<FrenchiseGetDTOs>> GetAll(int referenceId, int pageNo, int pageSize, string? search)
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetAccAcountList";
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
                                data.Add(new FrenchiseGetDTOs
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
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }
       
        
        public async Task<string> Create(AccRegistrationPostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "InsAccRegistration";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name ));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ContactTypeId", model.ContactTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@AddressTypeId", model.AddressTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DesignationId", model.RoleId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@RoleId", model.RoleId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@UserName", model.UserName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ReferenceId", model.ReferenceId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EntityTypeId", model.EntityTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@WhatsAppContactTypeId", model.WhatsAppContactTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherEntityTypeId", model.FatherEntityTypeId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@PasswordHash", model.PasswordHash));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@InstituteName", model.InstituteName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Experience", model.Experience));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Occupation", model.Occupation));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ExpectedAdmissions", model.ExpectedAdmissions));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Remark", model.Remark));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Dob", model.Dob));

                    if (command.Connection.State != System.Data.ConnectionState.Open) await command.Connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<FrenchiseGetDTOs> GetById(int id)
        {
            try
            {
                FrenchiseGetDTOs data = null;

                using (var connection = _context.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "getEditFrenchise";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Id", id));

                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                data = new FrenchiseGetDTOs
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    LocationId = Convert.ToInt32(reader["LocationId"]),
                                    DistrictId = Convert.ToInt32(reader["DistrictId"]),
                                    StateId = Convert.ToInt32(reader["StateId"]),
                                    Name = reader["Name"]?.ToString(),
                                    FatherName = reader["FatherName"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Pincode = reader["PinCode"]?.ToString(),
                                    MobileNo = reader["MobileNo"]?.ToString(),
                                    WhatsAppNo = reader["WhatsAppNo"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
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

        
        public async Task<string> Update(FrenchisePostDTOs model)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UpdateFrenchise";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", model.Code));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Name", model.Name));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FatherName", model.FatherName));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LocationId", model.LocationId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Address", model.Address));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Pincode", model.Pincode));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MobileNo", model.MobileNo));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Email", model.Email));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@WhatsAppNo", model.WhatsAppNo));

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
                    command.CommandText = "GetDelete";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Id",Id));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Code", 1));
   
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
