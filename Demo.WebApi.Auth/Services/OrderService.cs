using Demo.WebApi.Auth.Common;
using Demo.WebApi.Auth.Common.Extensions;
using Demo.WebApi.Auth.Models;
using Demo.WebApi.Auth.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Demo.WebApi.Auth.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public void CreateOrder(OrderDTO order)
        {
            Ensure.ArgumentNotNull(order, nameof(order));

            var orderEntity = order.ToEntity();
            orderRepository.Add(orderEntity);            
        }

        public IEnumerable<OrderDTO> GetAllOrders()
        {
            return orderRepository
                .All()
                .ToDTO();
        }

        public OrderDTO GetOrderByUserNameAndOrderNumber(string userName, string orderNumber)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userName, nameof(userName));
            Ensure.StringArgumentNotNullAndNotEmpty(orderNumber, nameof(orderNumber));

            return orderRepository
                .All()
                .Where(x => x.OwnerUser == userName && x.OrderNumber == orderNumber)
                .FirstOrDefault()
                .ToDTO();
        }

        public IEnumerable<OrderDTO> GetOrdersByUserName(string userName)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userName, nameof(userName));

            return orderRepository
                .All()
                .Where(x => x.OwnerUser == userName)
                .ToDTO();
        }
    }
}