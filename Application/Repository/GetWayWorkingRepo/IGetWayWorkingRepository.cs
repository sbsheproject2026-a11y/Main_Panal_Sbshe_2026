using Application.DTOs;
 

namespace Application.Repository 
{
    public interface IGetWayWorkingRepository
    {

        List<OrderList> OrderListAll(string viewtype, int orderid, int EId);

    }
}
