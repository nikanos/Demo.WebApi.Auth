using Demo.WebApi.Auth.Entities;
using Demo.WebApi.Auth.Models;
using System.Collections.Generic;
using System.Linq;

namespace Demo.WebApi.Auth.Common.Extensions
{
    static class ConversionExtensions
    {
        public static OrderDTO ToDTO(this Order order)
        {
            if (order == null)
                return null;

            return new OrderDTO
            {
                OrderNumber = order.OrderNumber,
                Amount = order.Amount,
                OwnerUser = order.OwnerUser
            };
        }

        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> orders)
        {
            if (orders == null)
                return null;

            return orders.Select(x => x.ToDTO());
        }

        public static Order ToEntity(this OrderDTO order)
        {
            if (order == null)
                return null;

            return new Order
            {
                OrderNumber = order.OrderNumber,
                Amount = order.Amount,
                OwnerUser = order.OwnerUser
            };
        }
    }
}