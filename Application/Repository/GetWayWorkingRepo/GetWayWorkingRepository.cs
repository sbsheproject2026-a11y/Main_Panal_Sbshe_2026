using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;




namespace Application.Repository
{
    public class GetWayWorkingRepository : IGetWayWorkingRepository
    {
        private readonly ApplicationDbContext _context;

        public GetWayWorkingRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<OrderList> OrderListAll(string viewtype, int orderid, int EId)
        {
            List<OrderList> list = new List<OrderList>();
            var connectionString = _context.Database.GetConnectionString();
            // ✅ Local connection — thread-safe
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("OrderListAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@viewtype", viewtype);
                cmd.Parameters.AddWithValue("@orderid", orderid);
                cmd.Parameters.AddWithValue("@EId", EId);

                con.Open();

                using (SqlDataReader Dr = cmd.ExecuteReader())
                {
                    while (Dr.Read())
                    {
                        var obj = new OrderList
                        {
                            EId = Convert.ToInt32(Dr["EId"]),
                            OId = Convert.ToInt32(Dr["OId"]),
                            FeeName = Dr["FeeName"].ToString(),
                            Name = Dr["Name"].ToString(),
                            MobileNo = Dr["MobileNo"].ToString(),
                            EmailId = Dr["EmailId"].ToString(),
                            OrderId = Dr["OrderId"].ToString(),
                            ReferenceId = Dr["ReferenceId"].ToString(),
                            Amount = Convert.ToDecimal(Dr["Amount"]),
                            PayStatus = Dr["PayStatus"].ToString(),
                            ODate = Dr["ODate"].ToString(),
                            Address = Dr["Address"].ToString(),
                            PrintDate = Dr["PrintDate"].ToString(),
                            PrintPayDate = Dr["PrintPayDate"].ToString(),
                            AmountW = Dr["AmountW"].ToString(),
                            ProductId = Convert.ToInt32(Dr["ProductId"]),
                            GetwayPrm = Dr["GetwayPrm"].ToString()
                        };

                        list.Add(obj);
                    }
                }
            }

            return list;
        }

        public async Task<List<OrderList>> offlinepayment(string viewtype)
        {
            List<OrderList> list = new List<OrderList>();
            var connectionString = _context.Database.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("offlinepayment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Status", viewtype);

                // ✅ Async connection open
                await con.OpenAsync();

                // ✅ Async reader
                using (SqlDataReader Dr = await cmd.ExecuteReaderAsync())
                {
                    // ✅ Async read
                    while (await Dr.ReadAsync())
                    {
                        var obj = new OrderList
                        {
                            EId = Convert.ToInt32(Dr["EId"]),
                            OId = Convert.ToInt32(Dr["OId"]),
                            FeeName = Dr["FeeName"].ToString(),
                            Name = Dr["Name"].ToString(),
                            MobileNo = Dr["MobileNo"].ToString(),
                            EmailId = Dr["EmailId"].ToString(),
                            OrderId = Dr["OrderId"].ToString(),
                            ReferenceId = Dr["ReferenceId"].ToString(),
                            Amount = Convert.ToDecimal(Dr["Amount"]),
                            PaidAmount = Convert.ToDecimal(Dr["PaidAmount"]),
                            PendingAmount = Convert.ToDecimal(Dr["PendingAmount"]),
                            PayStatus = Dr["PayStatus"].ToString(),
                            ODate = Dr["ODate"].ToString(),
                            Address = Dr["Address"].ToString(),
                            PrintDate = Dr["PrintDate"].ToString(),
                            PrintPayDate = Dr["PrintPayDate"].ToString(),
                            AmountW = Dr["AmountW"].ToString(),
                            ProductId = Convert.ToInt32(Dr["ProductId"]),
                            GetwayPrm = Dr["GetwayPrm"].ToString()
                        };

                        list.Add(obj);
                    }
                }
            }

            return list;
        }


        public async Task<string> InsgetwayPrm(int OrderId, string GetwayPrm)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("InsgetwayPrm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderId", OrderId);
                    command.Parameters.AddWithValue("@GetwayPrm", GetwayPrm);

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ;
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> InsertOrderStstus(string order_id, string paymentId, string paymentstatus, string R_orderId, decimal amount, int oid, string paymode)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("InsertOrderStstus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@OrderId", order_id);
                    command.Parameters.AddWithValue("@RefrenceId", paymentId);
                    command.Parameters.AddWithValue("@Status", paymentstatus);
                    command.Parameters.AddWithValue("@tracking_id", R_orderId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@OrdertblId", oid);
                    command.Parameters.AddWithValue("@payment_mode", paymode);

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString() ;
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<List<OrderList>> GetStudentFee(int Id)
        {
            List<OrderList> list = new List<OrderList>();
            var connectionString = _context.Database.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetStudentFeeDetail", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);

                // ✅ Async connection open
                await con.OpenAsync();

                // ✅ Async reader
                using (SqlDataReader Dr = await cmd.ExecuteReaderAsync())
                {
                    // ✅ Async read
                    while (await Dr.ReadAsync())
                    {
                        var obj = new OrderList
                        {
                            EId = Convert.ToInt32(Dr["EId"]),
                            OId = Convert.ToInt32(Dr["OId"]),
                            FeeName = Dr["FeeName"].ToString(),
                            Name = Dr["Name"].ToString(),
                            MobileNo = Dr["MobileNo"].ToString(),
                            EmailId = Dr["EmailId"].ToString(),
                            OrderId = Dr["OrderId"].ToString(),
                            ReferenceId = Dr["ReferenceId"].ToString(),
                            Amount = Convert.ToDecimal(Dr["Amount"]),
                            PaidAmount = Convert.ToDecimal(Dr["PaidAmount"]),
                            PendingAmount = Convert.ToDecimal(Dr["PendingAmount"]),
                            PayStatus = Dr["PayStatus"].ToString(),
                            ODate = Dr["ODate"].ToString(),
                            Address = Dr["Address"].ToString(),
                            PrintDate = Dr["PrintDate"].ToString(),
                            PrintPayDate = Dr["PrintPayDate"].ToString(),
                            AmountW = Dr["AmountW"].ToString(),
                            ProductId = Convert.ToInt32(Dr["ProductId"]),
                            GetwayPrm = Dr["GetwayPrm"].ToString()
                        };

                        list.Add(obj);
                    }
                }
            }

            return list;
        }

        public async Task<string> OfflinePayPayment(OfflinePaymentRequest request)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("OfflinePayPayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EntityId", request.UserId);
                    command.Parameters.AddWithValue("@ProductAmount", request.Amount);
                    command.Parameters.AddWithValue("@Remark", request.remark);
                    command.Parameters.AddWithValue("@PaidDate", request.PaidDate);
                    command.Parameters.AddWithValue("@CourseId", request.courseid);
                     

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> GenrateOrder(int Eid, decimal amount, int productid)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GenrateOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Eid", Eid);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@ProductId", productid);
                     

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
         

        //----------------------Master Type Drop Down----------------------------------------



    }
}
