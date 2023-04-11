using AutoMapper;
using UserManagement.Models.DataBase;
using UserManagement.Models.Incoming;

namespace UserManagement.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<IncomingUser, User>();
        }
    }
}
