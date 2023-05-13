using AutoMapper;
using ShopManagement.Common.Models.DataBase;
using ShopManagement.Common.Models.Inbound;

namespace ShopManagement.Common.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<NewProduct, Product>()
                .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => new Guid(src.SellerId)))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new Guid()));
        }
    }
}
