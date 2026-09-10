using Application.DTOs;

using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;




namespace Application.Repository
{
    public class WebsiteContentRepository : IWebsiteContentRepository
    {
        private readonly ApplicationDbContext _context;

        public WebsiteContentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------


        public async Task<List<FrenchiseGetDTOs>> GetAll()
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                await using (var connection = new SqlConnection(connectionString))
                await using (var command = new SqlCommand("GetListFrenchise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new FrenchiseGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),

                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),

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
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }




    }
}
