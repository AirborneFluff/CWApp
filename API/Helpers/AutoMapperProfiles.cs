namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Part, PartDto>();
            CreateMap<NewPartDto, Part>();
        }
    }
}