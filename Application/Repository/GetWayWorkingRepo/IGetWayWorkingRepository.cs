using Application.DTOs;
 

namespace Application.Repository 
{
    public interface IGetWayWorkingRepository
    {

        List<OrderList> OrderListAll(string viewtype, int orderid, int EId);
        Task<string> InsgetwayPrm(int OrderId, string GetwayPrm);
        Task<string> InsertOrderStstus(string order_id, string paymentId, string paymentstatus, string R_orderId, decimal amount, int oid, string paymode);
        Task<List<OrderList>> offlinepayment(string viewtype);

    }
}
