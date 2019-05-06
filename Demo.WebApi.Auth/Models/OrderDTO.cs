using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Auth.Models
{
    public class OrderDTO
    {
        [Required]
        [StringLength(maximumLength:20)]
        public string OrderNumber { get; set; }
        
        public Decimal Amount { get; set; }

        public string OwnerUser { get; set; }
    }
}