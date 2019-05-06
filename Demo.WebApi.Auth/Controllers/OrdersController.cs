using Demo.WebApi.Auth.Infrastructure.WebApi;
using Demo.WebApi.Auth.Models;
using Demo.WebApi.Auth.Repositories;
using Demo.WebApi.Auth.Services;
using System.Web.Http;

namespace Demo.WebApi.Auth.Controllers
{
    [RoutePrefix("api/orders")]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public OrdersController()
        {
            //hardoded dependency here.
            //TODO: Use a DI container to pass the dependency and remove this constructor
            this.orderService = new OrderService(new OrderRepository());
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetOrders()
        {
            string authenticatedUsername = this.AuthenticatedUsername;
            var orders = orderService.GetOrdersByUserName(authenticatedUsername);
            return Ok(orders);
        }

        [Route("{orderNumber}", Name = "GetOrder")]
        [HttpGet]
        public IHttpActionResult GetOrder(string orderNumber)
        {
            string authenticatedUsername = this.AuthenticatedUsername;
            var order = orderService.GetOrderByUserNameAndOrderNumber(authenticatedUsername, orderNumber);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [Route("")]
        [HttpPost]
        [ModelStateValidationFilter]
        public IHttpActionResult CreateOrder(OrderDTO order)
        {
            orderService.CreateOrder(order);
            string location = Url.Link("GetOrder", new { orderNumber = order.OrderNumber });
            return Created(location, order);            
        }

        [Route("all")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IHttpActionResult GetAllOrders()
        {
            var orders = orderService.GetAllOrders();
            return Ok(orders);
        }
    }
}