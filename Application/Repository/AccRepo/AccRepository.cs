using Application.DTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Data;

namespace Application.Repository
{
    public class AccRepository : IAccRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public AccRepository(ApplicationDbContext context)
        {
            _context = context;

            _connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException(
                    "Database connection string not found.");
        }

        // --------------------------------------- Account Start ---------------------------------------

        public async Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAll( int typeid,  int? referenceId,int? pageNo, int? pageSize, string? search)
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();
                int totalCount = 0;
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("GetAccAcountList", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@typeid", typeid));
                command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter(
                    "@Search",
                    search ?? (object)DBNull.Value));

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

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
                        StateName = reader["StateName"]?.ToString(),
                        
                    });
                    if (reader["TotalRows"] != DBNull.Value)
                    {
                        totalCount = Convert.ToInt32(reader["TotalRows"]);
                    }
                }
                return (data, totalCount);
           
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Account data.",
                    ex);
            }
        }
        
        
        public async Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAllPendingList( int typeid,  int? referenceId,int? pageNo, int? pageSize, string? search)
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();
                int totalCount = 0;
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("GetFranchisePendingList", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@typeid", typeid));
                command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));
                command.Parameters.Add(new SqlParameter("@PageNo", pageNo));
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter(
                    "@Search",
                    search ?? (object)DBNull.Value));

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

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
                    if (reader["TotalRows"] != DBNull.Value)
                    {
                        totalCount = Convert.ToInt32(reader["TotalRows"]);
                    }
                }
                return (data, totalCount);
           
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Account data.",
                    ex);
            }
        }


        public async Task<string> Create(AccRegistrationPostDTOs model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(
                    "InsAccRegistration",
                    connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Code", model.EnrollmentNo));
                command.Parameters.Add(new SqlParameter("@Name", model.Name));
                command.Parameters.Add(new SqlParameter("@FatherName", model.FatherName));
                command.Parameters.Add(new SqlParameter("@LocationId", model.LocationId));
                command.Parameters.Add(new SqlParameter("@Address", model.Address));
                command.Parameters.Add(new SqlParameter("@Pincode", model.Pincode));
                command.Parameters.Add(new SqlParameter("@ContactTypeId", model.ContactTypeId));
                command.Parameters.Add(new SqlParameter("@AddressTypeId", model.AddressTypeId));
                command.Parameters.Add(new SqlParameter("@MobileNo", model.MobileNo));
                command.Parameters.Add(new SqlParameter("@Email", model.Email));
                command.Parameters.Add(new SqlParameter("@DesignationId", model.RoleId));
                command.Parameters.Add(new SqlParameter("@RoleId", model.RoleId));
                command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
                command.Parameters.Add(new SqlParameter("@Password", model.Password));
                command.Parameters.Add(new SqlParameter("@ReferenceId", model.ReferenceId));
                command.Parameters.Add(new SqlParameter("@EntityTypeId", model.EntityTypeId));
                command.Parameters.Add(new SqlParameter(
                    "@WhatsAppContactTypeId",
                    model.WhatsAppContactTypeId));
                command.Parameters.Add(new SqlParameter(
                    "@FatherEntityTypeId",
                    model.FatherEntityTypeId));
                command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                command.Parameters.Add(new SqlParameter("@PasswordHash", model.PasswordHash));
                command.Parameters.Add(new SqlParameter("@InstituteName", model.InstituteName));
                command.Parameters.Add(new SqlParameter("@Experience", model.Experience));
                command.Parameters.Add(new SqlParameter("@Occupation", model.Occupation));
                command.Parameters.Add(new SqlParameter(
                    "@ExpectedAdmissions",
                    model.ExpectedAdmissions));
                command.Parameters.Add(new SqlParameter("@Remark", model.Remark));
                command.Parameters.Add(new SqlParameter("@Dob", model.Dob));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Account created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<FrenchiseGetDTOs?> GetById(int id)
        {
            try
            {
                FrenchiseGetDTOs? data = null;

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(
                    "getEditFrenchise",
                    connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", id));

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

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
                        OtherFacilities = reader["OtherFacilities"]?.ToString(),
                        Experience = reader["Experience"]?.ToString(),
                        InstituteName =  reader["InstituteName"]?.ToString(),
                        Dob = Convert.ToDateTime(reader["DOB"]),

                    };
                    if (data != null)
                    {
                        data.DOB1 = data.Dob?.ToString("yyyy-MM-dd");
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Account by id.",
                    ex);
            }
        }


        public async Task<string> Update(AccRegistrationPostDTOs model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand( "UpdateFrenchise", connection);

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
                command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                command.Parameters.Add(new SqlParameter("@InstituteName", model.InstituteName));
                command.Parameters.Add(new SqlParameter("@Experience", model.Experience));
                command.Parameters.Add(new SqlParameter("@Occupation", model.Occupation));
                command.Parameters.Add(new SqlParameter(
                    "@ExpectedAdmissions",
                    model.ExpectedAdmissions));
                command.Parameters.Add(new SqlParameter("@Remark", model.Remark));
                command.Parameters.Add(new SqlParameter("@Dob", model.Dob));
                command.Parameters.Add(new SqlParameter("@SelfImage", model.PassportPhoto));
                command.Parameters.Add(new SqlParameter("@SignatureImage", model.AuthorizedSignature));
                command.Parameters.Add(new SqlParameter("@AadhaarCardimage", model.AadhaarCardimage));
                command.Parameters.Add(new SqlParameter("@Pancardimage", model.PanCardimage));
                command.Parameters.Add(new SqlParameter("@InstituteCertificateimage", model.InstituteCertificate));
                command.Parameters.Add(new SqlParameter("@BuildingPhoto1image", model.BuildingPhoto1));
                command.Parameters.Add(new SqlParameter("@BuildingPhoto2image", model.BuildingPhoto2));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Account updated successfully";
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
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(  "GetDelete",  connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", id));
                command.Parameters.Add(new SqlParameter("@Code", 1));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Account deleted successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> ConfirmFranchise(int id,int sts)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("GetConfirmFranchise",  connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", id));
                command.Parameters.Add(new SqlParameter("@Code", sts));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Account deleted successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        // --------------------------------------- Account End ---------------------------------------
    }
}
