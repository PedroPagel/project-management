using AutoMapper;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;

namespace Project.Management.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemDto>().ReverseMap();
            CreateMap<Domain.Entities.Project, ProjectDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<ProjectMember, ProjectMemberDto>().ReverseMap();
        }
    }
}
