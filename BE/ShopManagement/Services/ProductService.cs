using AutoMapper;
using Common.Exceptions.CustomExceptions.ShopManagementExceptions;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.DataBase;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Common.Models.Outbound;
using ShopManagement.Database.DataAccess;
using ShopManagement.Services.Contracts;

namespace ShopManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductHelper _productHelper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IMapper mapper, IProductHelper productHelper, IUnitOfWork uniteOfWork)
        {
            _mapper = mapper;
            _productHelper = productHelper;
            _unitOfWork = uniteOfWork;
        }

        public async Task<Product> AddNewProduct(NewProduct newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);
            product.Image = await _productHelper.ParseProductImageToBytes(newProduct.Image);
            await _unitOfWork.Products.Insert(product);
            await _unitOfWork.SaveChanges();
            return product;
        }

        public async Task<string> Delete(DeleteProduct product)
        {
            var exist = await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(product.ProductId.ToLower()));
            if(exist != null)
            {
                if (!exist.SellerId.ToString().ToLower().Equals(product.SellerId.ToLower()))
                {
                    throw new UnauthorizedProductUpdateException("You are not authorized to change informations about this product.");
                }
                await _unitOfWork.Products.Delete(exist);
                await _unitOfWork.SaveChanges();
                return exist.Name;
            }
            throw new KeyNotFoundException("This product can't be found in our system.");
        }

        public async Task<List<Product>> GetAll()
        {
            return (await _unitOfWork.Products.GetAll()).Where(p => p.Quantity > 0).ToList();
        }

        public async Task<List<ProductView>> GetAllBySellerId(string sellerId)
        {
            var sellersProducts = _unitOfWork.Products.GetAll().Result.Where(p => p.SellerId.ToString().ToLower().Equals(sellerId.ToLower())).ToList();
            var productsAddapted = _mapper.Map<List<ProductView>>(sellersProducts);
            return productsAddapted;
        }

        public async Task<Product> Update(UpdateProduct product)
        {
            var productCurrentValues = await _unitOfWork.Products.FindAsync(p => p.Id.ToString().ToLower().Equals(product.ProductId.ToLower()));
            if(product.SellerId != null && !productCurrentValues.SellerId.ToString().ToLower().Equals(product.SellerId.ToLower()))
            {
                throw new UnauthorizedProductUpdateException("You are not authorized to change informations about this product.");
            }
            var productUpdatedValues = _mapper.Map<Product>(product);
            if (product.UpdatedImage != null)
                productUpdatedValues.Image = await _productHelper.ParseProductImageToBytes(product.UpdatedImage);
            else
                productUpdatedValues.Image = productCurrentValues.Image;

            await _unitOfWork.Products.Update(productUpdatedValues, productUpdatedValues.Id);
            await _unitOfWork.SaveChanges();

            return productUpdatedValues;
        }
    }
}
