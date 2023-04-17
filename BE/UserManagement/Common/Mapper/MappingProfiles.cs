using AutoMapper;
using UserManagement.Common.Models.DataBase;
using UserManagement.Common.Models.Incoming;

namespace UserManagement.Common.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IncomingUser, User>();
        }
    }
}
