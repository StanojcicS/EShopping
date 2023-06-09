using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using ShopManagement.Common.Models.Database;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Common.Models.Outbound;
using ShopManagement.Database.DataAccess;
using ShopManagement.Migrations;
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

            var orderProductsList = (await _unitOfWork.OrderProducts.GetAll()).Where(op => op.OrderId.ToString().ToLower().Equals(order.OrderId.ToLower()));
            foreach (var oneOrderProduct in orderProductsList)
            {
                var product = await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(oneOrderProduct.ProductId.ToString().ToLower()));
                product.Quantity += oneOrderProduct.ProductQuantity;
            }
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
            var usersOrders = (await _unitOfWork.Orders.GetAll()).Where(o => o.UserId.ToLower().Equals(userId.ToLower())).ToList();
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

        public async Task<List<OrderView>> GetAllOrdersForSeller(string userId)
        {   //sadrzi product id-eve ovog prodavca
            List<string> usersProductsIds = (await _unitOfWork.Products.GetAll()).Where(p => p.SellerId.ToString().ToLower().Equals(userId.ToLower())).Select(p => p.Id.ToString()).ToList();

            //sadrzi orderproduct-e gde su proizvodi ovog prodavca
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            List<OrderProduct> allOrderProducts = (await _unitOfWork.OrderProducts.GetAll()).ToList();
            foreach (var productId in usersProductsIds)
            {
                foreach (var orderProduct in allOrderProducts)
                {
                    if (orderProduct.ProductId.ToString().ToLower().Equals(productId.ToLower()))
                        orderProducts.Add(orderProduct);
                }
            }

            //sadrzi ordere gde su proizvodi ovog prodavca *napomena total price nije adekvatan jer mogu se naci tudji proizvodi
            HashSet<Order> sellerOrders = new HashSet<Order>();
            foreach (var orderProduct in orderProducts)
            {
                sellerOrders.Add(await _unitOfWork.Orders.FindAsync(o => o.Id.ToString().ToLower().Equals(orderProduct.OrderId.ToString().ToLower())));
            }

            List<OrderView> addaptedOrders = _mapper.Map<List<OrderView>>(sellerOrders);
            foreach (var adOrder in addaptedOrders)
            {
                adOrder.NumberOfProducts = 0;
                adOrder.TotalPrice = 0;
                foreach (var orderProduct in orderProducts)
                {
                    if (adOrder.Id.ToLower().Equals(orderProduct.OrderId.ToString().ToLower()))
                    {
                        adOrder.NumberOfProducts += orderProduct.ProductQuantity;
                        adOrder.TotalPrice += orderProduct.ProductQuantity * (await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(orderProduct.ProductId.ToString().ToLower()))).Price;
                    }
                }
            }

            return addaptedOrders;

        }

        public async Task<List<OrderView>> GetNewOrdersForSeller(string userId)
        {
            var allOrders = await GetAllOrdersForSeller(userId);
            var newOrders = allOrders.Where(o => DateTime.Parse(o.OrderedAt).AddHours(1) > DateTime.Now).ToList(); 
            return newOrders;
        }

        public async Task<OrderDetailsView> SellerOrderDetails(OrderDetailsInbound orderDetailsInbound)
        {
            var order = await _unitOfWork.Orders.FindAsync(o => o.Id.ToString().ToLower().Equals(orderDetailsInbound.OrderId));
            List<string> usersProductsIds = (await _unitOfWork.Products.GetAll()).Where(p => p.SellerId.ToString().ToLower().Equals(orderDetailsInbound.UserId.ToLower())).Select(p => p.Id.ToString()).ToList();
            //sadrzi orderproduct-e gde su proizvodi ovog prodavca
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            List<OrderProduct> allOrderProducts = (await _unitOfWork.OrderProducts.GetAll()).ToList();
            foreach (var productId in usersProductsIds)
            {
                foreach (var orderProduct in allOrderProducts)
                {
                    if (orderProduct.ProductId.ToString().ToLower().Equals(productId.ToLower()) && orderProduct.OrderId.ToString().ToLower().Equals(orderDetailsInbound.OrderId.ToLower()))
                        orderProducts.Add(orderProduct);
                }
            }

            //sad order details view dodajemo adekvatnu listu produkta za taj order, ali samo od ovog prodavca
            List<ProductView> products = new List<ProductView>();
            foreach (var orderProduct in orderProducts)
            {
                var productToAdd = _mapper.Map<ProductView>(await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(orderProduct.ProductId.ToString().ToLower())));
                productToAdd.Quantity = orderProduct.ProductQuantity;
                products.Add(productToAdd);
            }

            var adOrder = _mapper.Map<OrderView>(order);
             
            adOrder.TotalPrice = 0;
            foreach (var orderProduct in orderProducts)
            {
                if (adOrder.Id.ToLower().Equals(orderProduct.OrderId.ToString().ToLower()))
                { 
                    adOrder.TotalPrice += orderProduct.ProductQuantity * (await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(orderProduct.ProductId.ToString().ToLower()))).Price;
                }
            }

            OrderDetailsView returnValue = new OrderDetailsView()
            {
                Address = order.DeliveryAddress,
                Comment = order.Comment,
                DeliveringTime = order.DeliveringTime.ToString(),
                IsCanceled = order.OrderDeclinedd,
                OrderedAt = order.OrderedAt.ToString(),
                TotalPrice = adOrder.TotalPrice,
                Products = products
            };
            return returnValue;
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
                IsCanceled = order.OrderDeclinedd,
                OrderedAt = order.OrderedAt.ToString(),
                TotalPrice = order.TotalPrice,
                Products = products
            };
            return returnValue;
        }

    }
}
