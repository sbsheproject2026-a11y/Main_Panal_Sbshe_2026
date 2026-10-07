using Application.DTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Repository
{
    public class FrenchiseRepository : IFrenchiseRepository
    {
        private readonly ApplicationDbContext _context;

        public FrenchiseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<(List<FrenchiseGetDTOs> StudentList, int TotalCount)> GetAll(
            int? referenceId,
            int? pageNo,
            int? pageSize,
            string? search)
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();
                int totalCount = 0;
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetFrenchise", connection))
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
                    }
                }

                return (data, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }

        public async Task<string> Create(FrenchisePostDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("InsFrenchise", connection))
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
                    command.Parameters.Add(new SqlParameter("@DesignationId", model.RoleId));
                    command.Parameters.Add(new SqlParameter("@RoleId", model.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
                    command.Parameters.Add(new SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new SqlParameter("@ReferenceId", model.ReferenceId));
                    command.Parameters.Add(new SqlParameter("@EntityTypeId", model.EntityTypeId));
                    command.Parameters.Add(new SqlParameter("@WhatsAppContactTypeId", model.WhatsAppContactTypeId));
                    command.Parameters.Add(new SqlParameter("@FatherEntityTypeId", model.FatherEntityTypeId));
                    command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));
                    command.Parameters.Add(new SqlParameter("@PasswordHash", model.PasswordHash));
                    command.Parameters.Add(new SqlParameter("@InstituteTypeId", model.InstituteTypeId));
                    command.Parameters.Add(new SqlParameter("@Affiliation", model.Affiliation));
                    command.Parameters.Add(new SqlParameter("@Website", model.Website));
                    command.Parameters.Add(new SqlParameter("@TotalArea", model.TotalArea));
                    command.Parameters.Add(new SqlParameter("@NumberofClassrooms", model.NumberofClassrooms));
                    command.Parameters.Add(new SqlParameter("@ComputerLabAvailable", model.ComputerLabAvailable));
                    command.Parameters.Add(new SqlParameter("@NumberofComputers", model.NumberofComputers));
                    command.Parameters.Add(new SqlParameter("@OtherFacilities", model.OtherFacilities));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardNumber", model.AadhaarCardNumber));
                    command.Parameters.Add(new SqlParameter("@PANCardNumber", model.PANCardNumber));
                    command.Parameters.Add(new SqlParameter("@Remark", model.Remark));
                    command.Parameters.Add(new SqlParameter("@Experience", model.Experience));

                    command.Parameters.Add(new SqlParameter("@SelfImage", model.PassportPhoto));
                    command.Parameters.Add(new SqlParameter("@SignatureImage", model.AuthorizedSignature));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardimage", model.AadhaarCardimage));
                    command.Parameters.Add(new SqlParameter("@Pancardimage", model.PanCardimage));
                    command.Parameters.Add(new SqlParameter("@InstituteCertificateimage", model.InstituteCertificate));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto1image", model.BuildingPhoto1));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto2image", model.BuildingPhoto2));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise created successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> CreateInstituteDepartmentDetails(int entityId, FranchiseDepartmentDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("insInstituteDepartmentDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@EntityId", entityId));
                    command.Parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));


                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Institute Department Details created successfully";
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

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("getEditFrenchise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

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
                                InstituteTypeId = Convert.ToInt32(reader["InstituteTypeId"]),

                                NumberofClassrooms =
                                    Convert.ToInt32(reader["NumberofClassrooms"]),

                                ComputerLabAvailable =
                                    Convert.ToInt32(reader["ComputerLabAvailable"]),

                                NumberofComputers =
                                    Convert.ToInt32(reader["NumberofComputers"]),

                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                WhatsAppNo = reader["WhatsAppNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                Affiliation = reader["Affiliation"]?.ToString(),
                                Website = reader["Website"]?.ToString(),
                                TotalArea = reader["TotalArea"]?.ToString(),
                                OtherFacilities = reader["OtherFacilities"]?.ToString(),
                                AadhaarCardNumber =
                                    reader["AadhaarCardNumber"]?.ToString(),
                                PANCardNumber =
                                    reader["PANCardNumber"]?.ToString(),
                                Experience = reader["Experience"]?.ToString(),
                                Remark = reader["Remark"]?.ToString(),

                                PassportPhoto =
                                    reader["SelfImage"]?.ToString(),

                                AuthorizedSignature =
                                    reader["SignatureImage"]?.ToString(),

                                AadhaarCardimage =
                                    reader["AadhaarCardFrant"]?.ToString(),

                                PanCardimage =
                                    reader["Pancardimage"]?.ToString(),

                                InstituteCertificate =
                                    reader["InstituteCertificateimage"]?.ToString(),

                                BuildingPhoto1 =
                                    reader["BuildingPhoto1image"]?.ToString(),

                                BuildingPhoto2 =
                                    reader["BuildingPhoto2image"]?.ToString()
                            };
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
        public async Task<FrenchiseGetDTOs> GetDetaiId(int id)
        {
            try
            {
                FrenchiseGetDTOs data = null;

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("getFrenchiseDetail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new FrenchiseGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Code = reader["Code"]?.ToString(),
                               Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                WhatsAppNo = reader["WhatsAppNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                Affiliation = reader["Affiliation"]?.ToString(),
                                CityName = reader["CityName"]?.ToString(),
                                DistrictName = reader["District"]?.ToString(),
                                StateName = reader["State"]?.ToString(),
                            
                                
                                
                            };
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
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("UpdateFrenchise", connection))
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
                    command.Parameters.Add(new SqlParameter("@WhatsAppNo", model.WhatsAppNo));

                    command.Parameters.Add(new SqlParameter("@InstituteTypeId", model.InstituteTypeId));
                    command.Parameters.Add(new SqlParameter("@Affiliation", model.Affiliation));
                    command.Parameters.Add(new SqlParameter("@Website", model.Website));
                    command.Parameters.Add(new SqlParameter("@TotalArea", model.TotalArea));
                    command.Parameters.Add(new SqlParameter("@NumberofClassrooms", model.NumberofClassrooms));
                    command.Parameters.Add(new SqlParameter("@ComputerLabAvailable", model.ComputerLabAvailable));
                    command.Parameters.Add(new SqlParameter("@NumberofComputers", model.NumberofComputers));
                    command.Parameters.Add(new SqlParameter("@OtherFacilities", model.OtherFacilities));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardNumber", model.AadhaarCardNumber));
                    command.Parameters.Add(new SqlParameter("@PANCardNumber", model.PANCardNumber));
                    command.Parameters.Add(new SqlParameter("@Remark", model.Remark));
                    command.Parameters.Add(new SqlParameter("@Experience", model.Experience));

                    command.Parameters.Add(new SqlParameter("@SelfImage", model.PassportPhoto));
                    command.Parameters.Add(new SqlParameter("@SignatureImage", model.AuthorizedSignature));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardimage", model.AadhaarCardimage));
                    command.Parameters.Add(new SqlParameter("@Pancardimage", model.PanCardimage));
                    command.Parameters.Add(new SqlParameter("@InstituteCertificateimage", model.InstituteCertificate));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto1image", model.BuildingPhoto1));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto2image", model.BuildingPhoto2));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise Update successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> UpdateDocument(FrenchisePostDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("UpdateFrenchiseDocument", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new SqlParameter("@SelfImage", model.PassportPhoto));
                    command.Parameters.Add(new SqlParameter("@SignatureImage", model.AuthorizedSignature));
                    command.Parameters.Add(new SqlParameter("@AadhaarCardimage", model.AadhaarCardimage));
                    command.Parameters.Add(new SqlParameter("@Pancardimage", model.PanCardimage));
                    command.Parameters.Add(new SqlParameter("@InstituteCertificateimage", model.InstituteCertificate));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto1image", model.BuildingPhoto1));
                    command.Parameters.Add(new SqlParameter("@BuildingPhoto2image", model.BuildingPhoto2));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise Document Update successfully";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> UpdatePassword(FranchiseUpdatePasswordDTOs model)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("UpdatePassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", model.Id));
                    command.Parameters.Add(new SqlParameter("@Password", model.Password));
                    command.Parameters.Add(new SqlParameter("@PasswordHash", model.PasswordHash));

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ?? "Frenchise Document Update successfully";
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
                    command.Parameters.Add(new SqlParameter("@Code", 1));

                    await connection.OpenAsync();

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
