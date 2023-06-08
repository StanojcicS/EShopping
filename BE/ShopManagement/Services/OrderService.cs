using AutoMapper;
using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Common.Models.Outbound;
using ShopManagement.Database.DataAccess;
using ShopManagement.Services.Contracts;

namespace ShopManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderHelper _orderHelper;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, IOrderHelper orderHelper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderHelper = orderHelper;
        }

        public async Task<bool> CancelOrder(CancelOrder order)
        {
            var existingOrder = await _unitOfWork.Orders.FindAsync(o => o.Id.ToString().ToLower().Equals(order.OrderId.ToLower()));
            if(existingOrder == null || existingOrder.OrderDeclinedd == true || !existingOrder.UserId.ToLower().Equals(order.UserId.ToLower()) || existingOrder.OrderedAt.AddHours(1) < DateTime.Now)
            {
                return false;
            }
            existingOrder.OrderDeclinedd = true;
            await _unitOfWork.SaveChanges();
            return true;
            
        }

        public async Task<Order> CreateNewOrder(NewOrder order)
        {
            var databaseOrder = _mapper.Map<Order>(order);

            foreach (var productId in order.ProductIds)
            { 
                var doesProductAlreadyExistsInThisOrder = databaseOrder.OrderProducts.Find(op => op.ProductId.ToString().ToLower().Equals(productId.ToLower()));
                var product = await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(productId.ToLower()));
                if(doesProductAlreadyExistsInThisOrder != null)
                {
                    databaseOrder.OrderProducts.Find(op => op.ProductId.ToString().ToLower().Equals(productId.ToLower())).ProductQuantity++;
                    product.Quantity--;
                }
                else
                {
                    databaseOrder.OrderProducts.Add(new OrderProduct()
                    {
                        OrderId = databaseOrder.Id,
                        ProductId = new Guid(productId),
                        ProductQuantity = 1,
                        Product = await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(productId.ToLower())),
                        Order = databaseOrder
                    });
                    product.Quantity--;
                } 
            }
             

            databaseOrder.TotalPrice = await _orderHelper.GetTotalPrice(databaseOrder.OrderProducts);
            databaseOrder.DeliveringTime = await _orderHelper.GenerateDeliveringTime();
            
            await _unitOfWork.Orders.Insert(databaseOrder);
            await _unitOfWork.SaveChanges(); 

            return databaseOrder; 
        }

        public async Task<List<OrderView>> GetAllByBuyerId(string userId)
        { 
            var usersOrders = (await _unitOfWork.Orders.GetAll()).Where(o => o.UserId.ToLower().Equals(userId.ToLower()) && o.OrderDeclinedd == false).ToList();
            var mappedUserOrders = _mapper.Map<List<OrderView>>(usersOrders);
            foreach (var order in mappedUserOrders)
            {
                var productsInOrder = (await _unitOfWork.OrderProducts.GetAll()).Where(op => op.OrderId.ToString().ToLower().Equals(order.Id.ToLower()));
                foreach (var differentProduct in productsInOrder)
                {
                    order.NumberOfProducts += differentProduct.ProductQuantity;
                }

            }
            return mappedUserOrders;
        }

        public async Task<OrderDetailsView> OrderDetails(OrderDetailsInbound orderDetailsInbound)
        {
            var order = await _unitOfWork.Orders.FindAsync(o => o.Id.ToString().ToLower().Equals(orderDetailsInbound.OrderId));
            var productIds = (await _unitOfWork.OrderProducts.GetAll()).Where(op => op.OrderId.ToString().ToLower().Equals(order.Id.ToString().ToLower()));
            List<ProductView> products = new List<ProductView>();
            foreach (var product in productIds)
            {
                var productToAdd = _mapper.Map<ProductView>(await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(product.ProductId.ToString().ToLower())));
                productToAdd.Quantity = product.ProductQuantity;
                products.Add(productToAdd);
            }
            OrderDetailsView returnValue = new OrderDetailsView()
            {
                Address = order.DeliveryAddress,
                Comment = order.Comment,
                DeliveringTime = order.DeliveringTime.ToString(),
                OrderedAt = order.OrderedAt.ToString(),
                TotalPrice = order.TotalPrice,
                Products = products
            };
            return returnValue;
        }
    }
}
