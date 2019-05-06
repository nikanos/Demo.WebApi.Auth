using System;

namespace Demo.WebApi.Auth.Entities
{
    public class Order
    {
        public string OrderNumber { get; set; }
        public Decimal Amount { get; set; }
        public string OwnerUser { get; set; }
    }
}