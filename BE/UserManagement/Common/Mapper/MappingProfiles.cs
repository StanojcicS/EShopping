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
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            
            CreateMap<User, LogedInUser>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

            CreateMap<UpdateUser, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
        }
    }
}
