using AutoMapper;
using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.Inbound;
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
            databaseOrder.DeliveringDateTime = await _orderHelper.GenerateDeliveringTime();
            
            await _unitOfWork.Orders.Insert(databaseOrder);
            await _unitOfWork.SaveChanges(); 

            return databaseOrder; 
        }
    }
}
