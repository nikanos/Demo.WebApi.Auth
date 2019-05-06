using Demo.WebApi.Auth.Models;
using System.Collections.Generic;

namespace Demo.WebApi.Auth.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetOrdersByUserName(string userName);
        OrderDTO GetOrderByUserNameAndOrderNumber(string userName, string orderNumber);
        IEnumerable<OrderDTO> GetAllOrders();
        void CreateOrder(OrderDTO order);
    }
}
