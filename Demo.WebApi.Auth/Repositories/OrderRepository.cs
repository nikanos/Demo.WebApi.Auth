using Demo.WebApi.Auth.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Demo.WebApi.Auth.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void Add(Order entity)
        {
            //Implement logic to store order here.
            //no logic for this demo!
        }

        public IQueryable<Order> All()
        {
            //Dummy in-memory fill of orders
            return (Enumerable.Empty<Order>().
                    Union(GenerateUserOrders(user: "test", numberOfOrders: 1)).
                    Union(GenerateUserOrders(user: "frodo", numberOfOrders: 5)).
                    Union(GenerateUserOrders(user: "nik", numberOfOrders: 10))
                    ).AsQueryable();
        }

        private IEnumerable<Order> GenerateUserOrders(string user, int numberOfOrders)
        {
            for (int i = 1; i <= numberOfOrders; i++)
            {
                yield return new Order
                {
                    OrderNumber = i.ToString(),
                    Amount = i * 100,
                    OwnerUser = user
                };
            }
        }
    }
}