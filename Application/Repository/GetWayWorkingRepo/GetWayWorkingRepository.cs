using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;




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





        //----------------------Master Type Drop Down----------------------------------------



    }
}
