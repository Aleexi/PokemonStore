using AutoMapper;
using Contracts.PublicClasses;
using SearchService.Entities;

namespace SearchService.RequestHelpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<AttackContract, Attack>();
        }
    }
}
