
using Application.DTOs;
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


        public async Task<List<WalletGetDTOs>> GetAll(int UserId, int Amountid)
        {
            try
            {
                var data = new List<WalletGetDTOs>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("ViewWallet", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@FranchiseId", UserId));

                command.Parameters.Add(new SqlParameter("@walletId", Amountid));

               

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    data.Add(new WalletGetDTOs
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FundId = Convert.ToInt32(reader["FundId"]),
                        
                        EntryDate = Convert.ToDateTime(reader["date"]),
                        TransactionType = reader["TransactionType"]?.ToString(),
                        TransactionNumber = reader["TransactionNumber"]?.ToString(),
                        Remark = reader["Remark"]?.ToString(),
                        dr = Convert.ToDecimal(reader["dr"]),
                        cr = Convert.ToDecimal(reader["cr"]),
                       
                        balance = reader["balance"]?.ToString(),
                        Status = reader["Status"]?.ToString(),
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Course data.",
                    ex);
            }
        }
        public async Task<List<WalletGetDTOs>> GetAllPending(int UserId, int Amountid)
        {
            try
            {
                var data = new List<WalletGetDTOs>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("ViewWalletPending", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@FranchiseId", UserId));

                command.Parameters.Add(new SqlParameter("@walletId", Amountid));
                command.Parameters.Add(new SqlParameter("@status", 115));



                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    data.Add(new WalletGetDTOs
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FundId = Convert.ToInt32(reader["FundId"]),
                        
                        EntryDate = Convert.ToDateTime(reader["date"]),
                        TransactionType = reader["TransactionType"]?.ToString(),
                        TransactionNumber = reader["TransactionNumber"]?.ToString(),
                        Remark = reader["Remark"]?.ToString(),
                        dr = Convert.ToDecimal(reader["dr"]),
                        cr = Convert.ToDecimal(reader["cr"]),
                       
                        balance = reader["balance"]?.ToString(),
                        Status = reader["Status"]?.ToString(),
                        Branchname = reader["Branchname"]?.ToString(),
                        Slip = reader["slip"]?.ToString(),
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Course data.",
                    ex);
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
                command.Parameters.Add(new SqlParameter("@PaymentTypeId", model.PaymentTypeId));
                command.Parameters.Add(new SqlParameter("@BankNameId", model.BankNameId));
                command.Parameters.Add(new SqlParameter("@BankConfirmDate", model.BankConfirmDate));
                command.Parameters.Add(new SqlParameter("@URTNo", model.TransactionNo));


                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "Wallet created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> CreateOnline(int eId, decimal amount)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("InsWalletRecharage", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@EntityId", 112));
                command.Parameters.Add(new SqlParameter("@TotalAmount", amount));
                command.Parameters.Add(new SqlParameter("@Remark", "Wallet Recharge"));
                command.Parameters.Add(new SqlParameter("@EntryDate", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@FranchiseId", eId));
                command.Parameters.Add(new SqlParameter("@ParentFranchiseId", 1));
                command.Parameters.Add(new SqlParameter("@PayMode", 144));
                command.Parameters.Add(new SqlParameter("@Status", 116));
                command.Parameters.Add(new SqlParameter("@AccountId", 117));
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



        public async Task<string> WalletPaymentConfirm(int Id,int Status)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PaymentConfirm", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@EntityId", Id));
                command.Parameters.Add(new SqlParameter("@Status", Status));
                


                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "Wallet created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        //---------------------------------------State End------------------------------





    }
}
