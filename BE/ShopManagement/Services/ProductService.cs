using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.DataBase;
using ShopManagement.Common.Models.Inbound;
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
    }
}
