using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Models.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveType, ReadOnlyVM>();
            CreateMap<LeaveType, CreateVM>().ReverseMap();
            CreateMap<LeaveType, EditVM>().ReverseMap();
        }
    }
}
