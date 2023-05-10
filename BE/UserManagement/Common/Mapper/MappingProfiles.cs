using AutoMapper;
using UserManagement.Common.Enumerations;
using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Inbound;
using UserManagement.Common.Models.Outbound;

namespace UserManagement.Common.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterUser, User>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => Enum.Parse(typeof(UserType), src.UserType)))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.Verified, opt => opt.MapFrom(src => 0));
            
            CreateMap<User, LogedInUser>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.Verified, opt => opt.MapFrom(src => src.Verified == 1 ? "Your account is verified." 
                        : (src.Verified == null ? "Verification request is denied." 
                        : "Your account is not verified.")));

            CreateMap<UpdateUser, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());


            CreateMap<User, SellerView> ()
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePicture == null ? null 
                : $"data:image/jpg;base64,{Convert.ToBase64String(src.ProfilePicture)}"));
        }
    }
}
