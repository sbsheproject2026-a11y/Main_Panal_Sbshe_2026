
using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Application.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetConnectionString()
               ?? throw new InvalidOperationException(
                   "Database connection string not found.");
        }

        //---------------------------------------State Start------------------------------

        public async Task<List<LocationGetDTOs>> GetAll(int typeid)
        {
            try
            {
                var data = await _context.Location.Where(x => x.IsActive == 1 && x.TypeId == typeid).OrderByDescending(x => x.Id)
                    .Select(x => new LocationGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        TypeId = x.TypeId,

                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching  data.", ex);
            }
        }


        public async Task<string> Create(WalletPostDTOs model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("InsWalletRecharage", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@EntityId", 112));
                command.Parameters.Add(new SqlParameter("@TotalAmount", model.TotalAmount));
                command.Parameters.Add(new SqlParameter("@Remark", model.Remark));
                command.Parameters.Add(new SqlParameter("@EntryDate", model.EntryDate));
                command.Parameters.Add(new SqlParameter("@FranchiseId", model.FranchiseId));
                command.Parameters.Add(new SqlParameter("@ParentFranchiseId", 1));
                command.Parameters.Add(new SqlParameter("@PayMode", model.PayMode));
                command.Parameters.Add(new SqlParameter("@LedgerHeadId", model.LedgerHeadId));
                command.Parameters.Add(new SqlParameter("@Status", model.Status));
                command.Parameters.Add(new SqlParameter("@SlipUpload", model.SlipUpload));
                if (model.AccountId != null)
                {

                    command.Parameters.Add(new SqlParameter("@AccountId", model.AccountId));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@AccountId", 117));

                }

                command.Parameters.Add(new SqlParameter("@VoucherType", 106));


                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "Wallet created successfully";
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
                var data = await _context.Location
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterType not found";
                }

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return "Location delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Location", ex);
            }
        }

        //---------------------------------------State End------------------------------





    }
}
